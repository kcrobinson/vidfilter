using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VidFilter.Repository.Model;

namespace VidFilter.Repository
{
    public interface IDatabase : IDisposable
    {
        /// <summary>
        /// Inserts a movie record into the database.
        /// </summary>
        /// <param name="movie">The movie record to be added.</param>
        /// <returns>Information about the execution of the method.</returns>
        OperationStatus InsertMovie(Movie movie);

        OperationStatus InsertImage(Image image);

        /// <summary>
        /// Attempts to ascertain if the database is reachable.
        /// </summary>
        /// <returns>Information about the execution of the method.</returns>
        OperationStatus CheckConnection();

        IEnumerable<FriendlyName> QueryAllMovies(bool allowException = false);

        DenormalizedMovie QueryMovie(string Id, bool allowException = false);

        OperationStatus DeleteMovie(string Id);
    }
}
