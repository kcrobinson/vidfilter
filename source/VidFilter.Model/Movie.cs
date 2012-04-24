using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public class Movie : File, IMergeable
    {
        public string ParentMovieId { get; set; }
        public string RootMovieId { get; set; }
        public int BitRate { get; set; }
        public int FrameRate { get; set; }
        public decimal PlayLength { get; set; }
        public string SampleFrameId { get; set; }
        public string ResolutionTheoreticalId { get; set; }
        public string ResolutionActualId { get; set; }
        public string ColorSpaceId { get; set; }

        public bool IsRootMovie
        {
            get
            {
                return this.ParentMovieId == null;
            }
        }

        public virtual void Update(object newObject, bool IncludeId = false)
        {
            Movie newMovie = newObject as Movie;
            if (newMovie == null)
            {
                throw new NotSupportedException("Cannot update Movie record with non-record object.");
            }
            base.Merge(newMovie, IncludeId);
            this.ParentMovieId = newMovie.ParentMovieId;
            this.RootMovieId = newMovie.RootMovieId;
            this.BitRate = newMovie.BitRate;
            this.FrameRate = newMovie.FrameRate;
            this.PlayLength = newMovie.PlayLength;
            this.SampleFrameId = newMovie.SampleFrameId;
            this.ResolutionTheoreticalId = newMovie.ResolutionTheoreticalId;
            this.ResolutionActualId = newMovie.ResolutionActualId;
            this.ColorSpaceId = newMovie.ColorSpaceId;
        }
    }
}
