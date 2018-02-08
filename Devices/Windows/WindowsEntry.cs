using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Windows
{
    [DebuggerDisplay("{Type} - {FullName}")]
    public class WindowsEntry : IEntry
    {
        private WindowsDevice device;
        private FileSystemInfo info;
        
        /// <summary>
        /// Create root entry
        /// </summary>
        /// <param name="device"></param>
        internal WindowsEntry(WindowsDevice device, FileSystemInfo info)
        {
            this.device = device;
            this.info = info;
        }
        
        public string Name { get { return this.info.Name; } }

        public string FullName { get { return this.info.FullName; } }

        public string Link { get { return null; } }

        public EntryType Type
        {
            get
            {
                if (this.info.Attributes.HasFlag(FileAttributes.Device) || this.info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    return EntryType.Directory;
                }
                else
                {
                    return EntryType.File;
                }
            }
        }

        public long Size { get { return this.info is FileInfo ? (this.info as FileInfo).Length : 0; } }


        public DateTime? Date { get { return this.info.LastWriteTime; } }

        public bool IsDirectory
        {
            get { return this.Type == EntryType.Directory || this.Type == EntryType.Link; }
        }

        public IDevice Device
        {
            get
            {
                return (IDevice)this.device;
            }
        }

        public IEnumerable<IEntry> GetFolders()
        {
            return (this.info as DirectoryInfo)?.EnumerateDirectories().Select(e => new WindowsEntry(this.device, e));
        }

        public IEnumerable<IEntry> GetEntries()
        {
            return (this.info as DirectoryInfo)?.EnumerateFileSystemInfos().Select(e => new WindowsEntry(this.device, e));
        }

        public void CreateFolder(string folderName)
        {
            throw new NotSupportedException();
        }

        public void CreateLink(string linkName, string linkPath)
        {
            throw new NotSupportedException();
        }

        public void Delete()
        {
            throw new NotSupportedException();
        }

        public void Pull(string path, Stream stream)
        {
            throw new NotSupportedException();
        }

        public void Push(Stream stream, string path)
        {
            throw new NotSupportedException();
        }

        public bool CanDelete { get { return false; } }
        public bool CanCreateFolder { get { return false; } }
        public bool CanCreateLink { get { return false; } }
    }
}
