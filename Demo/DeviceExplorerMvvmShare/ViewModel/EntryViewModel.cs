using DeviceExplorer.Mvvm;
using DeviceExplorer.View;
using Devices;
using ExplorerCtrl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace DeviceExplorer.ViewModel
{
    [DebuggerDisplay("{Type} - {FullName}")]
    public class EntryViewModel : IExplorerItem
    {
        public event EventHandler<RefreshEventArgs> Refresh;

        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand NewFolderCommand { get; private set; }
        public DelegateCommand NewLinkCommand { get; private set; }
        public DelegateCommand PropertiesCommand { get; private set; }


        private IExplorerItem entry;
        private EntryViewModel parent;
       
        /// <summary>
        /// Constructor for TreeView dummy entry to show expand icon
        /// </summary>
        public EntryViewModel() : base()
        {
            this.Name = "Dummy";
            this.parent = null;
            this.entry = null;
        }

        public EntryViewModel(IExplorerItem entry, EntryViewModel parent)
        {
            this.entry = entry;
            this.parent = parent;

            this.DeleteCommand = new DelegateCommand(OnDelete);
            this.NewFolderCommand = new DelegateCommand(OnNewFolder);
            this.NewLinkCommand = new DelegateCommand(OnNewLink);
            this.PropertiesCommand = new DelegateCommand(OnProperties, () => false);
        }

        public string Name { get { return entry.Name; } set { } }

        public string FullName { get { return entry.FullName; } }

        public string Link { get { return entry.Link; } }

        public long Size { get { return entry.Size; } }

        public DateTime? Date { get { return entry.Date; } }

        public ExplorerItemType Type { get { return (ExplorerItemType)Enum.Parse(typeof(ExplorerItemType), entry.Type.ToString()); } }

        public bool IsDirectory { get { return entry.Type == ExplorerItemType.Directory || entry.Type == ExplorerItemType.Link; } }

        public bool HasChildren { get { return entry.Children.Where(e => e.Name != "." && e.Name != "..").Any(); } }

        public IEnumerable<IExplorerItem> Children
        {
            get
            {
                var list = entry.Children.Where(e => e.Name != "." && e.Name != "..").Select(e => new EntryViewModel(e, this));
                this.FilesCount = list.Count();
                return list;
            }
        }

        public int FilesCount { get; private set; }

        public void DoRefresh(bool recursive)
        {
            this.Refresh?.Invoke(this, new RefreshEventArgs(recursive));
        }
        
        public void Push(Stream stream, string path)
        {
            this.entry.Push(stream, path);
        }

        public void Pull(string path, Stream stream)
        {
            this.entry.Pull(path, stream);
        }

        public void CreateFolder(string folderName)
        {
            this.entry.CreateFolder(folderName);
        }

        #region Menu Commands

        public void OnDelete()
        {
            if (MessageBox.Show($"Do your really want do delete {FullName}?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //this.entry.Delete();
                this.parent.DoRefresh(false);
            }
        }

        public void OnNewFolder()
        {
            CreateFolderViewModel vm = new CreateFolderViewModel();
            CreateFolderView v = new CreateFolderView() { DataContext = vm };
            if (v.ShowDialog().Value)
            {
                this.entry.CreateFolder(vm.FolderName);
                DoRefresh(false);
            }
        }

        public void OnNewLink()
        {
            CreateLinkViewModel vm = new CreateLinkViewModel();
            CreateLinkView v = new CreateLinkView() { DataContext = vm };
            if (v.ShowDialog().Value)
            {
                //this.entry.CreateLink(vm.LinkName, vm.LinkPath);
                DoRefresh(false);
            }
        }

        public void OnProperties()
        { }

        #endregion

        //public bool Equals(EntryViewModel other)
        //{
        //    EntryViewModel evm = other as EntryViewModel;
        //    return this.entry?.FullName == evm.entry?.FullName;
        //}

        //public void Refresh()
        //{
        //    throw new NotImplementedException();
        //}

        public bool Equals(IExplorerItem other)
        {
            return this.Type == other.Type && this.FullName == other.FullName;
        }

        public ImageSource Icon
        {
            get
            {
                return this.entry.Icon;
            }
        }
    }
}
