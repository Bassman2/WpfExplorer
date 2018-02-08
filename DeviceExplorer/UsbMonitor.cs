using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Devices
{
    public enum UsbDeviceClass
    {
        [Description("Unknown")]
        [GuidAttribute()]
        Unknown = 0,

        [Description("Windows Portable Devices")]
        [GuidAttribute("eec5ad98-8080-425f-922a-dabf3de3f69a")]
        WPD,

        [Description("CD/DVD/Blu-ray drives")]
        [GuidAttribute("4D36E965-E325-11CE-BFC1-08002BE10318")]
        CDROM,

        [Description("Hard drives")]
        [GuidAttribute("4D36E967-E325-11CE-BFC1-08002BE10318")]
        DiskDrive,

        [Description("Video adapters")]
        [GuidAttribute("4D36E968-E325-11CE-BFC1-08002BE10318")]
        Display,

        [Description("Floppy controllers")]
        [GuidAttribute("4D36E969-E325-11CE-BFC1-08002BE10318")]
        FDC,

        [Description("Floppy drives")]
        [GuidAttribute("4D36E980-E325-11CE-BFC1-08002BE10318")]
        FloppyDisk,

        [Description("Hard drive controllers")]
        [GuidAttribute("4D36E96A-E325-11CE-BFC1-08002BE10318")]
        HDC,

        [Description("Some USB devices")]
        [GuidAttribute("745A17A0-74D3-11D0-B6FE-00A0C90F57DA")]
        HIDClass,

        [Description("IEEE 1394 host controller")]
        [GuidAttribute("6BDD1FC1-810F-11D0-BEC7-08002BE2092F")]
        IEEE1394,

        [Description("Cameras and scanners")]
        [GuidAttribute("6BDD1FC6-810F-11D0-BEC7-08002BE2092F")]
        Image,

        [Description("Keyboards")]
        [GuidAttribute("4D36E96B-E325-11CE-BFC1-08002BE10318")]
        Keyboard,

        [Description("Modems")]
        [GuidAttribute("4D36E96D-E325-11CE-BFC1-08002BE10318")]
        Modem,

        [Description("Mice and pointing devices")]
        [GuidAttribute("4D36E96F-E325-11CE-BFC1-08002BE10318")]
        Mouse,

        [Description("Audio and video devices")]
        [GuidAttribute("4D36E96C-E325-11CE-BFC1-08002BE10318")]
        Media,

        [Description("Network adapters")]
        [GuidAttribute("4D36E972-E325-11CE-BFC1-08002BE10318")]
        Net,

        [Description("Serial and parallel ports")]
        [GuidAttribute("4D36E978-E325-11CE-BFC1-08002BE10318")]
        Ports,

        [Description("SCSI and RAID controllers")]
        [GuidAttribute("4D36E97B-E325-11CE-BFC1-08002BE10318")]
        SCSIAdapter,

        [Description("System buses, bridges, etc.")]
        [GuidAttribute("4D36E97D-E325-11CE-BFC1-08002BE10318")]
        System,

        [Description("USB host controllers and hubs")]
        [GuidAttribute("36FC9E60-C465-11CF-8056-444553540000")]
        USB,

        [Description("Printers")]
        [GuidAttribute("4d36e979-e325-11ce-bfc1-08002be10318")]
        Printer,

        [Description("MTP Contact Service")]
        [GuidAttribute("DD04D5FC-9D6E-4F76-9DCF-ECA6339B7389")]
        MTPContactService,

        [Description("MTP Calendar Service")]
        [GuidAttribute("E4DFDBD3-7F04-45E9-9FA1-5CA0EAEB0AE3")]
        MTPCalendarService,

        [Description("MTP Notes Service")]
        [GuidAttribute("5c017aea-e706-4719-8cc0-a303836fd321")]
        MTPNotesService,

        [Description("MTP Task Service")]
        [GuidAttribute("BB340C54-B5C6-491D-8827-28D0E7631903")]
        MTPTaskService,

        [Description("MTP Status Service")]
        [GuidAttribute("0B9F1048-B94B-DC9A-4ED7-FE4FED3A0DEB")]
        MTPStatusService,

        [Description("MTP Hints Service")]
        [GuidAttribute("c8a98b1f-6b19-4e79-a414-67ea4c39eec2")]
        MTPHintsService,

        [Description("MTP Device Metadata Service")]
        [GuidAttribute("332ffe6a-af65-41e1-a0af-d3e2627bdf54")]
        MTPDeviceMetadataService,

        [Description("MTP Ringtone Service")]
        [GuidAttribute("d0eace0e-707d-4106-8d38-4f560e6a9f8e")]
        MTPRingtoneService,

        [Description("MTP Enumeration Synchronization Service")]
        [GuidAttribute("28d3aac9-c075-44be-8881-65f38d305909")]
        MTPEnumerationSynchronizationService,

        [Description("MTP Anchor Synchronization Service")]
        [GuidAttribute("056d8b9e-ad7a-44fc-946f-1d63a25cda9a")]
        MTPAnchorSynchronizationService,


    }

    public enum UsbDeviceAction
    {
        Add = 1,
        QueryRemove = 2,
        RemoveComplete = 3,
        Changed = 4
    }

    public class UsbEventArgs : EventArgs
    {
        public UsbEventArgs(UsbDeviceAction action)
        {
            this.Action = action;
        }

        public UsbEventArgs(UsbDeviceAction action, Guid classGuid, UsbDeviceClass classId, string name)
        {
            this.Action = action;
            this.ClassGuid = classGuid;
            this.Class = classId;
            this.Name = name;
        }

        public UsbDeviceAction Action { get; private set; }
        public Guid ClassGuid { get; private set; }
        public UsbDeviceClass Class { get; private set; }
        public string Name { get; private set; }
    }

    internal class GuidAttribute : Attribute
    {
        public GuidAttribute()
        {
            this.Guid = Guid.Empty;
        }

        public GuidAttribute(uint a, ushort b, ushort c, byte d, byte e, byte f, byte g, byte h, byte i, byte j, byte k)
        {
            this.Guid = new Guid(a, b, c, d, e, f, g, h, i, j, k);
        }

        public GuidAttribute(string guid)
        {
            this.Guid = new Guid(guid);
        }

        public Guid Guid { get; private set; }
    }


    public class UsbMonitor
    {
        private const int WM_DEVICECHANGE = 0x0219;

        private const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
        private const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004;

        private const int DBT_DEVTYP_DEVICEINTERFACE = 0x00000005;

        private const int DBT_DEVNODES_CHANGED = 0x0007;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEQUERYREMOVE = 0x8001;
        private const int DBT_DEVICEQUERYREMOVEFAILED = 0x8002;
        private const int DBT_DEVICEREMOVEPENDING = 0x8003;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;
        private const int DBT_DEVICETYPESPECIFIC = 0x8005;
        private const int DBT_CUSTOMEVENT = 0x8006;

        private IntPtr windowHandle;
        private IntPtr deviceEventHandle;

        public event EventHandler<UsbEventArgs> UsbUpdate;
        public event EventHandler UsbChanged;

        public UsbMonitor(Window window, bool start = true)
        {
            windowHandle = new WindowInteropHelper(window).EnsureHandle();
            HwndSource.FromHwnd(windowHandle)?.AddHook(HwndHandler);
            if (start) Start();
        }

        public void Start()
        {
            int size = Marshal.SizeOf(typeof(NativeMethods.DEV_BROADCAST_DEVICEINTERFACE));
            var deviceInterface = new NativeMethods.DEV_BROADCAST_DEVICEINTERFACE();
            deviceInterface.dbcc_size = size;
            deviceInterface.dbcc_devicetype = DBT_DEVTYP_DEVICEINTERFACE;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(deviceInterface, buffer, true);
            this.deviceEventHandle = NativeMethods.RegisterDeviceNotification(windowHandle, buffer, DEVICE_NOTIFY_WINDOW_HANDLE | DEVICE_NOTIFY_ALL_INTERFACE_CLASSES);
            if (deviceEventHandle == IntPtr.Zero)
            {
                int error = Marshal.GetLastWin32Error();
            }
            Marshal.FreeHGlobal(buffer);
        }

        public void Stop()
        {
            if (deviceEventHandle != IntPtr.Zero)
            {
                NativeMethods.UnregisterDeviceNotification(deviceEventHandle);
            }
            deviceEventHandle = IntPtr.Zero;
        }

        private IntPtr HwndHandler(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam, ref bool handled)
        {
            if (msg == WM_DEVICECHANGE)
            {
                switch (wparam.ToInt32())
                {
                case DBT_DEVICEARRIVAL:
                    if (Marshal.ReadInt32(lparam, 4) == DBT_DEVTYP_DEVICEINTERFACE)
                    {
                        this.UsbUpdate?.Invoke(this, CreateUsbEventArgs(UsbDeviceAction.Add, lparam));
                    }
                    break;
                case DBT_DEVICEQUERYREMOVE:
                    if (Marshal.ReadInt32(lparam, 4) == DBT_DEVTYP_DEVICEINTERFACE)
                    {
                        this.UsbUpdate?.Invoke(this, CreateUsbEventArgs(UsbDeviceAction.QueryRemove, lparam));
                    }
                    break;
                case DBT_DEVICEREMOVECOMPLETE:
                    if (Marshal.ReadInt32(lparam, 4) == DBT_DEVTYP_DEVICEINTERFACE)
                    {
                        this.UsbUpdate?.Invoke(this, CreateUsbEventArgs(UsbDeviceAction.RemoveComplete, lparam));
                    }
                    break;
                case DBT_DEVNODES_CHANGED:
                    this.UsbChanged?.Invoke(this, new EventArgs());
                    break;
                }
            }
            handled = false;
            return IntPtr.Zero;
        }

        private UsbEventArgs CreateUsbEventArgs(UsbDeviceAction action, IntPtr lparam)
        {

            int size = Marshal.ReadInt32(lparam, 0);
            var deviceInterface = Marshal.PtrToStructure<UsbMonitor.NativeMethods.DEV_BROADCAST_DEVICEINTERFACE>(lparam);
            var classGuid = new Guid(deviceInterface.dbcc_classguid);
            var classId = GuidToDeviceClass(classGuid);
            var name = new string(deviceInterface.dbcc_name.TakeWhile(c => c != 0).ToArray());
            return new UsbEventArgs(action, classGuid, classId, name);
        }

        private UsbDeviceClass GuidToDeviceClass(Guid guid)
        {
            UsbDeviceClass en = Enum.GetValues(typeof(UsbDeviceClass)).Cast<UsbDeviceClass>().Where(e =>
            {
                return e.GetType().GetField(e.ToString()).GetCustomAttribute<GuidAttribute>().Guid == guid;
            }).FirstOrDefault();
            return en;
        }

        internal static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            public struct DEV_BROADCAST_DEVICEINTERFACE
            {
                public Int32 dbcc_size;
                public Int32 dbcc_devicetype;
                public Int32 dbcc_reserved;
                [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
                public byte[] dbcc_classguid;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
                public char[] dbcc_name;
//                public StringBuilder dbcc_name;
            }

            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, Int32 Flags);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool UnregisterDeviceNotification(IntPtr hHandle);
        }
    }
}
