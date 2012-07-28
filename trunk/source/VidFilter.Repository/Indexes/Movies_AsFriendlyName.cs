using System.Linq;
using Raven.Client.Indexes;
using VidFilter.Repository.Model;

namespace VidFilter.Repository.Indexes
{
    class Movies_AsFriendlyName : AbstractIndexCreationTask<Movie>
    {
        public Movies_AsFriendlyName()
        {
            Map = movies => movies.Select(movie => new { Id = movie.Id, Name = movie.FileName });

            TransformResults = (database, movies) => movies.Select(movie => new { Id = movie.Id, Name = movie.FileName });
        }
    }
}
