using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VidFilter.Engine
{
    public class Image : BaseFile
    {
        public Image(string filePath) : base(filePath) { }
        public Image(FileInfo fileInfo) : base(fileInfo) { }

        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public string ColorSpaceId { get; set; }
    }
}
