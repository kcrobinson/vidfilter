using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class Image : BaseFile
    {
        public string ResolutionId { get; set; }
        public string ColorSpaceId { get; set; }
    }
}
