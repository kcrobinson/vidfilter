using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace VidFilter.Engine
{
    public class Movie : BaseFile
    {
        [JsonConstructor]
        public Movie(string FullName) : base(FullName) { }

        public Movie(FileInfo fileInfo) : base(fileInfo) { }

        public string ParentMovieId { get; set; }
        public int BitRate { get; set; }
        public int FrameRate { get; set; }
        public decimal PlayLength { get; set; }
        public string SampleFrameId { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public string ColorSpaceId { get; set; }

        [JsonIgnore]
        public string FormattedResolution
        {
            get
            {
                return ResolutionWidth + "x" + ResolutionHeight;
            }
        }

        [JsonIgnore]
        public string FormattedFramerate
        {
            get
            {
                return FrameRate + " FPS";
            }
        }
    }
}
