using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devices
{
    public enum DeviceType
    {
        /// <summary>
        /// Windows File System
        /// </summary>
        Windows,

        /// <summary>
        /// Android Debug Bridge
        /// </summary>
        ADB,

        /// <summary>
        /// Media Transport Protocol
        /// </summary>
        MTP,

        /// <summary>
        /// Serial Terminal over COM port
        /// </summary>
        Terminal
    }
}
