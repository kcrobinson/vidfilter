using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class DenormalizedMovie
    {
        public Movie Movie { get; set; }
        public Image Image { get; set; }
        public Colorspace Colorspace { get; set; }
    }
}
