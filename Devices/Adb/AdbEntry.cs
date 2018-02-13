using Devices.Internal;
using ExplorerCtrl;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Devices.Adb
{
    [DebuggerDisplay("{Type} - {FullName}")]
    public class AdbEntry : IEntry
    {
        private AdbDevice device;
        private FileStatistics fileStatistics;
        private string path;
        private string link;

        internal AdbEntry(AdbDevice device, FileStatistics fileStatistics, string path)
        {
            this.device = device;
            this.fileStatistics = fileStatistics;
            this.path = path == null ? fileStatistics.Path : UnixPath.Combine(path, fileStatistics.Path);

            if (fileStatistics.FileMode.HasFlag(UnixFileMode.SymbolicLink))
            {
                string str = device.ExcecuteCommandResult($"ls -l {this.path}");
                Match match = Regex.Match(str, @".*->(?<link>.+)");
                if (match.Success)
                {
                    this.link = match.Groups["link"].Value.Trim();
                }
            }
        }

        #region IExplorerItem

        #pragma warning disable 414, 67
        public event EventHandler<RefreshEventArgs> Refresh;

        public string Name
        {
            get
            {
                return this.fileStatistics.Path == "/" ? "/" : this.fileStatistics.Path;
            }
            set
            {

            }
        }

        public string FullName
        {
            get
            {
                return this.path;
            }
        }

        public string Link
        {
            get
            {
                return this.link;
            }
        }

        
        public long Size
        {
            get
            {
                return this.fileStatistics.Size;
            }
        }

        public DateTime? Date
        {
            get
            {
                return this.fileStatistics.Time;
            }
        }

        public ExplorerItemType Type
        {
            get
            {
                if (this.fileStatistics.FileMode.HasFlag(UnixFileMode.Directory))
                {
                    return ExplorerItemType.Directory;
                }
                else if (this.fileStatistics.FileMode.HasFlag(UnixFileMode.SymbolicLink))
                {
                    return ExplorerItemType.Link;
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
            get
            {
                return this.fileStatistics.FileMode.HasFlag(UnixFileMode.Directory) || this.fileStatistics.FileMode.HasFlag(UnixFileMode.SymbolicLink);
            }
        }


        public bool HasChildren { get; }


        //public IEnumerable<IEntry> Folders
        //{
        //    using (SyncService service = new SyncService(this.device.deviceData))
        //    {
        //        return service.GetDirectoryListing(path).Where(f => f.Path != "." && f.Path != "..").Select(f => new AdbEntry(this.device, f, path)).Where(e => e.IsDirectory);
        //    }
        //}

        public IEnumerable<IExplorerItem> Children
        {
            get
            {

                using (SyncService service = new SyncService(this.device.deviceData))
                {
                    return service.GetDirectoryListing(path).OrderBy(f => f.Path).Select(f => new AdbEntry(this.device, f, path));
                }
            }
        }
              

        public void Push(Stream stream, string path)
        { }

        public void Pull(string path, Stream stream)
        {
        }

        public void CreateFolder(string folderName)
        {
            string path = UnixPath.Combine(this.FullName, folderName);
            this.device.ExcecuteCommand($"mkdir -p \"{path}\"");
        }

        #endregion

        #region IEntry

        public bool CanDelete { get { return true; } }
        public bool CanCreateFolder { get { return this.IsDirectory; } }
        public bool CanCreateLink { get { return this.IsDirectory;  } }

        public bool CanRename { get { return true; } }

        public void CreateLink(string linkName, string linkPath)
        {
            string path = UnixPath.Combine(linkName, linkPath);
            this.device.ExcecuteCommand($"ln -s {linkPath} {path}");
        }

        public void Delete()
        {
            this.device.ExcecuteCommand($"rm -r {this.FullName}");
        }

        public void Rename(string newName)
        {
            
        }

        #endregion
    }
}
