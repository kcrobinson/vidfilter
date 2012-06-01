﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class ProbeResult
    {
        public ProbeRequest OriginalRequest { get; set; }
        public string ProcessArguments { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Colorspace { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public int FrameRate { get; set; }
        public decimal PlayLength { get; set; }
        public int BitRate { get; set; }
    }
}
