using ExplorerCtrl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media;

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

        #region IExplorerItem

        public event EventHandler<RefreshEventArgs> Refresh;

        public string Name
        {
            get
            {
                return this.info.Name;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public string FullName { get { return this.info.FullName; } }

        public string Link { get { return null; } }

        public long Size { get { return this.info is FileInfo ? (this.info as FileInfo).Length : 0; } }

        public DateTime? Date { get { return this.info.LastWriteTime; } }


        public ExplorerItemType Type
        {
            get
            {
                if (this.info.Attributes.HasFlag(FileAttributes.Device) || this.info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    return ExplorerItemType.Directory;
                }
                else
                {
                    return ExplorerItemType.File;
                }
            }
        }

        public ImageSource Icon { get; }
        
        public bool IsDirectory
        {
            get { return this.Type == ExplorerItemType.Directory || this.Type == ExplorerItemType.Link; }
        }

        public bool HasChildren { get; }

        public IDevice Device
        {
            get
            {
                return (IDevice)this.device;
            }
        }
        
        public IEnumerable<IExplorerItem> Children
        {
            get
            {
                return (this.info as DirectoryInfo)?.EnumerateFileSystemInfos().Select(e => new WindowsEntry(this.device, e));
            }
        }
        
        public void Pull(string path, Stream stream)
        {
            throw new NotSupportedException();
        }

        public void Push(Stream stream, string path)
        {
            throw new NotSupportedException();
        }

        public void CreateFolder(string folderName)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region IEntry

        public bool CanCreateFolder { get { return false; } }
        public bool CanCreateLink { get { return false; } }
        public bool CanDelete { get { return false; } }
        
        public void CreateLink(string linkName, string linkPath)
        {
            throw new NotSupportedException();
        }

        public void Delete()
        {
            throw new NotSupportedException();
        }

        #endregion

        //#region IEquatable

        //public bool Equals(IExplorerItem other)
        //{
        //    throw new NotImplementedException();
        //}

        //#endregion
    }
}
