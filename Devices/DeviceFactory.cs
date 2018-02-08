using Devices.Adb;
using Devices.Mtp;
using Devices.Terminal;
using Devices.Windows;
using System.Collections.Generic;

namespace Devices
{
    public static class DeviceFactory
    {
        private static Dictionary<DeviceType, IClient> clients;

        static DeviceFactory()
        {
            clients = new Dictionary<DeviceType, IClient>();
            clients.Add(DeviceType.Windows, new WindowsClient());
            clients.Add(DeviceType.ADB, new AdbClient());
            clients.Add(DeviceType.MTP, new MtpClient());
            clients.Add(DeviceType.Terminal, new TerminalClient());
        }

        public static IEnumerable<IClient> Clients
        {
            get { return clients.Values; }
        }

        public static IClient GetClient(DeviceType deviceType)
        {
            return clients[deviceType];
        }
    }
}
