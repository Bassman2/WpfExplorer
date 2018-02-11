using ExplorerCtrl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Devices.Terminal
{
    [DebuggerDisplay("{Type} - {FullName}")]
    public class TerminalEntry : IEntry
    {
        private TerminalDevice device;
        private string path;

        /// <summary>
        /// Create root entry
        /// </summary>
        /// <param name="device"></param>
        internal TerminalEntry(TerminalDevice device)
        {
            this.device = device;
            this.path = "/";
            this.Name = "/";
            this.FullName = "/";
            this.Link = null;
            this.Type = EntryType.Directory;
            this.Size = 0;
            this.Date = new DateTime(1970, 1, 1);
        }

        internal TerminalEntry(TerminalDevice device, string path)
        {
            this.device = device;
            this.path = path;
        }

        #region IExplorerItem

        public string Name { get; private set;}
        
        public string FullName { get; private set; }
        
        public string Link { get; private set; }
        
        public EntryType Type { get; private set; }
        
        public long Size { get; private set; }
        

        public DateTime? Date { get; private set; }        

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
            return this.device.ReadDirectory(this.path);
        }

        public IEnumerable<IEntry> GetEntries()
        {
            return this.device.ReadDirectory(this.path);
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

        public bool CanCreateFolder { get { return true; } }
        public bool CanCreateLink { get { return this.IsDirectory; } }
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
    }
}
