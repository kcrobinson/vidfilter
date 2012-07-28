using System;
using System.Collections.Generic;

namespace VidFilter.Repository.Model
{
    public class Colorspace
    {
        public string Name { get; set; }
        public string CodeName { get; set; }
        public int NumChannels { get; set; }
        public int BitsPerPixel { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public IEnumerable<string> Validate()
        {
            if (String.IsNullOrWhiteSpace(this.Name))
            {
                yield return "Invalid colorspace name";
            }
            else
            {
                if (String.IsNullOrWhiteSpace(this.CodeName))
                {
                    yield return String.Format("Invalid colorspace codename for {0}", this.Name);
                }
                if (this.NumChannels <= 0)
                {
                    yield return String.Format("Invalid colorspace NumChannels value for {0}", this.Name);
                }
                if (this.BitsPerPixel <= 0)
                {
                    yield return String.Format("Invalid colorspace BitsPerPixel value for {0}", this.Name);
                }
            }
        }
    }
}
