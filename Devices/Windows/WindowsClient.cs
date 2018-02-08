using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Devices.Windows
{
    [DebuggerDisplay("{Name}")]
    public class WindowsClient : IClient
    {
        internal WindowsClient()
        { }

#pragma warning disable 414, 67
        public event EventHandler DevicesChanged;

        public string Name { get { return "Windows"; } }

        public string Description { get { return "Windows drives"; } }

        public IEnumerable<IDevice> Devices
        {
            get
            {
                return DriveInfo.GetDrives().Select(d => new WindowsDevice(d));
            }
        }
    }
}
