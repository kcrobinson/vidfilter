using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public class Resolution : IMergeable
    {
        public string Id { get; set; }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }

        public void Merge(object newObject, bool IncludeId = false)
        {
            Resolution newResolution = newObject as Resolution;
            if (newResolution == null)
            {
                throw new NotSupportedException("Cannot update Resolution record with non-Resolution object");
            }
            if (IncludeId)
            {
                this.Id = newResolution.Id;
            }
            this.PixelHeight = newResolution.PixelHeight;
            this.PixelWidth = newResolution.PixelWidth;
        }
    }
}
