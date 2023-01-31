using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices
{
    public class DeviceMemoryInfo
    {
        public DeviceMemoryInfo(long free)
        {
            this.Total = 2 * 1024 * 1024;
            this.Free = free;
            this.Used = this.Total - free;
        }

        public DeviceMemoryInfo(long total, long used, long free)
        {
            this.Total = total;
            this.Used = used;
            this.Free = free;
        }

        public long Total { get; private set; }
        public long Used { get; private set; }
        public long Free { get; private set; }

        public double Percentage
        {
            get
            {
                return this.Total != 0 ? ((double)this.Used / this.Total) : 0.0;
            }
        }
    }
}
