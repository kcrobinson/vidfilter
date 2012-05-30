using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace VidFilter.Engine
{
    class Engine : IEngine
    {
        private const string inputVideoFormat = "-i {0}";
        private const string overwriteFlag = "-y";
        private const string noOverwriteFlag = "-n";
        private const string framerateFormat = "-r {0}";
        private const string sizeFormat = "-s {0}x{1}";
        private const string colorspaceFormat = "-pix_fmt {0}";
        private const string codecFormat = "-vcodec {0}";
        private const string frameprocessFormat = "-vf {0}";
        private const string scaleFormat = "scale={0}*iw/{1}:{0}*ih/{1}";
        private const string cropFormat = "crop={0}:{1}:{2}:{3}";
        private const string padFormat = "pad={0}:{1}:{2}:{3}";

        public EngineResult ProcessRequest(EngineRequest request)
        {
            EngineResult engResult = new EngineResult()
            {
                OriginalRequest = request
            };

            ProcessStartInfo startInfo = new ProcessStartInfo("ffmpeg.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = engResult.ProcessArguments = ProcessArguments(request);

            Process process = null;
            int exitCode;
            try
            {
                process = Process.Start(startInfo);
                DateTime startTime = DateTime.Now;
                TimeSpan timeout = new TimeSpan(0, 0, 10);
                while (!process.HasExited && DateTime.Now - startTime < timeout) { }
                if (!process.HasExited)
                {
                    process.Kill();
                    throw new Exception("Engine timeout");
                }
                engResult.StdError = process.StandardError.ReadToEnd();
                engResult.StdOutput = process.StandardOutput.ReadToEnd();
                exitCode = process.ExitCode;
            }
            catch(Exception ex)
            {
                engResult.HandleException("Exception thrown while processing engine request", ex);
                return engResult;
            }
            finally
            {
                if (process != null)
                    process.Dispose();
            }

            switch(exitCode)
            {
                case 0:
                    FileInfo outFile = new FileInfo(request.OutputPath);
                    if (outFile.Exists && outFile.LastWriteTime > DateTime.Now.AddSeconds(-30))
                    {
                        if (request.OutputFrameRate > 0)
                            engResult.OutFramerate = request.OutputFrameRate;
                        else if (request.InputFrameRate > 0)
                            engResult.OutFramerate = request.InputFrameRate;

                        if (request.OutputWidth > 0 && request.OutputHeight > 0)
                        {
                            engResult.OutWidth = request.OutputWidth;
                            engResult.OutHeight = request.OutputHeight;
                        }
                        else if (request.InputWidth > 0 && request.InputHeight > 0)
                        {
                            engResult.OutHeight = request.InputHeight;
                            engResult.OutWidth = request.InputWidth;
                        }
                        engResult.IsSuccess = true;
                        engResult.OutFile = outFile;
                    }
                    else
                    {
                        engResult.IsSuccess = false;
                        engResult.Message = "Program mysteriously failed. Output file doesn't exist or wasn't overwritten.";
                    }
                    break;
                default:
                    engResult.Message = "Processing engine failed. Exited with status code: " + exitCode;
                    engResult.IsSuccess = false;
                    break;
            }
            return engResult;
        }

        private string ProcessArguments(EngineRequest request)
        {
            List<string> argumentList = new List<string>();

            argumentList.Add(ProcessArgument(framerateFormat, IntGreaterThanZero, request.InputFrameRate));
            argumentList.Add(ProcessArgument(sizeFormat, IntGreaterThanZero, request.InputWidth, request.InputHeight));
            argumentList.Add(ProcessArgument(inputVideoFormat, ObjectNotNull, request.InputFile.FullName));
            argumentList.Add(ProcessArgument(colorspaceFormat, ObjectNotNull, request.OutputColorspace));
            argumentList.Add(ProcessArgument(codecFormat, ObjectNotNull, request.OutputCodec));
            argumentList.Add(ProcessArgument(framerateFormat, IntGreaterThanZero, request.OutputFrameRate));
            argumentList.Add(ProcessArgument(sizeFormat, IntGreaterThanZero, request.OutputWidth, request.OutputHeight));

            argumentList.Add(overwriteFlag);

            argumentList.Add(request.OutputPath);
            
            argumentList.RemoveAll(arg => string.IsNullOrWhiteSpace(arg));
            return string.Join(" ", argumentList);
        }

        private string ProcessArgument(string argumentFormat, AddArgumentCondition addArgument, params object[] values)
        {
            if (addArgument(values))
            {
                return string.Format(argumentFormat, values);
            }
            return null;
        }

        #region AddArgumentConditions
        
        private delegate bool AddArgumentCondition(object[] values);

        private bool IntGreaterThanZero(object[] values)
        {
            foreach (object value in values)
            {
                int? val = value as int?;
                if (!val.HasValue || val.Value <= 0)
                    return false;
            }
            return true;
        }

        private bool ObjectNotNull(object[] values)
        {
            foreach (object value in values)
            {
                if (value == null)
                    return false;
            }
            return true;
        }
        #endregion
        

    }
}
