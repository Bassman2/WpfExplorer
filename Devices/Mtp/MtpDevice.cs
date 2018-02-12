using Devices.Internal;
using ExplorerCtrl;
using MediaDevices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices.Mtp
{
    [DebuggerDisplay("{Name}")]
    public class MtpDevice : IDevice//, IEquatable<IDevice>, IComparable<IDevice>
    {
        internal MediaDevice device;
        
        internal MtpDevice(MediaDevice device)
        {
            this.device = device;
            this.device.Connect();
        }

        public string Name
        {
            get
            {
                return string.IsNullOrEmpty(this.device.FriendlyName) ? this.device.Model : this.device.FriendlyName;
            }
        }

        //public string Id
        //{
        //    get
        //    {
        //        return this.device.DeviceId;
        //    }
        //}

        
        public IExplorerItem Root
        {
            get
            {
                return new MtpEntry(this, this.device.GetRootDirectory());

            }
        }

        
    
        //public bool Equals(IDevice other)
        //{
        //    if (other.GetType() != typeof(MtpDevice))
        //    {
        //        return false;
        //    }
        //    return this.device.DeviceId == (other as MtpDevice).device.DeviceId;
        //}

        //public int CompareTo(IDevice other)
        //{
        //    return this.Name.CompareTo(other.Name);
        //}

        
    }
}
