using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;

namespace VidFilter.Engine
{
    class Movies_ByFriendlyName : AbstractIndexCreationTask<Movie>
    {
        public Movies_ByFriendlyName()
        {
            Map = movies => movies.Select(movie => new { Id = movie.Id, Name = movie.FileName });

            TransformResults = (database, movies) => movies.Select(movie => new { Id = movie.Id, Name = movie.FileName });
        }
    }
}
