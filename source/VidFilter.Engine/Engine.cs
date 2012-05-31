using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace VidFilter.Engine
{
    class Engine : IEngine
    {
        private const string inputVideoFormat = "-i {0}";
        private const string overwriteFlag = "-y";
        private const string framerateFormat = "-r {0}";
        private const string sizeFormat = "-s {0}x{1}";
        private const string colorspaceFormat = "-pix_fmt {0}";
        private const string codecFormat = "-vcodec {0}";
        private const string frameprocessFormat = "-vf {0}";
        private const string scaleFormat = "scale={0}*iw/{1}:{0}*ih/{1}";
        private const string cropFormat = "crop={0}:{1}:{2}:{3}";
        private const string padFormat = "pad={0}:{1}:{2}:{3}";

        #region Process Methods
        public EngineResult ProcessRequest(EngineRequest request)
        {
            EngineResult engResult = new EngineResult()
            {
                OriginalRequest = request
            };

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Users\krobins\VidFilter\trunk\source\VidFilter.Engine\ffmpeg\bin\ffmpeg.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.Arguments = engResult.ProcessArguments = GetProcessArguments(request);

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

        private string GetProcessArguments(EngineRequest request)
        {
            List<string> argumentList = new List<string>();

            argumentList.Add(GetProcessArgument(framerateFormat, IntGreaterThanZero, request.InputFrameRate));
            argumentList.Add(GetProcessArgument(sizeFormat, IntGreaterThanZero, request.InputWidth, request.InputHeight));
            argumentList.Add(GetProcessArgument(inputVideoFormat, ObjectNotNull, request.InputFile.FullName));
            argumentList.Add(GetProcessArgument(colorspaceFormat, ObjectNotNull, request.OutputColorspace));
            argumentList.Add(GetProcessArgument(codecFormat, ObjectNotNull, request.OutputCodec));
            argumentList.Add(GetProcessArgument(framerateFormat, IntGreaterThanZero, request.OutputFrameRate));
            argumentList.Add(GetProcessArgument(sizeFormat, IntGreaterThanZero, request.OutputWidth, request.OutputHeight));

            argumentList.Add(overwriteFlag);

            argumentList.Add(request.OutputPath);
            
            argumentList.RemoveAll(arg => string.IsNullOrWhiteSpace(arg));
            return string.Join(" ", argumentList);
        }

        private string GetProcessArgument(string argumentFormat, AddArgumentCondition addArgument, params object[] values)
        {
            if (addArgument(values))
            {
                return string.Format(argumentFormat, values);
            }
            return null;
        }

        #endregion

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

        #region Probe Methods

        public ProbeResult ProbeVideoFile(ProbeRequest request)
        {
            ProbeResult result = new ProbeResult()
            {
                OriginalRequest = request
            };

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Users\krobins\VidFilter\trunk\source\VidFilter.Engine\ffmpeg\bin\ffprobe.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.Arguments = result.ProcessArguments = GetProbeArguments(request);

            Process process = null;
            int exitCode;
            string stdError;
            try
            {
                process = Process.Start(startInfo);
                DateTime startTime = DateTime.Now;
                TimeSpan timeout = new TimeSpan(0, 0, 10);
                while (!process.HasExited && DateTime.Now - startTime < timeout) { }
                if (!process.HasExited)
                {
                    process.Kill();
                    result.ErrorMessage = "ffprobe timeout";
                    return result;
                }
                stdError = process.StandardError.ReadToEnd();
                exitCode = process.ExitCode;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Exception thrown during ffprobe: " + ex.Message;
                return result;
            }
            finally
            {
                if (process != null)
                    process.Dispose();
            }

            if (exitCode != 0)
            {
                result.ErrorMessage = "ffprobe exited with non-zero exit code. Exit code: " + exitCode;
                return result;
            }

            var lines = stdError.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim());
            
            string streamLine = lines.FirstOrDefault(l => l.StartsWith("Stream #"));
            if (streamLine == null)
            {
                result.ErrorMessage = "Cannot find stream information in movie file";
                return result;
            }
            string[] parts = streamLine.Split(',').Select(l => l.Trim()).ToArray();
            result.Colorspace = parts[1].Trim();
            string[] resolution = parts[2].Trim().Split('x');
            result.ResolutionWidth = int.Parse(resolution[0]);
            result.ResolutionHeight = int.Parse(resolution[1]);
            string[] framerate = parts[3].Trim().Split();
            result.FrameRate = int.Parse(framerate[0]);

            string durationLine = lines.FirstOrDefault(l => l.StartsWith("Duration"));
            parts = durationLine.Split(',').Select(l => l.Trim()).ToArray();
            foreach (string part in parts)
            {
                string[] split = part.Split(':').Select(l => l.Trim()).ToArray();
                switch(split[0].ToLower())
                {
                    case "duration":
                        result.PlayLength = ParsePlayLength(split.Skip(1).ToArray());
                        if (result.PlayLength == 0)
                        {
                            result.ErrorMessage = "Failure to parse Play Length";
                            return result;
                        }
                        break;
                    case "bitrate":
                        result.BitRate = ParseBitrate(split[1]);
                        if (result.BitRate == 0)
                        {
                            result.ErrorMessage = "Failure to parse Bitrate";
                            return result;
                        }
                        break;
                }
            }
            result.IsSuccess = true;
            return result;
        }

        private string GetProbeArguments(ProbeRequest request)
        {
            List<string> argumentList = new List<string>();
            argumentList.Add("-print_format json");
            argumentList.Add(request.FilePath);
            return string.Join(" ", argumentList);
        }

        private decimal ParsePlayLength(string[] parts)
        {
            if (parts.Length != 3)
            {
                return 0;
            }
            int hours = 0, minutes = 0;
            decimal seconds = 0;

            bool success =
                int.TryParse(parts[0], out hours) &&
                int.TryParse(parts[1], out minutes) &&
                decimal.TryParse(parts[2], out seconds);
            if (!success)
            {
                return 0;
            }
            return hours * 3600 + minutes * 60 + seconds;
        }

        private int ParseBitrate(string str)
        {
            Match match = Regex.Match(str, @"(\d+) kb/s");
            if (!match.Success)
            {
                return 0;
            }
            int bitRate = 0;
            int.TryParse(match.Groups[1].Value, out bitRate);
            return bitRate;
        }

        #endregion

        #region Image Create

        public ImageResult CreateImage(ImageRequest request)
        {
            ImageResult result = new ImageResult()
            {
                OrigRequest = request
            };

            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Users\krobins\VidFilter\trunk\source\VidFilter.Engine\ffmpeg\bin\ffmpeg.exe");
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            string outFile;
            startInfo.Arguments = CreateImageArguments(request, out outFile);

            result.OutFile = outFile;

            Process process = null;
            int exitCode;
            string stdError;
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
                stdError = process.StandardError.ReadToEnd();
                exitCode = process.ExitCode;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "Exception thrown while creating image: " + ex.Message;
                return result;
            }
            finally
            {
                if (process != null)
                    process.Dispose();
            }
            if (exitCode != 0)
            {
                result.ErrorMessage = "ffprobe exited with non-zero exit code. Exit code: " + exitCode;
                result.ErrorMessage += "\r\n" + stdError;
                return result;
            }
            result.IsSuccess = true;
            return result;
        }

        private const string ProcessImageArgumentFormat = "-ss {0} -y -t 1 -i {1} -f mjpeg {2}";

        private string CreateImageArguments(ImageRequest request, out string outFile)
        {
            FileInfo fileInfo = new FileInfo(request.MoviePath);
            string snapshotTime = FormatTime(request.MovieLength / 2);
            outFile = fileInfo.FullName.TrimEnd(fileInfo.Extension.Skip(1).ToArray()) + "png";

            return string.Format(ProcessImageArgumentFormat, snapshotTime, fileInfo.FullName, outFile);
        }

        private string FormatTime(decimal input)
        {
            decimal timeLeft = input;

            int hours = 0, minutes = 0;
            decimal seconds = 0;

            hours = (int)(timeLeft / 3600);
            timeLeft -= hours * 3600;

            minutes = (int)(timeLeft / 60);
            timeLeft -= minutes * 3600;

            seconds = timeLeft;

            return string.Join(":", hours.ToString("00"), minutes.ToString("00"), seconds.ToString("00.00"));
        }

        #endregion
    }
}
