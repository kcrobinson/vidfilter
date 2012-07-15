using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using VidFilter.Repository.Model;

namespace VidFilter.Repository.Indexes
{
    class DenormalizedMovie_ByMovie : AbstractIndexCreationTask<Movie>
    {
        public DenormalizedMovie_ByMovie()
        {
            Map = movies => movies.Select(movie => new {movie.Id});

            TransformResults =
                (database, movies) => from movie in movies
                                      let image = database.Load<Image>(movie.SampleFrameId)
                                      let colorspace = database.Load<Colorspace>(movie.ColorSpaceId)
                                      select new
                                      { 
                                          MovieId = movie.Id,
                                          FileName = movie.FileName,
                                          Directory = movie.DirectoryPath,
                                          FrameRate = movie.FrameRate,
                                          ResolutionWidth = movie.ResolutionWidth,
                                          ResolutionHeight = movie.ResolutionHeight,
                                          Colorspace = colorspace,
                                          PlayLength = movie.PlayLength,
                                          SampleImagePath = image.FullName
                                      };
        }
    }
}
