using Devices;
using SharpAdbClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ExtAdbClient = SharpAdbClient.AdbClient;


namespace Devices.Adb
{
    [DebuggerDisplay("{Name}")]
    public class AdbDevice : IDevice, IEquatable<IDevice>, IComparable<IDevice>
    {
        internal ExtAdbClient client;
        internal DeviceData deviceData;

        internal AdbDevice(ExtAdbClient client, DeviceData deviceData)
        {
            this.client = client;
            this.deviceData = deviceData;
        }

        public string Name
        {
            get
            {
                return this.deviceData?.Name ?? this.deviceData.Product;
            }
        }

        public string Id
        {
            get
            {
                return this.deviceData.Serial;
            }
        }

        //public void CreateFolder(string path)
        //{
        //    ExcecuteCommand($"mkdir -p \"{path}\"");
        //}

        //public void CreateLink(string path, string link)
        //{
        //    ExcecuteCommand($"ln -s {link} {path}");
        //}

        //public void Delete(string path)
        //{
        //    ExcecuteCommand($"rm -r {path}");
        //}

        public IEntry Root
        {
            get
            {
                return new AdbEntry(this, new FileStatistics() { FileMode = UnixFileMode.Directory, Path = "/", Size = 0, Time = DateTime.MinValue }, null);
            }
        }

        //public IEnumerable<IEntry> GetEntries(string path)
        //{
        //    using (SyncService service = new SyncService(this.deviceData))
        //    {
        //        return service.GetDirectoryListing(path).OrderBy(f => f.Path).Select(f => new AdbEntry(this, f, path));
        //    }
        //}

        //public IEnumerable<IEntry> GetFolders(string path)
        //{
        //    using (SyncService service = new SyncService(this.deviceData))
        //    {
        //        return service.GetDirectoryListing(path).Where(f => f.Path != "." && f.Path != "..").Select(f => new AdbEntry(this, f, path)).Where(e => e.IsDirectory);
        //    }
        //}
        
        //public void Pull(string src, string dest)
        //{
        //    using (FileStream stream = File.Create(dest))
        //    {
        //        using (SyncService service = new SyncService(this.deviceData))
        //        {
        //            CancellationToken token;
        //            service.Pull(src, stream, null, token);
        //        }
        //    }
        //}

        //public void Pull(string path, Stream stream)
        //{
        //    using (SyncService service = new SyncService(this.deviceData))
        //    {
        //        CancellationToken token;
        //        service.Pull(path, stream, null, token);
        //    }
        //}

        //public void Push(Stream stream, string path)
        //{
        //    using (SyncService service = new SyncService(this.deviceData))
        //    {
        //        CancellationToken token;
        //        service.Push(stream, path, 0x777, DateTime.Now, null, token);
        //    }
        //}

        //public void Push(string src, string dest)
        //{
        //    string path = UnixPath.Combine(dest, Path.GetFileName(src));
        //    PushRecursive(src, path);
        //}

        //private void PushRecursive(string src, string dest)
        //{
        //    if (File.Exists(src))
        //    {
        //        using (FileStream stream = File.OpenRead(src))
        //        {
        //            using (SyncService service = new SyncService(this.deviceData))
        //            {
        //                CancellationToken token;
        //                service.Push(stream, dest, 0x777, DateTime.Now, null, token);
        //            }
        //        }
        //    }
        //    else if (Directory.Exists(src))
        //    {
        //        CreateFolder(dest);
        //        foreach (string entry in Directory.EnumerateFileSystemEntries(src))
        //        {
        //            string path = UnixPath.Combine(dest, Path.GetFileName(entry));
        //            PushRecursive(entry, path);
        //        }
        //    }
        //}

        public bool HasDeviceInfo
        {
            get { return true; }
        }

