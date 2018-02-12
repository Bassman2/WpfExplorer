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

        public string Id
        {
            get
            {
                return this.device.DeviceId;
            }
        }

        //public void CreateFolder(string path)
        //{
        //    this.device.CreateDirectory(path);
        //}

        //public void CreateLink(string path, string link)
        //{
        //    throw new NotSupportedException();
        //}

        //public void Delete(string path)
        //{
        //    if (this.device.DirectoryExists(path))
        //    {
        //        this.device.DeleteDirectory(path);
        //    }
        //    else if (this.device.FileExists(path))
        //    {
        //        this.device.DeleteFile(path);
        //    }
        //}

        public IExplorerItem Root
        {
            get
            {
                return new MtpEntry(this, this.device.GetDirectoryInfo("/"));

            }
        }

        //public IEnumerable<IEntry> GetEntries(string path)
        //{
        //    return this.device.EnumerateFileSystemEntries(path).Select(f => new MtpEntry(this, UnixPath.Combine(path, f))).OrderBy(e => e.Name);
        //}

        //public IEnumerable<IEntry> GetFolders(string path)
        //{
        //    var files = this.device.EnumerateFileSystemEntries(path).ToList();
        //    var folder = this.device.EnumerateDirectories(path).ToList();
        //    return folder.Select(f => new MtpEntry(this, UnixPath.Combine(path, f))).OrderBy(e => e.Name);
        //}

        //public DeviceInfo GetInfo()
        //{
        //    throw new NotSupportedException();
        //}

        //public void Pull(string path, Stream stream)
        //{
        //    this.device.DownloadFile(path, stream);
        //}

        //public void Push(Stream stream, string path)
        //{
        //    this.device.UploadFile(stream, path);
        //}
    
        public bool Equals(IDevice other)
        {
            if (other.GetType() != typeof(MtpDevice))
            {
                return false;
            }
            return this.device.DeviceId == (other as MtpDevice).device.DeviceId;
        }

        public int CompareTo(IDevice other)
        {
            return this.Name.CompareTo(other.Name);
        }

        //public override string ToString()
        //{
        //    return this.Name;
        //}

        public bool FolderExist(string path)
        {
            return this.device.DirectoryExists(path);
        }

        //public DeviceInfo GetDeviceInfo()
        //{
        //    throw new NotImplementedException();
        //}

        //public DeviceMemory GetDeviceMemory()
        //{
        //    throw new NotImplementedException();
        //}

        public bool HasDeviceInfo
        {
            get { return true; }
        }

        public Dictionary<string, string> GetDeviceInfo()
        {
            var deviceInfo = new Dictionary<string, string>();
            deviceInfo.Add("FriendlyName", this.device.FriendlyName);
            deviceInfo.Add("Description", this.device.Description);
            deviceInfo.Add("DeviceId", this.device.DeviceId);
            deviceInfo.Add("DeviceType", this.device.DeviceType.ToString());
            deviceInfo.Add("FirmwareVersion", this.device.FirmwareVersion);
            //deviceInfo.Add("Manufacture", this.device.Manufacture);
            deviceInfo.Add("Model", this.device.Model);
            deviceInfo.Add("PnPDeviceID", this.device.PnPDeviceID);
            deviceInfo.Add("PowerLevel", this.device.PowerLevel.ToString());
            deviceInfo.Add("PowerSource", this.device.PowerSource.ToString());
            deviceInfo.Add("Protocol", this.device.Protocol);
            deviceInfo.Add("SerialNumber", this.device.SerialNumber);
            deviceInfo.Add("Transport", this.device.Transport.ToString());
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
            this.device.ResetDevice();
        }
    }
}
