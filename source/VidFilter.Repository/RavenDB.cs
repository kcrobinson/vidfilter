using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using Raven.Client;
using VidFilter.Model;

namespace VidFilter.Repository
{
    public class RavenDB : IDatabase
    {
        private static readonly string ServerAddress = "http://localhost:8080";

        private static IDocumentStore _DocumentStore;
        private IDocumentStore DocumentStore { 
            get
            {
                if (_DocumentStore == null)
                {
                    _DocumentStore = new DocumentStore { Url = ServerAddress };
                    _DocumentStore.Initialize();
                }
                return _DocumentStore;
            }
        }

        /// <summary>
        /// Tries to query for a dummy result from the database.
        /// </summary>
        /// <returns>Information about the attempt. If successful, it can be infered that the 
        /// database is up and running. Otherwise, it would be prudent to send an error message to the user.</returns>
        public OperationStatus CheckConnection()
        {
            OperationStatus opStatus;
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    session.Query<object>().Where(a => a.Equals("dummyrequest")).ToList();
                }
            }
            catch (Exception ex)
            {
                opStatus = OperationStatus.GetOperationStatusFromException("Failure while checking database connection", ex);
            }
            opStatus = new OperationStatus();
            opStatus.IsSuccess = true;
            opStatus.Message = "Success checking database connection";
            return opStatus;
        }

        /// <summary>
        /// Implementation of IDisposable object. Safely disposes of connection to Raven Database implementation.
        /// </summary>
        public void Dispose()
        {
            if (_DocumentStore != null)
            {
                _DocumentStore.Dispose();
            }
        }

        public OperationStatus InsertMovie(Movie movie)
        {
            OperationStatus opStatus = new OperationStatus();

            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    NormalizedMovie normMovie = NormalizeMovie(session, movie);

                    NormalizedMovie recordLookup = session.Load<NormalizedMovie>(normMovie.Id);
                    if (recordLookup != null)
                    {
                        opStatus.Message = "Movie record already existed. Record not updated.";
                    }
                    else
                    {
                        session.Store(normMovie);
                        opStatus.NumRecordsAffected++;
                        opStatus.Message = "Movie record successfully inserted.";
                    }
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                opStatus.HandleException("Failure while adding Movie record", ex);
                return opStatus;
            }

            opStatus.IsSuccess = true;
            return opStatus;
        }

        private NormalizedMovie NormalizeMovie(IDocumentSession session, Movie movie)
        {
            NormalizedMovie normalizedMovie = new NormalizedMovie(movie.GetFileInfo());

            normalizedMovie.BitRate = movie.BitRate;
            normalizedMovie.FrameRate = movie.FrameRate;
            normalizedMovie.PlayLength = movie.PlayLength;

            if (movie.ColorSpace != null)
            {
                // Load or Insert colorspace
            }
            if (movie.ResolutionActual != null)
            {
                Resolution res1 = session.Load<Resolution>(movie.ResolutionActual.Id);
                if (res1 != null)
                {
                    normalizedMovie.ResolutionActualId = res1.Id;
                }
                else
                {
                    session.Store(movie.ResolutionActual);
                    normalizedMovie.ResolutionActualId = movie.ResolutionActual.Id;
                }
            }
            if (movie.ParentMovie != null)
            {
                NormalizedMovie parentMovie = session.Load<NormalizedMovie>(BaseFile.IdFromBaseFile(movie.ParentMovie));
                if (parentMovie != null)
                {
                    normalizedMovie.ParentMovieId = parentMovie.Id;
                }
            }

            return normalizedMovie;
        }
    
        public OperationStatus InsertOrUpdateColorspace(Colorspace colorspace)
        {
            OperationStatus opStatus = new OperationStatus();

            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    Colorspace loadedRecord = session.Load<Colorspace>(colorspace.Id);
                    if (loadedRecord != null)
                    {
                        loadedRecord.BitsPerPixel = colorspace.BitsPerPixel;
                        loadedRecord.IsMonochrome = colorspace.IsMonochrome;
                        loadedRecord.NumChannels = colorspace.NumChannels;
                    }
                    else
                    {
                        session.Store(colorspace);
                    }
                    session.SaveChanges();
                    opStatus.IsSuccess = true;
                    opStatus.NumRecordsAffected++;
                }
            }
            catch (Exception ex)
            {
                opStatus.HandleException("Failure updating colorspace", ex);
                return opStatus;
            }
            return opStatus;
        }
    }
}
