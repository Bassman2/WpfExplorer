using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Devices
{
    public interface IClient
    {
        //event EventHandler DevicesChanged;

        string Name { get; }

        string Description { get; }

        ImageSource Icon { get; }

        IEnumerable<IDevice> Devices { get; }
    }
}
