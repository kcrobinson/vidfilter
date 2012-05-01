using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Engine
{
    public class Resolution
    {
        public Resolution(int width, int height)
        {
            this.PixelWidth = width;
            this.PixelHeight = height;
        }

        public string Id
        {
            get
            {
                if (PixelWidth <= 0 || PixelHeight <= 0)
                {
                    throw new Exception("Resolution record does not have a valid height or width value. Cannot create record ID.");
                }
                return Resolution.IdFromResolution(this);
            }
        }

        public static string IdFromResolution(Resolution resolution)
        {
            return "Resolution/" + resolution.PixelWidth + "x" + resolution.PixelHeight;
        }
        public int PixelWidth { get; set; }
        public int PixelHeight { get; set; }
    }
}
