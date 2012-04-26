using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VidFilter.Engine
{
    public class EngineRequest
    {
        public FileInfo InputFile { get; set; }
        public string OutputPath { get; set; }

        public bool OverWriteOutput { get; set; }
        
        public int InputFrameRate { get; set; }
        public int InputWidth { get; set; }
        public int InputHeight { get; set; }

        public string OutputColorspace { get; set; }
        public string OutputCodec { get; set; }
        public int OutputFrameRate { get; set; }
        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; }
        public bool PadToOriginal { get; set; }

    }
}
