using ExplorerCtrl;
using System;
using System.Collections.Generic;

namespace Devices
{
    public interface IDevice //: IEquatable<IDevice>, IComparable<IDevice>
    {
        string Name { get; }

        //string Id { get; }

        IExplorerItem Root { get; }

        //bool FolderExist(string path);
        
        //bool HasDeviceInfo { get; }

        //Dictionary<string, string> GetDeviceInfo();

        //bool HasMemoryInfo { get; }

        //Dictionary<string, DeviceMemoryInfo> GetMemoryInfo();
        
        //void Reboot();
    }
}
