using DeviceExplorer.Mvvm;
using Devices;
using ExplorerCtrl;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DeviceExplorer.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IClient selectedClient;
        private ObservableCollection<IDevice> devices;
        private IDevice selectedDevice;
        private IEnumerable<IExplorerItem> rootFolders;
        private IExplorerItem selectedFolder;
        private string selectedPath;

        public MainViewModel()
        {
        
        }

        public IEnumerable<IClient> Clients
        {
            get { return DeviceFactory.Clients; }
        }

        public IClient SelectedClient
        {
            get
            {
                return this.selectedClient;
            }
            set
            {
                if (this.selectedClient != value)
                {
                    this.selectedClient = value;
                    NotifyPropertyChanged(nameof(SelectedClient));
                    UpdateDevices();
                }
            }
        }

        public ObservableCollection<IDevice> Devices
        {
            get
            {
                return this.devices;
            }
            set
            {
                this.devices = value;
                NotifyPropertyChanged(nameof(SelectedClient));
            }
        }

        public IDevice SelectedDevice
        {
            get
            {
                return this.selectedDevice;
            }
            set
            {
                this.selectedDevice = value;
                NotifyPropertyChanged(nameof(SelectedDevice));
                UpdateExplorer();
                
            }
        }

        public IEnumerable<IExplorerItem> RootFolders
        {
            get
            {
                return this.rootFolders;
            }
            set
            {
                this.rootFolders = value;
                NotifyPropertyChanged(nameof(RootFolders));
            }
        }

        public IExplorerItem SelectedFolder
        {
            get
            {
                return this.selectedFolder;
            }
            set
            {
                this.selectedFolder = value;
                NotifyPropertyChanged(nameof(SelectedFolder));
            }
        }

        public string SelectedPath
        {
            get
            {
                return this.selectedPath;
            }
            set
            {
                this.selectedPath = value;
                NotifyPropertyChanged(nameof(SelectedPath));
            }
        }

        protected void OnRefresh()
        {
            UpdateDevices();
        }


        private void UpdateDevices()
        {
            this.Devices = new ObservableCollection<IDevice>(this.SelectedClient.Devices);
            this.SelectedDevice = this.Devices.FirstOrDefault();
        }

        private void UpdateExplorer()
        {
            var list  = new List<IExplorerItem>();
            list.Add(this.SelectedDevice.Root);
            this.RootFolders = list;
        }


        #region Properties


        //public ObservableCollection<EntryViewModel> RootFolders
        //{
        //    get { return this.rootfolders; }
        //}

        //public EntryViewModel SelectedFolder
        //{
        //    get { return this.selectedFolder; }
        //    set { this.selectedFolder = value; NotifyPropertyChanged(nameof(SelectedFolder)); }
        //}



        #endregion

        #region Methods

        //private void UpdateDevices()
        //{

        //    if (change)
        //    {
        //        this.devices.Clear();
        //    }

        //    var devs = DeviceFactory.GetClient(this.deviceType).Devices;
        //    this.devices.Update(devs);

        //    if (this.SelectedDevice == null || !this.devices.Contains(this.SelectedDevice))
        //    {
        //        this.SelectedDevice = this.Devices.FirstOrDefault();
        //    }

        //}

        //private void UpdateRootFolder()
        //{
        //    try
        //    {
        //        this.RootFolders.Clear();
        //        if (this.SelectedDevice != null)
        //        {
        //            this.RootFolders.Add(new EntryViewModel(this.SelectedDevice.Root, null));
        //        }
        //        this.SelectedFolder = this.RootFolders.FirstOrDefault();
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //    }
        //}

        #endregion



    }
}
