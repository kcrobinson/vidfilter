using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VidFilter.Model
{
    public abstract class BaseFile
    {
        protected BaseFile() { }
        protected BaseFile(FileInfo fileInfo)
        {
            this._FileInfo = fileInfo;
        }

        public string Id { 
            get
            {
                if (_FileInfo == null)
                {
                    throw new Exception("BaseFile record does not have a FileInfo value. Cannot create record ID.");
                }
                return BaseFile.BaseFileId(_FileInfo);
            } 
        }

        public static string IdFromBaseFile(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;
            return BaseFile.BaseFileId(fileInfo);
        }

        private static string BaseFileId(FileInfo fileInfo)
        {
            return "BaseFile/" + fileInfo.FullName.Replace('\\', '/');
        }

        protected FileInfo _FileInfo;
        public FileInfo GetFileInfo() { return _FileInfo; }

        public string FileName 
        { 
            get
            {
                if (_FileInfo == null) return null;
                return _FileInfo.Name;
            }
        }

        public string DirectoryPath
        {
            get
            {
                if (_FileInfo == null)
                {
                    return null;
                }
                return _FileInfo.DirectoryName;
            }
        }

        public long SizeInBytes
        {
            get
            {
                if (_FileInfo == null)
                {
                    return 0;
                }
                return _FileInfo.Length;
            }
        }

        public DateTime TimeCreation
        {
            get
            {
                if (_FileInfo == null)
                {
                    return default(DateTime);
                }
                return _FileInfo.CreationTime;
            }
        }

        public DateTime TimeLastModified
        {
            get
            {
                if (_FileInfo == null)
                {
                    return default(DateTime);
                }
                return _FileInfo.LastWriteTime;
            }
        }

        public virtual void MergeFrom(BaseFile newObject)
        {
            this._FileInfo = newObject.GetFileInfo();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
