using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VidFilter.Model
{
    public abstract class BaseFile : IMergeable
    {
        public BaseFile()
        {
            CreationDateTime = ModifyDateTime = DateTime.Now;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime ModifyDateTime { get; set; }
        public decimal FileSizeInBytes { get; set; }

        public virtual void Merge(object newObject, bool IncludeId=false)
        {
            BaseFile newFile = newObject as BaseFile;
            if (newFile == null)
            {
                throw new NotImplementedException("Cannot update File record with non-File object");
            }
            if (IncludeId)
            {
                this.Id = newFile.Id;
            }
            this.Name = newFile.Name;
            this.Path = newFile.Path;
            this.CreationDateTime = newFile.CreationDateTime;
            this.ModifyDateTime = newFile.ModifyDateTime;
            this.FileSizeInBytes = newFile.FileSizeInBytes;
        }

        public override bool Equals(object obj)
        {
            BaseFile file = obj as BaseFile;
            if (file == null) return false;

            return this.Name == file.Name && this.Path == file.Path;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
