﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class ImageResult
    {
        public ImageRequest OrigRequest { get; set; }
        public string OutFile { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}