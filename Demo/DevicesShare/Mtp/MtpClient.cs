using MediaDevices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media;

namespace Devices.Mtp
{
    [DebuggerDisplay("{Name}")]
    public class MtpClient : IClient
    {
        internal MtpClient()
        {
            //MediaDevice.DeviceRemoved += (s, e) => this.DevicesChanged?.Invoke(this, new EventArgs());
        }

        #region IClient

        #pragma warning disable 414, 67
        public event EventHandler DevicesChanged;

        public string Name { get { return "Media Device"; } }

        public string Description { get { return "Media Device over Media Transport Protocol"; } }

        public ImageSource Icon { get { return DeviceIcons.Device; } }

        public IEnumerable<IDevice> Devices
        {
            get
            {
                return MediaDevice.GetDevices().Select(d => new MtpDevice(d));
            }
        }

        #endregion
    }
}
