using System;
using System.IO;

namespace VidFilter.Engine
{
    public class EngineResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ProcessArguments { get; set; }
        public string StdError { get; set; }
        public string StdOutput { get; set; }
        public Exception Exception { get; set; }
        public FileInfo OutFile { get; set; }
        public int OutWidth { get; set; }
        public int OutHeight { get; set; }
        public int OutFramerate { get; set; }
        public EngineRequest OriginalRequest { get; set; }

        public void HandleException(string message, Exception ex)
        {
            IsSuccess = false;
            Message = message;
            Exception = ex;
        }
    }
}
