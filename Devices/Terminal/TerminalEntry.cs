using ExplorerCtrl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media;

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
            this.Type = ExplorerItemType.Directory;
            this.Size = 0;
            this.Date = new DateTime(1970, 1, 1);
        }

        internal TerminalEntry(TerminalDevice device, string path)
        {
            this.device = device;
            this.path = path;
        }

        #region IExplorerItem

        public event EventHandler<RefreshEventArgs> Refresh;

        public string Name { get; set;}
        
        public string FullName { get; private set; }
        
        public string Link { get; private set; }
        
        public long Size { get; private set; }
        

        public DateTime? Date { get; private set; }

        public ExplorerItemType Type { get; private set; }

        public ImageSource Icon { get; }

        public bool IsDirectory
        {
            get { return this.Type == ExplorerItemType.Directory || this.Type == ExplorerItemType.Link; }
        }

        //public IDevice Device
        //{
        //    get
        //    {
        //        return (IDevice)this.device;
        //    }
        //}

        //public IEnumerable<IEntry> GetFolders()
        //{
        //    return this.device.ReadDirectory(this.path);
        //}

        public bool HasChildren
        {
            get
            {
                return this.Children.Any();
            }
        }

        public IEnumerable<IExplorerItem> Children
        {
            get
            {
                return this.device.ReadDirectory(this.path);
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
