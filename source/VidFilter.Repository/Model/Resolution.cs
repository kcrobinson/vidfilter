using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public class Resolution : IMergeable
    {
        public string Id
        {
            get
            {
                if (PixelWidth <= 0 || PixelHeight <= 0)
                {
                    throw new Exception("Resolution record does not have a valid height or width value. Cannot create record ID.");
                }
                return "Resolution/" + PixelWidth + "x" + PixelHeight;
            }
        }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }

        public void MergeFrom(IMergeable newObject)
        {
            Resolution newResolution = newObject as Resolution;
            if (newResolution == null)
            {
                throw new NotSupportedException("Cannot update Resolution record with non-Resolution object");
            }
            this.PixelHeight = newResolution.PixelHeight;
            this.PixelWidth = newResolution.PixelWidth;
        }

        public bool IsEqual(IMergeable record)
        {
            Resolution res = record as Resolution;
            if (res == null)
            {
                return false;
            }
            return res.PixelHeight == this.PixelHeight && res.PixelWidth == this.PixelWidth;
        }
    }
}
