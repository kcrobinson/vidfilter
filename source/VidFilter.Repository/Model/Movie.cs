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
    }
}
