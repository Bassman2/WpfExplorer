using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ExtAdbClient = SharpAdbClient.AdbClient;

namespace Devices.Adb
{
    [DebuggerDisplay("{Name}")]
    public class AdbClient : IClient
    {
        private ExtAdbClient client;
        private DeviceMonitor monitor;

        internal AdbClient()
        {
            AdbServer.Instance.StartServer("adb.exe", false);
            if (!AdbServer.Instance.GetStatus().IsRunning)
            {
                throw new Exception("No ADB server running!");
            }
            
            this.client = (ExtAdbClient)ExtAdbClient.Instance;
            this.monitor = new DeviceMonitor(new AdbSocket(client.EndPoint));

            monitor.DeviceChanged += OnDeviceChanged;
            monitor.DeviceConnected += OnDeviceConnected;
            monitor.DeviceDisconnected += OnDeviceDisconnected;
            monitor.Start();
        }

        #region IClient

        public event EventHandler DevicesChanged;

        public string Name { get { return "Android"; } }

        public string Description { get { return "Android Devices over Android Debug Bridge"; } }

        public ImageSource Icon { get { return DeviceIcons.Android; } }

        public IEnumerable<IDevice> Devices
        {
            get { return client.GetDevices().Select(d => new AdbDevice(this.client, d)); }
        }

        #endregion

        private void OnDeviceChanged(object sender, DeviceDataEventArgs args)
        {
            this.DevicesChanged?.Invoke(this, new EventArgs());
        }

        private void OnDeviceConnected(object sender, DeviceDataEventArgs args)
        {
            this.DevicesChanged?.Invoke(this, new EventArgs());
        }

        private void OnDeviceDisconnected(object sender, DeviceDataEventArgs args)
        {
            this.DevicesChanged?.Invoke(this, new EventArgs());
        }
    }
}
