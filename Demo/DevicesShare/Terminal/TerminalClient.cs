using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Windows.Media;

namespace Devices.Terminal
{
    [DebuggerDisplay("{Name}")]
    public class TerminalClient : IClient
    {
        internal TerminalClient()
        {
        }

        #region IClient

        #pragma warning disable 414, 67
        public event EventHandler DevicesChanged;

        public string Name { get { return "Terminal"; } }

        public string Description { get { return "Serial Terminal over COM port"; } }

        public ImageSource Icon { get { return DeviceIcons.Terminal; } }

        public IEnumerable<IDevice> Devices
        {
            get
            {
                return SerialPort.GetPortNames().Select(d => new TerminalDevice(d));
            }
        }

        #endregion
    }
}