        public Dictionary<string, string> GetDeviceInfo()
        {
            Dictionary<string, string> deviceInfo = null;
            try
            {
                //deviceInfo.Product = ExcecuteCommandResult("getprop ro.build.product").Trim();
                //deviceInfo.System = "Android";
                //deviceInfo.Version = ExcecuteCommandResult("getprop ro.build.version.release").Trim();
                //deviceInfo.Release = ExcecuteCommandResult("getprop ro.build.version.sdk").Trim();
                //deviceInfo.BuildDate = ExcecuteCommandResult("getprop ro.build.date").Trim();
                //deviceInfo.BuildId = ExcecuteCommandResult("getprop ro.build.id").Trim();
                //deviceInfo.Hardware = ExcecuteCommandResult("getprop ro.hardware").Trim();
                //deviceInfo.Board = ExcecuteCommandResult("getprop ro.product.board").Trim();
                //deviceInfo.CPU = ExcecuteCommandResult("getprop ro.product.cpu.abi").Trim();
                //deviceInfo.Device = ExcecuteCommandResult("getprop ro.product.device").Trim();
                //deviceInfo.Model = ExcecuteCommandResult("getprop ro.product.model").Trim();
                //deviceInfo.OpenGl = ExcecuteCommandResult("getprop ro.opengles.version").Trim();
                //deviceInfo.Language = ExcecuteCommandResult("getprop ro.product.locale.language").Trim() + "-" + ExcecuteCommandResult("getprop ro.product.locale.region").Trim();
                //deviceInfo.Timezone = ExcecuteCommandResult("getprop persist.sys.timezone").Trim();
                //deviceInfo.Fingerprint = ExcecuteCommandResult("getprop ro.build.fingerprint").Trim();
                //deviceInfo.Fingerprint = ExcecuteCommandResult("getprop ro.build.fingerprint").Trim();
                //deviceInfo.Heapsize = ExcecuteCommandResult("getprop dalvik.vm.heapsize").Trim();

                deviceInfo = new Dictionary<string, string>();
                string prop = ExcecuteCommandResult("getprop");
                foreach (string line in prop.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Match match = Regex.Match(line, @"\[(?<name>[^\]]+)\]: \[(?<value>[^\]]+)\]");
                    if (match.Success)
                    {
                        string name = match.Groups["name"].Value;
                        string value = match.Groups["value"].Value;
                        deviceInfo.Add(name, value);
                    }
                }
            }
            catch { }
            return deviceInfo;
        }

        public bool HasMemoryInfo
        {
            get { return true; }
        }

        private bool useBusybox = true;
        private bool useVmstat = true; 

        private const long Kilo = 1024;
        private const long Mega = 1024 * 1024;
        private const long Giga = 1024 * 1024 * 1024;

        public Dictionary<string, DeviceMemoryInfo> GetMemoryInfo()
        {
            Dictionary<string, DeviceMemoryInfo> deviceMemory = new Dictionary<string, DeviceMemoryInfo>();
            if (useBusybox)
            {
                try
                {
                    // busybox free -m; vmstat -n 1
                    string res = ExcecuteCommandResult("busybox free -m");
                    Match match = Regex.Match(res, @"[^\n]*\nMem:\s+(?<total>\d+)\s+(?<used>\d+)\s+(?<free>\d+)");
                    if (!match.Success)
                    {
                        throw new Exception("Parse free failed");
                    }
                    deviceMemory.Add("System", new DeviceMemoryInfo(
                        long.Parse(match.Groups["total"].Value) * Mega,
                        long.Parse(match.Groups["used"].Value) * Mega,
                        long.Parse(match.Groups["free"].Value) * Mega));
                }
                catch
                {
                    useBusybox = false;
                }
            }

            if (!useBusybox && useVmstat)
            {
                try
                {
                    string res = ExcecuteCommandResult("vmstat -n 1");
                    Match match = Regex.Match(res, @"[^\n]*\n[^\n]*\n\s*(?<r>\d+)\s+(?<b>\d+)\s+(?<free>\d+)\s+(?<mapped>\d+)\s+(?<anon>\d+)\s+(?<slab>\d+)");
                    if (!match.Success)
                    {
                        throw new Exception("Parse vmstat failed");
                    }
                    
                    deviceMemory.Add("System", new DeviceMemoryInfo (long.Parse(match.Groups["free"].Value))); 
                }
                catch
                {
                    useBusybox = false;
                }
            }

            try
            {
                string res = ExcecuteCommandResult("df");
                foreach (string line in res.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.StartsWith("/data"))
                    {
                        Match match = Regex.Match(line, @"\D*(?<size>[\d\.]+)(?<sizeUnit>[MKG])\s+(?<used>[\d\.]+)(?<usedUnit>[MKG])\s+(?<free>[\d\.]+)(?<freeUnit>[MKG])");
                        if (match.Success)
                        {
                            deviceMemory.Add("Local", new DeviceMemoryInfo(
                                GetSize(match, "size"),
                                GetSize(match, "used"),
                                GetSize(match, "free")));
                        }
                    }
                    else if (line.StartsWith("/storage/sdcard0"))
                    {
                        Match match = Regex.Match(line, @"\D*(?<size>[\d\.]+)(?<sizeUnit>[MKG])\s+(?<used>[\d\.]+)(?<usedUnit>[MKG])\s+(?<free>[\d\.]+)(?<freeUnit>[MKG])");
                        if (match.Success)
                        {
                            deviceMemory.Add("SDCard", new DeviceMemoryInfo(
                                GetSize(match, "size"),
                                GetSize(match, "used"),
                                GetSize(match, "free")));
                        }
                    }
                    else if (line.StartsWith("/storage/udisk"))
                    {
                        Match match = Regex.Match(line, @"\D*(?<size>[\d\.]+)(?<sizeUnit>[MKG])\s+(?<used>[\d\.]+)(?<usedUnit>[MKG])\s+(?<free>[\d\.]+)(?<freeUnit>[MKG])");
                        if (match.Success)
                        {
                            deviceMemory.Add("USB", new DeviceMemoryInfo(
                                GetSize(match, "size"),
                                GetSize(match, "used"),
                                GetSize(match, "free")));
                        }
                    }
                } 

            }
            catch
            { }

