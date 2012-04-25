using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public class Image : BaseFile, IMergeable
    {
        public string ResolutionId { get; set; }
        public string ColorSpaceId { get; set; }

        public override void Merge(object newObject, bool IncludeId=false)
        {
            Image newImage = newObject as Image;
            if (newImage == null)
            {
                throw new NotSupportedException("Cannot update Image record with non-Image object");
            }
            base.Merge(newImage, IncludeId);
            this.ResolutionId = newImage.ResolutionId;
            this.ColorSpaceId = newImage.ColorSpaceId;
        }
    }
}
