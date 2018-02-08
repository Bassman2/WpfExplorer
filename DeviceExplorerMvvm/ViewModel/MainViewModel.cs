using DeviceExplorer.Misc;
using DeviceExplorer.Mvvm;
using DeviceExplorer.View;
using Devices;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

namespace DeviceExplorer.ViewModel
{
    public class MainViewModel : AppViewModel
    {
        private DeviceType deviceType = DeviceType.Windows;
        private ObservableCollection<IDevice> devices;
        private IDevice selectedDevice;
        private ObservableCollection<EntryViewModel> rootfolders;
        private EntryViewModel selectedFolder;
        private DispatcherTimer dispatcherTimer;
        private long freeMemory = 0; 

        public DelegateCommand ShellCommand { get; private set; }
        public DelegateCommand DeviceInfoCommand { get; private set; }
        public DelegateCommand CheckCompanionCommand { get; private set; }

        public MainViewModel()
        {
            this.ShellCommand = new DelegateCommand(OnShell, OnCanShell);
            this.DeviceInfoCommand = new DelegateCommand(OnDeviceInfo, OnCanDeviceInfo);
            this.CheckCompanionCommand = new DelegateCommand(OnCheckCompanion, () => this.SelectedDevice != null);

            this.devices = new ObservableCollection<IDevice>();
            this.rootfolders = new ObservableCollection<EntryViewModel>();

            this.MemoryValues = new ObservableCollection<double>();
            this.NativeValues = new ObservableCollection<double>();
            this.DalvikValues = new ObservableCollection<double>();            

            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += new EventHandler(OnTick);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
        }
        
        public override void OnStartup()
        {
            RefreshDevices();
            base.OnStartup();
        }

        protected override void OnRefresh()
        {
            IDevice device = this.SelectedDevice;
            RefreshDevices();
            if (device == this.SelectedDevice)
            {
                this.RootFolders?.ToList().ForEach(i => i.DoRefresh(true));
            }
        }

        protected bool OnCanShell()
        {
            return this.DeviceType == DeviceType.ADB && this.SelectedDevice != null;
        }

        protected void OnShell()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "adb.exe";
                psi.Arguments = $"-s {this.SelectedDevice.Id} shell";
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
        
        protected bool OnCanDeviceInfo()
        {
            return this.SelectedDevice != null && ( this.SelectedDevice.HasDeviceInfo || this.SelectedDevice.HasMemoryInfo); 
        }

        protected void OnDeviceInfo()
        {
            DeviceInfoView dlg = new DeviceInfoView();
            dlg.DataContext = new DeviceInfoViewModel(this.SelectedDevice);
            dlg.ShowDialog();  
        }

        private void OnTick(object sender, EventArgs e)
        {
            IDevice device = this.SelectedDevice;
            if (device != null)
            {
                //DeviceMemoryInfo deviceMemory = device.GetDeviceMemory();
                //if (deviceMemory != null)
                //{
                //    this.MemoryValues.Add(deviceMemory.System.Percentage);
                //    this.NativeValues.Add(deviceMemory.AppNative?.Percentage ?? 0.0);
                //    this.DalvikValues.Add(deviceMemory.AppDalvik?.Percentage ?? 0.0);

                //    this.FreeMemory = deviceMemory.System.Free;
                //}
            }
        }
        
        protected void OnCheckCompanion()
        {
            CheckCompanionView dlg = new CheckCompanionView();
            dlg.DataContext = new CheckCompanionViewModel(this.SelectedDevice);
            dlg.ShowDialog();
        }

        #region Properties

        public DeviceType DeviceType
        {
            get { return this.deviceType; }
            set { this.deviceType = value; NotifyPropertyChanged(nameof(DeviceType)); RefreshDevices(true); }
        }

        public ObservableCollection<IDevice> Devices
        {
            get { return this.devices; }
        }

        public IDevice SelectedDevice
        {
            get { return this.selectedDevice; }
            set
            {
                this.selectedDevice = value;
                NotifyPropertyChanged(nameof(SelectedDevice));
                UpdateRootFolder();
                this.MemoryValues.Clear();
                this.NativeValues.Clear();
                this.DalvikValues.Clear();
            }
        }

        public ObservableCollection<EntryViewModel> RootFolders
        {
            get { return this.rootfolders; }
        }

        public EntryViewModel SelectedFolder
        {
            get { return this.selectedFolder; }
            set { this.selectedFolder = value; NotifyPropertyChanged(nameof(SelectedFolder)); }
        }
        
        public ObservableCollection<double> MemoryValues { get; private set; }
        public ObservableCollection<double> NativeValues { get; private set; }
        public ObservableCollection<double> DalvikValues { get; private set; }

        public long FreeMemory
        {
            get { return this.freeMemory; }
            set { this.freeMemory = value; NotifyPropertyChanged(nameof(FreeMemory)); }
        }
        
        #endregion

        #region Methods

        private void RefreshDevices(bool change = false)
        {
            
            if (change)
            {
                this.devices.Clear();
            }

            var devs = DeviceFactory.GetClient(this.deviceType).Devices;
            this.devices.Update(devs);

            if (this.SelectedDevice == null || !this.devices.Contains(this.SelectedDevice))
            {
                this.SelectedDevice = this.Devices.FirstOrDefault();
            }
           
        }

        private void UpdateRootFolder()
        {
            try
            {
                this.RootFolders.Clear();
                if (this.SelectedDevice != null)
                {
                    this.RootFolders.Add(new EntryViewModel(this.SelectedDevice.Root, null));
                }
                this.SelectedFolder = this.RootFolders.FirstOrDefault();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        #endregion


        #region Smart Auto 

        private bool isChartsVisible = false;

        public bool IsChartsVisible
        {
            get { return this.isChartsVisible; }
            set
            {
                this.isChartsVisible = value;
                this.dispatcherTimer.IsEnabled = value;
                if (value)
                {
                    this.MemoryValues.Clear();
                    this.NativeValues.Clear();
                    this.DalvikValues.Clear();
                }
                NotifyPropertyChanged(nameof(IsChartsVisible));
            }
        }

        #endregion
    }
}
