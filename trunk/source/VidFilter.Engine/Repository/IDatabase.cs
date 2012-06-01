using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
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

        /// <summary>
        /// Inserts a colorspace record if it doesn't already exist (based on Id), or updates the existing record if it does.
        /// </summary>
        /// <param name="colorspace"></param>
        /// <returns></returns>
        OperationStatus InsertOrUpdateColorspace(Colorspace colorspace);

        IEnumerable<FriendlyName> QueryAllMovies(bool allowException = false);

        IEnumerable<Colorspace> QueryAllColorspaces(bool allowException = false);

        DenormalizedMovie QueryMovie(string Id, bool allowException = false);
    }
}
