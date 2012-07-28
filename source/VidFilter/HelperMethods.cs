using System.Text;
using VidFilter.Engine;
using VidFilter.Repository.Model;

namespace VidFilter
{
    class HelperMethods
    {
        public static string InsertMovieToDatabase(Movie movie)
        {
            StringBuilder sb = new StringBuilder();

            ImageRequest imageRequest = new ImageRequest()
            {
                MoviePath = movie.FullName,
                MovieLength = movie.PlayLength
            };
            ImageResult imageResult = App.Engine.CreateImage(imageRequest);
            if (!imageResult.IsSuccess)
            {
                sb.Append("Error creating image: " + imageResult.ErrorMessage);
            }
            Image image = new Image(imageResult.OutFile)
            {
                ColorSpaceId = movie.ColorspaceName,
                ResolutionHeight = movie.ResolutionHeight,
                ResolutionWidth = movie.ResolutionWidth
            };
            OperationStatus status = App.Database.InsertImage(image);
            if (!status.IsSuccess)
            {
                sb.Append("Error inserting image to database: " + status.Message);
                return sb.ToString();
            }
            movie.SampleFrameId = image.Id;

            status = App.Database.InsertMovie(movie);
            if (status.IsSuccess)
            {
                sb.Append("Success adding movie");
            }
            else
            {
                sb.Append(status.Message);
                if (status.Exception != null)
                {
                    sb.Append("\r\n" + status.Exception.Message);
                }
            }
            return sb.ToString();
        }
    }
}
