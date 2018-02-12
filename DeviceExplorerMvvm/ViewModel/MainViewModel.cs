using DeviceExplorer.Mvvm;
using Devices;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DeviceExplorer.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private IClient selectedClient;
        private ObservableCollection<IDevice> devices;
        private IDevice selectedDevice;
        
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
                    //UpdateDevices();
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
                //UpdateRootFolder();
                
            }
        }


        protected void OnRefresh()
        {
            //UpdateDevices();
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
