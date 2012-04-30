﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public class Colorspace
    {
        public string Id 
        { 
            get 
            {
                if (Name == null)
                {
                    throw new Exception("Colorspace record does not have a Name value. Cannot create record ID.");
                }
                return "Colorspace/" + Name; 
            } 
        }
        public string Name { get; set; }
        public bool IsMonochrome { get; set; }
        public int NumChannels { get; set; }
        public int BitsPerPixel { get; set; }
    }
}