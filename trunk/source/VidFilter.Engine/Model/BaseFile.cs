using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace VidFilter.Engine
{
    /// <summary>
    /// Abstract POCO wrapper for System.IO.FileInfo
    /// </summary>
    public abstract class BaseFile
    {
        protected BaseFile() { }
        protected BaseFile(FileInfo fileInfo)
        {
            this._FileInfo = fileInfo;
        }

        /// <summary>
        /// Id is generated from FileInfo using same logic as static method IdFromBaseFile.
        /// </summary>
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

        public string FullName
        {
            get
            {
                if (_FileInfo == null)
                {
                    return null;
                }
                return _FileInfo.FullName;
            }
            set
            {
                _FileInfo = new FileInfo(value);
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
        protected FileInfo _FileInfo { get; set; }
        public FileInfo GetFileInfo() { return _FileInfo; }

        [JsonIgnore]
        /// <summary>
        /// Name of the file
        /// </summary>
        public string FileName 
        { 
            get
            {
                if (_FileInfo == null) return null;
                return _FileInfo.Name;
            }
        }

        [JsonIgnore]
        /// <summary>
        /// Full path of the directory which contains the file. Null
        /// </summary>
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

        [JsonIgnore]
        /// <summary>
        /// File size
        /// </summary>
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

        [JsonIgnore]
        /// <summary>
        /// System information about the file's creation time
        /// </summary>
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

        [JsonIgnore]
        /// <summary>
        /// System information about the file's last write time
        /// </summary>
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
    }
}
