using System;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class EngineRequest
    {
        public string InputFileName { get; set; }
        public string InputDirectory { get; set; }

        private string _InputPath;
        public string InputPath
        {
            get
            {
                if (_InputPath == null)
                {
                    // TODO: validation
                    _InputPath = String.Join("\\", InputDirectory, InputFileName);
                }
                return _InputPath;
            }
        }

        private string _OutputPath;
        public string OutputPath
        {
            get
            {
                if(_OutputPath != null)
                {
                    return _OutputPath;
                }

                if (_OutputPath == null)
                {
                    // Some logic for creating an output file name if none is specified
                    StringBuilder sb = new StringBuilder(InputDirectory + "\\");

                    string fileName;
                    string extension;
                    string[] fileNameSplit = InputFileName.Split('.');
                    if (fileNameSplit.Count() < 2)
                    {
                        fileName = InputFileName;
                        extension = ".avi";
                    }
                    else
                    {
                        fileName = string.Join(".", fileNameSplit.Take(fileNameSplit.Count() - 1));
                        extension = "." + fileNameSplit.Last();
                    }
                    sb.Append(fileName);
                    if (!string.IsNullOrWhiteSpace(OutputColorspace))
                        sb.Append("_" + OutputColorspace);
                    if (OutputFrameRate > 0)
                        sb.Append("_" + OutputFrameRate + "fps");
                    if (OutputWidth > 0 && OutputHeight > 0)
                        sb.Append("_" + OutputWidth + "x" + OutputHeight);

                    sb.Append(extension);
                    _OutputPath = sb.ToString();
                }
                return _OutputPath;
            }
            set
            {
                _OutputPath = value;
            }
        }

        public int InputFrameRate { get; set; }
        public int InputWidth { get; set; }
        public int InputHeight { get; set; }

        public string OutputColorspace { get; set; }
        public string OutputCodec { get; set; }
        public int OutputFrameRate { get; set; }
        public int OutputWidth { get; set; }
        public int OutputHeight { get; set; }
        // public bool PadToOriginal { get; set; }
    }
}
