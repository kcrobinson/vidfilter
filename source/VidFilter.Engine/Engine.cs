using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace VidFilter.Engine
{
    class Engine : IEngine
    {
        private const string inputVideoFormat = "-i {0}";
        private const string overwriteFlag = "-y";
        private const string framerateFormat = "-r {0}";
        private const string sizeFormat = "-s {0}x{1}";
        private const string colorspaceFormat = "-pix_fmt {0}";
        private const string frameprocessFormat = "-vf {0}";
        private const string scaleFormat = "scale={0}*iw/{1}:{0}*ih/{1}";
        private const string cropFormat = "crop={0}:{1}:{2}:{3}";
        private const string padFormat = "pad={0}:{1}:{2}:{3}";

        public EngineResult ProcessRequest(EngineRequest request)
        {
            EngineResult opStatus = new EngineResult();

            ProcessStartInfo startInfo = new ProcessStartInfo("ffmpeg.exe");
            startInfo.Arguments = "";
            Process.Start(startInfo);
            return opStatus;
        }

    }
}
