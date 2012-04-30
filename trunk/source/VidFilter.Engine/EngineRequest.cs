﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VidFilter.Engine
{
    public class EngineRequest
    {
        public FileInfo InputFile { get; set; }

        private string _OutputPath;
        public string OutputPath
        {
            get
            {
                if(_OutputPath != null)
                {
                    return _OutputPath;
                }
                if(InputFile == null)
                {
                    return null;
                }

                StringBuilder sb = new StringBuilder(InputFile.DirectoryName + "\\");

                sb.Append(InputFile.Name);
                if (!string.IsNullOrWhiteSpace(OutputColorspace))
                    sb.Append("_" + OutputColorspace);
                if (OutputFrameRate > 0)
                    sb.Append("_" + OutputFrameRate + "fps");
                if (OutputWidth > 0 && OutputHeight > 0)
                    sb.Append("_" + OutputWidth + "x" + OutputHeight);

                sb.Append(".avi");
                return sb.ToString();
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
        public bool PadToOriginal { get; set; }

    }
}