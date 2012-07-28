using System.IO;
using Newtonsoft.Json;

namespace VidFilter.Repository.Model
{
    public class Movie : BaseFile
    {
        [JsonConstructor]
        public Movie(string FullName) : base(FullName) { }

        public Movie(FileInfo fileInfo) : base(fileInfo) { }

        public string ParentMovieId { get; set; }
        public int BitRate { get; set; }
        public int FrameRate { get; set; }
        public decimal PlayLength { get; set; }
        public string SampleFrameId { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public string ColorspaceName { get; set; }
    }
}
