using ExplorerCtrl;
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
    public class WindowsDevice : IDevice//, IEquatable<IDevice>, IComparable<IDevice>
    {
        private DriveInfo deviceInfo;

        internal WindowsDevice(DriveInfo deviceInfo)
        {
            this.deviceInfo = deviceInfo;
        }

        #region IDevice

        public string Name
        {
            get
            {
                return this.deviceInfo.Name;
            }
        }

        #endregion

        //public string Id
        //{
        //    get
        //    {
        //        return this.deviceInfo.Name;
        //    }
        //}

        public IExplorerItem Root
        {
            get
            {
                return new WindowsEntry(this, this.deviceInfo.RootDirectory);
            }
        }

        
    }
}
