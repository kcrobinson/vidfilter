using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VidFilter.Repository;
using VidFilter.Model;
using System.IO;
using VidFilter.Engine;

namespace VidFilter.Console
{
    public class Program
    {
        public static readonly IDatabase Database = DatabaseFactory.GetDatabase();
        public static readonly IEngine Engine = EngineFactory.GetEngine();

        static void Main(string[] args)
        {
            WriteMessagesInColor(ConsoleColor.Cyan,
                    "VidFilter v. 0.1",
                    "Written by Kyle Parker-Robinson",
                    "kcrobinson@gmail.com",
                    "Part of The MANOME project at Eastern Washington University",
                    "Released under GPL",
                    "For more information, visit http://code.google.com/p/vidfilter");

            WriteMessages("Initializing Database");
            OperationStatus opStatus = Database.CheckConnection();
            WriteOperationStatus(opStatus);
            if (!opStatus.IsSuccess)
            {
                WriteMessages("Quitting");
                return;
            }
            WriteMessagesInColor(ConsoleColor.Cyan, "For usage, type 'help'");
            try
            {
                bool quit = false;
                while (!quit)
                {
                    string input = Prompt();
                    string[] split = input.Split();
                    switch(split[0].ToLower())
                    {
                        case "help":
                            PrintHelp();
                            break;
                        case "insert":
                            WriteMessages("Insert not supported yet");
                            break;
                        case "delete":
                            WriteMessages("Delete not supported yet");
                            break;
                        case "load":
                            opStatus = LoadFiles(split.Skip(1));
                            break;
                        case "process":
                            EngineResult engineResult = ProcessMovie(split.Skip(1));
                            break;
                        case "quit":
                            quit = true;
                            break;
                        default:
                            WriteMessages("Didn't understand '" + split[0] + "'");
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                List<string> exceptionMessages = new List<string>();
                Exception curEx = ex;
                while (curEx != null)
                {
                    WriteMessagesInColor(ConsoleColor.Red, ex.Message);
                    WriteMessagesInColor(ConsoleColor.Yellow, ex.StackTrace);
                    curEx = curEx.InnerException;
                }
            }
            finally
            {
                WriteMessages("Disposing database connection");
                Database.Dispose();
            }
            System.Console.Write("Program ended. Press <enter> to exit.");
            System.Console.ReadLine();
        }

        static void WriteMessagesInColor(ConsoleColor color, params string[] messages)
        {
            ConsoleColor origColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = color;
            WriteMessages(messages);
            System.Console.ForegroundColor = origColor;
        }

        static void WriteMessages(params string[] messages)
        {
            if (messages != null)
            {
                foreach (string message in messages)
                {
                    System.Console.WriteLine(message);
                }
            }
            System.Console.WriteLine();
        }

        static void WriteOperationStatus(OperationStatus opStatus)
        {
            if (opStatus.IsSuccess)
            {
                WriteMessages("Success" + " - " + opStatus.Message);
                return;
            }
            WriteMessagesInColor(ConsoleColor.Magenta, opStatus.Message, "Rows affected: " + opStatus.NumRecordsAffected);
            if(opStatus.ExceptionMessages != null && opStatus.ExceptionMessages.Count > 0)
            {
                for(int i=0; i<opStatus.ExceptionMessages.Count; i++)
                {
                    WriteMessagesInColor(ConsoleColor.Red, opStatus.ExceptionMessages.ElementAt(i));
                    WriteMessagesInColor(ConsoleColor.Yellow, opStatus.ExceptionStackTraces.ElementAt(i));
                }
                return;
            }
        }

        static void PrintHelp()
        {
            WriteMessagesInColor(ConsoleColor.Green,
                "VidFilter Help:",
                " 'insert' - Enters Insert mode",
                " 'delete {ID1 ID2 ... IDn}' - Delete records by given ID",
                " 'load {FolerPath}' - Loads all files recursively from a given directory",
                " 'help' - Displays this message",
                " 'quit' - Disconnects from Database and exits program");
        }

        static OperationStatus LoadFiles(IEnumerable<string> folderPaths)
        {
            OperationStatus opStatus;
            if (folderPaths == null)
            {
                opStatus = new OperationStatus();
                opStatus.IsSuccess = true;
                opStatus.Message = "0 files loaded. Given path was empty.";
                return opStatus;
            }

            foreach (string folder in folderPaths)
            {
                LoadFolder(folder);
            }
            opStatus = new OperationStatus();
            // opStatus.Message
            return opStatus;
        }

        static void LoadFolder(string folderPath)
        {
            DirectoryInfo directory = new DirectoryInfo(folderPath);
            if (!directory.Exists)
            {
                return;
            }
            foreach (FileInfo fileInfo in directory.EnumerateFiles())
            {
                switch(fileInfo.Extension)
                {
                    case "yuv":
                    case "mpeg":
                    case "avi":
                        LoadMovie(fileInfo);
                        break;
                    case "jpeg":
                    case "gif":
                        // LoadImage();
                        break;
                    default:
                        // Not understood filetype
                        break;
                }
            }
        }

        static void LoadMovie(FileInfo fileInfo)
        {
            Movie movie = new Movie();
            movie.CreationDateTime = fileInfo.CreationTime;
            movie.ModifyDateTime = fileInfo.LastWriteTime;
            movie.FileSizeInBytes = fileInfo.Length;
            movie.Name = fileInfo.Name;
            movie.Path = fileInfo.DirectoryName;
            //movie.BitRate
            //movie.ColorSpaceId
            //movie.FrameRate
            //movie.PlayLength
            //movie.ResolutionActualId
            //movie.ResolutionTheoreticalId
            //movie.RootMovieId
            //movie.SampleFrameId
            Database.Insert<Movie>(movie);
        }

        static EngineResult ProcessMovie(IEnumerable<string> options)
        {
            EngineRequest request = new EngineRequest();
            EngineResult result;

            string inputFileName = Prompt("Movie file");
            FileInfo inputfileInfo = new FileInfo(inputFileName);
            if(!inputfileInfo.Exists)
            {
                result = new EngineResult();
                result.IsSuccess = false;
                result.Message = "Invalid input file";
                return result;
            }
            request.InputFile = inputfileInfo;
            if(inputfileInfo.Extension.Equals("yuv", StringComparison.OrdinalIgnoreCase))
            {
                request.InputFrameRate = PromptInt("Input Framerate");
                request.InputWidth = PromptInt("Input Width");
                request.InputHeight = PromptInt("Input Height");
            }
            request.OutputFrameRate = PromptInt("Output Framerate");
            request.OutputWidth = PromptInt("Output Width");
            request.OutputHeight = PromptInt("Output Height");

            result = Engine.ProcessRequest(request);
            return result;
        }

        static string Prompt(string prompt = null)
        {
            System.Console.Write(prompt);
            System.Console.Write(" > ");
            return System.Console.ReadLine();
        }

        static int PromptInt(string prompt = null, int? minValid = null, int? maxValid = null)
        {
            string value = Prompt(prompt);
            int parsed = 0;
            bool success = false;
            do
            {
                if (int.TryParse(value, out parsed))
                {
                    bool validMin = !minValid.HasValue || minValid.Value <= parsed;
                    bool validMax = !maxValid.HasValue || maxValid.Value >= parsed;

                    if (validMin && validMax)
                    {
                        success = true;
                    }
                }
                else
                {
                    WriteMessagesInColor(ConsoleColor.Red, "Invalid value");
                    value = Prompt(prompt);
                }
            }
            while (success == false);
            
            return parsed;
        }

        static void dummyFill()
        {
            OperationStatus opStatus;
            WriteMessages("Adding colorspace");
            Colorspace colorSpace = new Colorspace
            {
                Name = "yuv420p",
                BitsPerPixel = 12,
                IsMonochrome = false,
                NumChannels = 3
            };
            opStatus = Database.Insert(colorSpace);
            WriteOperationStatus(opStatus);

            WriteMessages("Adding resolutions");
            Resolution resolutionActual = new Resolution
            {
                PixelWidth = 352,
                PixelHeight = 288
            };
            Resolution resolutionTheoretical = new Resolution
            {
                PixelWidth = 176,
                PixelHeight = 144
            };
            opStatus = Database.Insert(resolutionActual, resolutionTheoretical);
            WriteOperationStatus(opStatus);

            WriteMessages("Adding Image");
            Image image = new Image
            {
                Name = "newImage",
                Path = @"c:/data",
                ColorSpaceId = colorSpace.Id,
                ResolutionId = resolutionActual.Id,
                FileSizeInBytes = 256
            };
            opStatus = Database.Insert(image);
            WriteOperationStatus(opStatus);

            WriteMessages("Adding Movie");
            Movie movie = new Movie
            {
                Name = "newMovie",
                Path = @"c:/data",
                ColorSpaceId = colorSpace.Id,
                ResolutionActualId = resolutionActual.Id,
                ResolutionTheoreticalId = resolutionTheoretical.Id,
                FileSizeInBytes = 1024,
                BitRate = 2000,
                FrameRate = 30,
                ParentMovieId = null,
                RootMovieId = null,
                PlayLength = 10.2m,
                SampleFrameId = image.Id
            };
            opStatus = Database.Insert(movie);
            WriteOperationStatus(opStatus);
        }
    }
}