            try
            {
                string res = ExcecuteCommandResult("dumpsys meminfo com.elektrobit.mobile.companion");
                foreach (string line in res.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (line.Trim().StartsWith("Native Heap"))
                    {
                        Match match = Regex.Match(line, @"\D*\d+\s+\d+\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<used>\d+)\s+(?<free>\d+)");
                        if (match.Success)
                        {
                            deviceMemory.Add("AppNative", new DeviceMemoryInfo(
                                long.Parse(match.Groups["size"].Value) * Kilo,
                                long.Parse(match.Groups["used"].Value) * Kilo,
                                long.Parse(match.Groups["free"].Value) * Kilo));
                        }
                    }
                    else if (line.Trim().StartsWith("Dalvik Heap"))
                    {
                        Match match = Regex.Match(line, @"\D*\d+\s+\d+\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<used>\d+)\s+(?<free>\d+)");
                        if (match.Success)
                        {
                            deviceMemory.Add("AppDalvik", new DeviceMemoryInfo(
                                long.Parse(match.Groups["size"].Value) * Kilo,
                                long.Parse(match.Groups["used"].Value) * Kilo,
                                long.Parse(match.Groups["free"].Value) * Kilo));
                        }
                    }
                }
            }
            catch 
            { }
            return deviceMemory;
        }

        private long GetSize(Match match, string group)
        {
            string val = match.Groups[group].Value;
            double num = double.Parse(val, new CultureInfo("en-US"));
            string unit = match.Groups[group+"Unit"].Value;
            switch (unit)
            {
            case "K": num = num * Kilo; break;
            case "M": num = num * Mega; break;
            case "G": num = num * Giga; break;
            }
            return (long)Math.Ceiling(num);
        }
        
        public void ExcecuteCommand(string cmd)
        {
            var receiver = new ConsoleOutputReceiver();
            client.ExecuteRemoteCommand(cmd, this.deviceData, receiver);
            string text = receiver.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                throw new Exception(text);
            }
        }

        internal string ExcecuteCommandResult(string cmd)
        {
            var receiver = new ConsoleOutputReceiver();
            client.ExecuteRemoteCommand(cmd, this.deviceData, receiver);
            string text = receiver.ToString();
            if (receiver.ParsesErrors)
            {
                throw new Exception(text); 
            }
            return text;
        }
        
        public bool FolderExist(string path)
        {
            using (SyncService service = new SyncService(this.deviceData))
            {
                FileStatistics fs = service.Stat(path);
                return fs.FileMode != 0;
            }
        }


        //private string RelPath(string path, string rootPath)
        //{
        //    if (!path.StartsWith(rootPath))
        //    {
        //        throw new ArgumentException($"path {path} does not begin with rootPath {rootPath}");
        //    }
        //    return path.Substring(rootPath.Length).TrimStart('/', '\\');
        //}

        public bool Equals(IDevice other)
        {
            if (other.GetType() != typeof(AdbDevice))
            {
                return false;
            }
            return this.deviceData.Serial == (other as AdbDevice).deviceData.Serial;
        }

        public int CompareTo(IDevice other)
        {
            return this.Name.CompareTo(other.Name);
        }

        public void Reboot()
        {
            client.Reboot(this.deviceData);
        }
    }
}
