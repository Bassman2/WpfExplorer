using DeviceExplorer.Mvvm;
using Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DeviceExplorer.ViewModel
{
    public class DeviceInfoViewModel : DialogViewModel
    {
        private DispatcherTimer dispatcherTimer;
        private IDevice device;

        private const long Kilo = 1024;
        private const long Mega = 1024 * 1024;

        public DeviceInfoViewModel(IDevice device)
        {
            this.DeviceInfo = device.GetDeviceInfo();
            this.device = device;
            OnTick(this, null);
            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += new EventHandler(OnTick);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 2);
            this.dispatcherTimer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            this.DeviceMemory = this.device.GetMemoryInfo();
            NotifyAllPropertiesChanged();
        }

        protected override void OnOK()
        {
            this.dispatcherTimer.Stop();
            base.OnCancel();
        }

        public Dictionary<string, string> DeviceInfo { get; private set; }

        public Dictionary<string, DeviceMemoryInfo> DeviceMemory { get; private set; }
    }
}
