using System;
using System.Collections.Generic;

namespace Devices
{
    public interface IClient
    {
        event EventHandler DevicesChanged;

        string Name { get; }

        string Description { get; }

        IEnumerable<IDevice> Devices { get; }
    }
}
