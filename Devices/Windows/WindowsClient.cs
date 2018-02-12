using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media;

namespace Devices.Windows
{
    [DebuggerDisplay("{Name}")]
    public class WindowsClient : IClient
    {
        internal WindowsClient()
        { }

        #region IClient

        #pragma warning disable 414, 67
        public event EventHandler DevicesChanged;

        public string Name { get { return "Windows"; } }

        public string Description { get { return "Windows drives"; } }

        public ImageSource Icon { get { return DeviceIcons.Windows; } }

        public IEnumerable<IDevice> Devices
        {
            get
            {
                return DriveInfo.GetDrives().Select(d => new WindowsDevice(d));
            }
        }

        #endregion
    }
}
