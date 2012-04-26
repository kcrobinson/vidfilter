using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class EngineResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string OutPath { get; set; }
        EngineRequest OriginalRequest { get; set; }
    }
}
