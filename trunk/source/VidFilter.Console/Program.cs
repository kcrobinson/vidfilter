using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VidFilter.Repository;
using VidFilter.Model;

namespace VidFilter.Console
{
    public class Program
    {
        public static readonly IDatabase Database = new RavenDB();

        static void Main(string[] args)
        {
            OperationStatus opStatus;
            try
            {
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
    }
}
