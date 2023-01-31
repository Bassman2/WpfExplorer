using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;
using ExplorerCtrl;

namespace Devices.Terminal
{
    [DebuggerDisplay("{Name}")]
    public class TerminalDevice : IDevice//, IEquatable<IDevice>, IComparable<IDevice>
    {
        internal SerialPort device;

        internal TerminalDevice(string portName)
        {
            this.device = new SerialPort(portName, 115200);
            this.device.Open();
        }

        public string Name
        {
            get
            {
                return this.device.PortName;
            }
        }

        public string Id
        {
            get
            {
                return this.device.PortName;
            }
        }
        
        public IExplorerItem Root
        {
            get
            {
                return new TerminalEntry(this, "/");
            }
        }
        
        public bool Equals(IDevice other)
        {
            if (other.GetType() != typeof(TerminalDevice))
            {
                return false;
            }
            return this.device.PortName == (other as TerminalDevice).device.PortName;
        }

        public int CompareTo(IDevice other)
        {
            return this.Name.CompareTo(other.Name);
        }
        
        public bool FolderExist(string path)
        {
            throw new NotSupportedException();
        }
        
        public bool HasDeviceInfo
        {
            get { return true; }
        }

        public Dictionary<string, string> GetDeviceInfo()
        {
            var deviceInfo = new Dictionary<string, string>();
            deviceInfo.Add("PortName", this.device.PortName);
            return deviceInfo;
        }

        public bool HasMemoryInfo
        {
            get { return false; }
        }

        public Dictionary<string, DeviceMemoryInfo> GetMemoryInfo()
        {
            throw new NotImplementedException();
        }

        public void Reboot()
        {
            throw new NotSupportedException();
        }

        private const string linePatter =
              @"(?<type>[\-dlcbn])[\-r][\-w][\-x][\-r][\-w][\-x][\-r][\-w][\-x]"            // Rights
            + @"\s+\d+"                                                                     // ln
            + @"\s+\S+"                                                                     // Owner
            + @"\s+\S+"                                                                     // Group
            + @"\s+(/d,\s+)?"                                                               // ??
            + @"(?<size>\d+)\s+"                                                            // Size
            + @"(?<month>\S{3})\s+(?<day>\d{2})\s+((?<year>\d{4})|(?<time>\d{2}:\d{2}))\s+" // Date
            + @"(?<name>\S+)"                                                               // Filename
            + @"(\s->\s(?<link>\S+))?"                                                      // Link
            ;

        internal List<TerminalEntry> ReadDirectory(string path)
        {
            List<TerminalEntry> list = new List<TerminalEntry>();

            this.device.ReadExisting();

            this.device.WriteLine($"ls -la {path}");
            Thread.Sleep(1000);
            while (this.device.BytesToRead > 0)
            {
                string line = this.device.ReadLine();
                if (line.Contains("ls -la"))
                {
                }
                else if (line.StartsWith("total"))
                {
                    Match match = Regex.Match(line, @"total (?<size>\d+)");
                    if (match.Success)
                    {
                        long size = long.Parse(match.Groups["size"].Value);
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    Match match = Regex.Match(line, linePatter);
                    if (match.Success)
                    {
                        string type = match.Groups["type"].Value;
                        long size = long.Parse(match.Groups["size"].Value);
                        string month = match.Groups["month"].Value;
                        string year = match.Groups["year"].Value;
                        string time = match.Groups["time"].Value;
                        string name = match.Groups["name"].Value;
                        string link = match.Groups["link"].Value;


                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            return list.Any() ? list : null;
        }
    }
}
