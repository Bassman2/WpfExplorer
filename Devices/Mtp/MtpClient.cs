using MediaDevices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Devices.Mtp
{
    [DebuggerDisplay("{Name}")]
    public class MtpClient : IClient
    {
        internal MtpClient()
        {
            //MediaDevice.DeviceRemoved += (s, e) => this.DevicesChanged?.Invoke(this, new EventArgs());
        }

        #pragma warning disable 414, 67
        public event EventHandler DevicesChanged;

        public string Name { get { return "MTP"; } }

        public string Description { get { return "Media Transport Protocol"; } }

        public IEnumerable<IDevice> Devices
        {
            get
            {
                return MediaDevice.GetDevices().Select(d => new MtpDevice(d));
            }
        }
    }
}
