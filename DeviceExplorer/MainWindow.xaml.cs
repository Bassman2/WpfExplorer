using Devices;
using ExplorerCtrl;
using ExplorerCtrl.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        private void OnDelete(object sender, RoutedEventArgs e)
        {
            ExplorerItem expItem = ((MenuItem)sender).DataContext as ExplorerItem;
            IExplorerItem item = expItem?.Content;
            IEntry entry = item as IEntry;
            if (entry != null)
            {
                entry.Delete();
            }
        }

        private void OnCreateFolder(object sender, RoutedEventArgs e)
        {
            ExplorerItem expItem = ((MenuItem)sender).DataContext as ExplorerItem;
            IExplorerItem item = expItem?.Content;
            IEntry entry = item as IEntry;
            if (entry != null)
            {
                entry.CreateFolder("NewFolder");
            }
        }

        private void OnCreateLink(object sender, RoutedEventArgs e)
        {
            ExplorerItem expItem = ((MenuItem)sender).DataContext as ExplorerItem;
            IExplorerItem item = expItem?.Content;
            IEntry entry = item as IEntry;
            if (entry != null)
            {
                entry.CreateLink("NewLink", "xxx");
            }
        }
    }
}
