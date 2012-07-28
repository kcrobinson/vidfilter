using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Commands;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using VidFilter.Repository.Indexes;
using VidFilter.Repository.Model;

namespace VidFilter.Repository
{
    public class RavenDB : IDatabase
    {
        public RavenDB(params KeyValuePair<string, object>[] options)
        {
            if (options == null)
                throw new ArgumentNullException("options");

            _ConnectionPath = null;
            _DatabaseType = null;

            foreach(KeyValuePair<string, object> option in options)
            {
                switch (option.Key.ToLower())
                {
                    case "connectionpath":
                        ParseOptionString(option.Value, "ConnectionPath", ref _ConnectionPath);
                        break;
                    case "databasetype":
                        ParseOptionString(option.Value, "DatabaseType", ref _DatabaseType);
                        break;
                }
            }

            if (_ConnectionPath == null)
            {
                throw new ArgumentException("ConnectionPath not specified");
            }
            if (_DatabaseType == null)
            {
                throw new ArgumentException("DatabaseType not specified");
            }
        }

        private string _ConnectionPath;
        private string _DatabaseType;

        private void ParseOptionString(object value, string paramName, ref string param)
        {
            if (param != null)
            {
                throw new ArgumentException(string.Format("{0} option specified more than once", paramName));
            }
            var val = value as string;
            if (val == null)
            {
                throw new ArgumentException(String.Format("Invalid {0} value. Null or not string.", paramName));
            }
            param = val;
        }

        private IDocumentStore _DocumentStore;
        private IDocumentStore DocumentStore { 
            get
            {
                if (_DocumentStore == null)
                {
                    switch(_DatabaseType.ToLower())
                    {
                        case "hosted":
                            _DocumentStore = new DocumentStore { Url = _ConnectionPath };
                            break;
                        case "embedded":
                            _DocumentStore = new EmbeddableDocumentStore { DataDirectory = _ConnectionPath, UseEmbeddedHttpServer = true };
                            break;
                    }
                    _DocumentStore.Initialize();
                    IndexCreation.CreateIndexes(typeof(Movies_AsFriendlyName).Assembly, _DocumentStore);
                    IndexCreation.CreateIndexes(typeof(DenormalizedMovie_ByMovie).Assembly, _DocumentStore);
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

        public IEnumerable<FriendlyName> QueryAllMovies(bool allowException = false)
        {
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    return session.Query<Movie, Movies_AsFriendlyName>().As<FriendlyName>();
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

        public DenormalizedMovie QueryMovie(string id, bool allowException = false)
        {
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    return session.Query<Movie, DenormalizedMovie_ByMovie>().Where(m => m.Id == id).As<DenormalizedMovie>().SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                if (allowException) 
                    throw ex;
                return null;
            }
        }

        public OperationStatus DeleteMovieAndImage(string movieId)
        {
            OperationStatus status = new OperationStatus();
            try
            {
                using (var session = DocumentStore.OpenSession())
                {
                    var movie = session.Load<Movie>(movieId);
                    if (movie != null)
                    {
                        session.Advanced.Defer(new DeleteCommandData { Key = movieId });
                        session.Advanced.Defer(new DeleteCommandData { Key = movie.SampleFrameId });
                    }
                    session.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                return OperationStatus.GetOperationStatusFromException("Failure deleting movie", ex);
            }
            status.IsSuccess = true;
            return status;
        }
    }
}
