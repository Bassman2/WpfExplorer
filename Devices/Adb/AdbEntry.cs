using Devices.Internal;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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

        public string Name
        {
            get
            {
                return this.fileStatistics.Path == "/" ? "/" : this.fileStatistics.Path;
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

        public EntryType Type
        {
            get
            {
                if (this.fileStatistics.FileMode.HasFlag(UnixFileMode.Directory))
                {
                    return EntryType.Directory;
                }
                else if (this.fileStatistics.FileMode.HasFlag(UnixFileMode.SymbolicLink))
                {
                    return EntryType.Link;
                }
                else
                {
                    return EntryType.File;
                }
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

        public bool IsDirectory
        {
            get
            {
                return this.fileStatistics.FileMode.HasFlag(UnixFileMode.Directory) || this.fileStatistics.FileMode.HasFlag(UnixFileMode.SymbolicLink);
            }
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
            using (SyncService service = new SyncService(this.device.deviceData))
            {
                return service.GetDirectoryListing(path).Where(f => f.Path != "." && f.Path != "..").Select(f => new AdbEntry(this.device, f, path)).Where(e => e.IsDirectory);
            }
        }

        public IEnumerable<IEntry> GetEntries()
        {

            using (SyncService service = new SyncService(this.device.deviceData))
            {
                return service.GetDirectoryListing(path).OrderBy(f => f.Path).Select(f => new AdbEntry(this.device, f, path));
            }
        }

        //bool FolderExist(string path)
        //{ }

        public void CreateFolder(string folderName)
        {
            string path = UnixPath.Combine(this.FullName, folderName);
            this.device.ExcecuteCommand($"mkdir -p \"{path}\"");
        }

        public void CreateLink(string folderName, string linkpath)
        {
            string path = UnixPath.Combine(this.FullName, folderName);
            this.device.ExcecuteCommand($"ln -s {linkpath} {path}");
        }
        
        public void Delete()
        {
            this.device.ExcecuteCommand($"rm -r {this.FullName}");
        }

        public void Pull(string path, Stream stream)
        {
        }

        public void Push(Stream stream, string path)
        { }

        public bool CanDelete { get { return true; } }
        public bool CanCreateFolder { get { return this.IsDirectory; } }
        public bool CanCreateLink { get { return this.IsDirectory; } }
    }
}
