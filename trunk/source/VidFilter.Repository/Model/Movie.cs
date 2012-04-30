using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VidFilter.Model
{
    public class Movie : BaseFile
    {
        public Movie(FileInfo fileInfo) : base(fileInfo) { }

        public FileInfo ParentMovie { get; set; }
        public int BitRate { get; set; }
        public int FrameRate { get; set; }
        public decimal PlayLength { get; set; }
        public FileInfo SampleFrame { get; set; }
        public Resolution ResolutionTheoretical { get; set; }
        public Resolution ResolutionActual { get; set; }
        public Colorspace ColorSpace { get; set; }

        public virtual void Update(object newObject)
        {
            Movie newMovie = newObject as Movie;
            if (newMovie == null)
            {
                throw new NotSupportedException("Cannot update Movie record with non-record object.");
            }
            base.MergeFrom(newMovie);
            this.BitRate = newMovie.BitRate;
            this.FrameRate = newMovie.FrameRate;
            this.PlayLength = newMovie.PlayLength;
            this.SampleFrame = newMovie.SampleFrame;
            this.ResolutionTheoretical = newMovie.ResolutionTheoretical;
            this.ResolutionActual = newMovie.ResolutionActual;
            this.ColorSpace = newMovie.ColorSpace;
        }
    }
}
