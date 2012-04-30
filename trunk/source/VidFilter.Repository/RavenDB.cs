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
    }
}
