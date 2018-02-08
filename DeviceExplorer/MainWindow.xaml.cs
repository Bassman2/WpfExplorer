using Devices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DeviceExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DeviceType deviceType = DeviceType.Windows;
        private List<IDevice> devices;
        private IDevice selectedDevice;
        //private List<EntryViewModel> rootfolders;
        //private EntryViewModel selectedFolder;
        private DispatcherTimer dispatcherTimer;
        private long freeMemory = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            //try
            //{
            //    ProcessStartInfo psi = new ProcessStartInfo();
            //    psi.FileName = "adb.exe";
            //    psi.Arguments = $"-s {this.SelectedDevice.Id} shell";
            //    Process.Start(psi);
            //}
            //catch (Exception ex)
            //{
            //    Trace.TraceError(ex.ToString());
            //}

            base.OnInitialized(e);
        }

        private void OnWindows(object sender, RoutedEventArgs e)
        {
            this.deviceType = DeviceType.Windows;

            this.devices.Clear();
            var devs = DeviceFactory.GetClient(this.deviceType).Devices;

        }

        private void OnAndroid(object sender, RoutedEventArgs e)
        {
            this.deviceType = DeviceType.ADB;
        }

        private void OnMedia(object sender, RoutedEventArgs e)
        {
            this.deviceType = DeviceType.MTP;
        }

        private void OnTerminal(object sender, RoutedEventArgs e)
        {
            this.deviceType = DeviceType.Terminal;
        }

        private void RefreshDevice(DeviceType deviceType)
        {
            this.deviceType = deviceType;

            this.devicesComboBox.Items.Clear();
            var devs = DeviceFactory.GetClient(this.deviceType).Devices;
            devs.ToList().ForEach(d => this.devicesComboBox.Items.Add(d));
            this.devicesComboBox.SelectedIndex = 0;

            this.selectedDevice = (IDevice)this.devicesComboBox.SelectedItem;


        }

        private void OnRefresh(object sender, RoutedEventArgs e)
        {

        }
    }
}
