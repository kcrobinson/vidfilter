using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VidFilter.Engine
{
    public class NormalizedMovie : BaseFile
    {
        public NormalizedMovie(FileInfo fileInfo) : base(fileInfo) { }

        public string ParentMovieId { get; set; }
        public int BitRate { get; set; }
        public int FrameRate { get; set; }
        public decimal PlayLength { get; set; }
        public string SampleFrameId { get; set; }
        public string ResolutionTheoreticalId { get; set; }
        public string ResolutionActualId { get; set; }
        public string ColorSpaceId { get; set; }
    }
}
