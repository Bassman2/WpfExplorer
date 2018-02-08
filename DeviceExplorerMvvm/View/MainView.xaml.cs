using DeviceExplorer.Mvvm;
using DeviceExplorer.ViewModel;
using Devices;
using System;
using System.Collections.Generic;
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

namespace DeviceExplorer.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : AppView
    {
        private UsbMonitor usbMonitor;

        public MainView()
        {
            InitializeComponent();
            this.usbMonitor = new UsbMonitor(this);
            this.usbMonitor.UsbUpdate += OnUsbUpdate;
            this.usbMonitor.UsbChanged += OnUsbChanged;
        }

        private void OnUsbChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine($"OnUsbChanged");
        }

        private void OnUsbUpdate(object sender, UsbEventArgs e)
        {
           // System.Diagnostics.Trace.WriteLine($"OnUsbUpdate {e.Action} {e.Class} {e.ClassGuid} {e.Name}");
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }

        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainViewModel vm = this.DataContext as MainViewModel;
            vm.SelectedFolder = e.NewValue as EntryViewModel;
        }
    }
}
