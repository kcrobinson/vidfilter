﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace VidFilter.Engine
{
    public class Image : BaseFile
    {
        [JsonConstructor]
        public Image(string FullName) : base(FullName) { }

        public Image(FileInfo fileInfo) : base(fileInfo) { }

        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public string ColorSpaceId { get; set; }
    }
}
