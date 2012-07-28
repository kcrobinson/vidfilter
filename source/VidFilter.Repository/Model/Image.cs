using System.IO;
using Newtonsoft.Json;

namespace VidFilter.Repository.Model
{
    public class Image : BaseFile
    {
        [JsonConstructor]
        public Image(string FullName) : base(FullName) { }

        public Image(FileInfo fileInfo) : base(fileInfo) { }

        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public string ColorSpaceId { get; set; }
    }
}
