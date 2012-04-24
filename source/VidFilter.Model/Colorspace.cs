using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public class Colorspace : IMergeable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsMonochrome { get; set; }
        public int NumChannels { get; set; }
        public int BitsPerPixel { get; set; }

        public void Merge(object newObject, bool IncludeId = false)
        {
            Colorspace newColorspace = newObject as Colorspace;
            if (newColorspace == null)
            {
                throw new NotSupportedException("Cannot update Colorspace record with non-Colorspace object");
            }
            if (IncludeId)
            {
                this.Id = newColorspace.Id;
            }
            this.Name = newColorspace.Name;
            this.IsMonochrome = newColorspace.IsMonochrome;
            this.NumChannels = newColorspace.NumChannels;
            this.BitsPerPixel = newColorspace.BitsPerPixel;
        }

        public override bool Equals(object obj)
        {
            Colorspace colorspace = obj as Colorspace;
            if (colorspace == null)
            {
                return false;
            }
            return colorspace.Name == this.Name;
        }
    }
}
