using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Windows
{
    [DebuggerDisplay("{Name}")]
    public class WindowsDevice : IDevice, IEquatable<IDevice>, IComparable<IDevice>
    {
        private DriveInfo deviceInfo;

        internal WindowsDevice(DriveInfo deviceInfo)
        {
            this.deviceInfo = deviceInfo;
        }

        public string Name
        {
            get
            {
                return this.deviceInfo.Name;
            }
        }

        public string Id
        {
            get
            {
                return this.deviceInfo.Name;
            }
        }

        public IEntry Root
        {
            get
            {
                return new WindowsEntry(this, this.deviceInfo.RootDirectory);
            }
        }

        public bool Equals(IDevice other)
        {
            if (other.GetType() != typeof(WindowsDevice))
            {
                return false;
            }
            return this.deviceInfo.Name == (other as WindowsDevice).deviceInfo.Name;
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
            deviceInfo.Add("Name", this.deviceInfo.Name);
            deviceInfo.Add("VolumeLabel", this.deviceInfo.VolumeLabel);
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
    }
}
