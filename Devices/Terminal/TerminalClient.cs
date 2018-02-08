using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;

namespace Devices.Terminal
{
    [DebuggerDisplay("{Name}")]
    public class TerminalClient : IClient
    {
        internal TerminalClient()
        {
        }

        #pragma warning disable 414, 67
        public event EventHandler DevicesChanged;

        public string Name { get { return "Terminal"; } }

        public string Description { get { return "Serial Terminal over COM port"; } }

        public IEnumerable<IDevice> Devices
        {
            get
            {
                return SerialPort.GetPortNames().Select(d => new TerminalDevice(d));
            }
        }
    }
}
