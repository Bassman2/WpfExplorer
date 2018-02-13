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

        #pragma warning disable 414, 67
        public event EventHandler<RefreshEventArgs> Refresh;

        public string Name
        {
            get
            {
                return this.info.Name;
            }
            set
            {
                if (this.info.Attributes.HasFlag(FileAttributes.Device) || this.info.Attributes.HasFlag(FileAttributes.Directory))
                {
                    DirectoryInfo directoryInfo = this.info as DirectoryInfo;
                    string newPath = Path.Combine(directoryInfo.Parent.FullName, value);
                    directoryInfo.MoveTo(newPath);
                    this.info = new DirectoryInfo(newPath);
                }
                else
                {
                    FileInfo fileInfo = this.info as FileInfo;
                    string newPath = Path.Combine(fileInfo.Directory.FullName, value);
                    fileInfo.MoveTo(newPath);
                    this.info = new FileInfo(newPath);
                }
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

        public ImageSource Icon
        {
            get
            {
                switch (this.Type)
                {
                    case ExplorerItemType.Directory:
                        return DeviceIcons.Folder;
                    case ExplorerItemType.Link:
                        return DeviceIcons.Link;
                    case ExplorerItemType.File:
                        return DeviceIcons.File;
                    default:
                        return null;
                }
            }
        }

        public bool IsDirectory
        {
            get { return this.Type == ExplorerItemType.Directory || this.Type == ExplorerItemType.Link; }
        }

        public bool HasChildren
        {
            get
            {
                try
                {
                    var x = this.info as DirectoryInfo;
                    return x?.EnumerateDirectories().Any() ?? false;
                }
                catch { }
                return false;
            }
        }

        //public IDevice Device
        //{
        //    get
        //    {
        //        return (IDevice)this.device;
        //    }
        //}
        
        public IEnumerable<IExplorerItem> Children
        {
            get
            {
                try
                {
                    return (this.info as DirectoryInfo)?.EnumerateFileSystemInfos().Select(e => new WindowsEntry(this.device, e));
                }
                catch { }
                return null;
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
            if (this.info.Attributes.HasFlag(FileAttributes.Device) || this.info.Attributes.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo directoryInfo = this.info as DirectoryInfo;
                directoryInfo.CreateSubdirectory(folderName);
            }
        }

        #endregion

        #region IEntry

        public bool CanCreateFolder { get { return true; } }
        public bool CanCreateLink { get { return false; } }
        public bool CanDelete { get { return true; } }

        public bool CanRename { get { return true; } }

        public void CreateLink(string linkName, string linkPath)
        {
            throw new NotSupportedException();
        }

        public void Delete()
        {
            throw new NotSupportedException();
        }

        public void Rename(string newName)
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
