using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VidFilter.Engine
{
    public class Movie : BaseFile
    {
        public Movie(FileInfo fileInfo) : base(fileInfo) { }

        public Movie(NormalizedMovie normalizedMovie)
        {
            _FileInfo = normalizedMovie.GetFileInfo();
            BitRate = normalizedMovie.BitRate;
            FrameRate = normalizedMovie.FrameRate;
            PlayLength = normalizedMovie.PlayLength;
        }

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
