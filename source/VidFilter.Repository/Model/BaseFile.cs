using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace VidFilter.Repository.Model
{
    /// <summary>
    /// Abstract POCO wrapper for System.IO.FileInfo
    /// </summary>
    public abstract class BaseFile
    {
        protected BaseFile() { }
        protected BaseFile(string filePath)
        {
            try
            {
                this.FileInfo = new FileInfo(filePath);
                this.FileName = this.FileInfo.Name;
            }
            catch 
            { 
                // Major error case but there's not a whole lot that can be done at this level.
            }
        }
        protected BaseFile(FileInfo fileInfo)
        {
            this.FileInfo = fileInfo;
            this.FileName = fileInfo.Name;
        }

        /// <summary>
        /// Id is generated from FileInfo using same logic as static method IdFromBaseFile.
        /// </summary>
        public string Id { 
            get
            {
                if (FileInfo == null)
                {
                    return null;
                }
                return BaseFile.BaseFileId(FileInfo);
            } 
        }

        public string FullName
        {
            get
            {
                if (FileInfo == null)
                {
                    return null;
                }
                return FileInfo.FullName;
            }
        }

        /// <summary>
        /// The RavenDB ID for the record of an object that inherits from BaseFile is just the full name of the path of the file, slightly modified.
        /// </summary>
        /// <param name="fileInfo">A FileInfo object</param>
        /// <returns>What the RavenDB Id value of a record made from this FileInfo would be</returns>
        public static string IdFromBaseFile(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return null;
            return BaseFile.BaseFileId(fileInfo);
        }

        private static string BaseFileId(FileInfo fileInfo)
        {
            // Begin with 'BaseFile/' to distinguish the Id as an Id
            // Replace all '\' with '/' due to Raven DB rule of Id strings
            return "BaseFile/" + fileInfo.FullName.Replace('\\', '/');
        }

        [JsonIgnore]
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Name of the file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Full path of the directory which contains the file.
        /// </summary>
        public string DirectoryPath
        {
            get
            {
                if (FileInfo == null)
                {
                    return null;
                }
                return FileInfo.DirectoryName;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// File size
        /// </summary>
        public long FileLength
        {
            get
            {
                if (FileInfo == null)
                {
                    return 0;
                }
                return FileInfo.Length;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// System information about the file's creation time
        /// </summary>
        public DateTime FileCreationTime
        {
            get
            {
                if (FileInfo == null)
                {
                    return default(DateTime);
                }
                return FileInfo.CreationTime;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// System information about the file's last write time
        /// </summary>
        public DateTime FileModificationTime
        {
            get
            {
                if (FileInfo == null)
                {
                    return default(DateTime);
                }
                return FileInfo.LastWriteTime;
            }
        }
    }
}
