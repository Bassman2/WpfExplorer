using Devices;
using ExplorerCtrl;
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
        private IClient selectedClient;
        private IDevice selectedDevice;
        
        public MainWindow()
        {
            InitializeComponent();

            // fill client ComboBox
            foreach (var client in DeviceFactory.Clients)
            {
                this.clientsComboBox.Items.Add(client);
            }
            this.clientsComboBox.SelectedIndex = 0;
        }

        private void OnClientSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedClient = e.AddedItems.Cast<IClient>().FirstOrDefault();
            RefreshDevice();
        }

        private void OnRefresh(object sender, RoutedEventArgs e)
        {
            RefreshDevice();
        }

        protected override void OnUsbChanged()
        {
            RefreshDevice();
        }
        
        private void OnDeviceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selectedDevice = e.AddedItems.Cast<IDevice>().FirstOrDefault();

            RefreshExplorer();
        }

        private void RefreshDevice()
        {
            this.devicesComboBox.Items.Clear();
            foreach (var device in this.selectedClient.Devices)
            {
                this.devicesComboBox.Items.Add(device);
            }
            this.devicesComboBox.SelectedIndex = 0;
        }

        private void RefreshExplorer()
        {
            var root = new List<IExplorerItem>();
            if (this.selectedDevice != null)
            {
                root.Add(this.selectedDevice.Root);
            }
            this.explorer.ItemsSource = root;
        }
    }
}
