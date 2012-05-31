using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using Raven.Client;
using Raven.Client.Linq;
using Raven.Client.Indexes;

namespace VidFilter.Engine
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
                    IndexCreation.CreateIndexes(typeof(Movies_ByFriendlyName).Assembly, _DocumentStore);
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
                    Movie recordLookup = session.Load<Movie>(movie.Id);
                    if (recordLookup != null)
                    {
                        opStatus.Message = "Movie record already existed. Record not updated.";
                    }
                    else
                    {
                        session.Store(movie);
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

            opStatus.ResultingFriendlyName = new FriendlyName(movie.Id, movie.FileName);
            opStatus.IsSuccess = true;
            return opStatus;
        }

        public OperationStatus InsertImage(Image image)
        {
            OperationStatus opStatus = new OperationStatus();

            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    Image recordLookup = session.Load<Image>(image.Id);
                    if (recordLookup != null)
                    {
                        opStatus.Message = "Image record already existed. Record not updated.";
                    }
                    else
                    {
                        session.Store(image);
                        opStatus.NumRecordsAffected++;
                    }
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                opStatus.HandleException("Failure while adding Image record", ex);
                return opStatus;
            }

            opStatus.IsSuccess = true;
            return opStatus;
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
                        loadedRecord.CodeName = colorspace.CodeName;
                        loadedRecord.BitsPerPixel = colorspace.BitsPerPixel;
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
    
        public IEnumerable<FriendlyName> QueryAllMovies(bool allowException = false)
        {
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    return session.Query<Movie, Movies_ByFriendlyName>().As<FriendlyName>();
                }
            }
            catch(Exception ex)
            {
                if (allowException) 
                    throw ex;
                return new List<FriendlyName>(0);
            }
        }

        public IEnumerable<Colorspace> QueryAllColorspaces(bool allowException = false)
        {
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    return session.Query<Colorspace>();
                }
            }
            catch (Exception ex)
            {
                if (allowException)
                    throw ex;
                return new List<Colorspace>(0);
            }
        }

        public Movie QueryMovie(string id, bool allowException = false)
        {
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    return session.Load<Movie>(id);
                }
            }
            catch (Exception ex)
            {
                if (allowException) 
                    throw ex;
                return null;
            }
        }

        public Colorspace QueryColorspace(string id, bool allowException = false)
        {
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    return session.Load<Colorspace>(id);
                }
            }
            catch (Exception ex)
            {
                if (allowException)
                    throw ex;
                return null;
            }
        }
    }
}
