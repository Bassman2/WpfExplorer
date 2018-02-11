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
using WpfUsbMonitor;

namespace DeviceExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UsbMonitorWindow
    {
        private DeviceType deviceType = DeviceType.Windows;
        private List<IDevice> devices;
        private IDevice selectedDevice;
        
        public MainWindow()
        {
            InitializeComponent();
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
