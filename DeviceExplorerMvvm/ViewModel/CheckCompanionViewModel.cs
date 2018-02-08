using DeviceExplorer.Mvvm;
using Devices;
using Devices.Adb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceExplorer.ViewModel
{
    public class CheckCompanionViewModel : DialogViewModel
    {
        private IDevice device;
        private const string appPath = "/data/data/com.elektrobit.mobile.companion";
        private const string dataPath = "/sdcard/Android/data/com.elektrobit.mobile.companion/files";
        private const string data2Path = "/storage/sdcard0/Android/data/com.elektrobit.mobile.companion/files";
        private const string mapPath = "/storage/udisk/Android/data/com.elektrobit.mobile.companion/files/streetdirector/MapRegions/00/nds";

        public DelegateCommand FixData { get; private set; }


        public CheckCompanionViewModel(IDevice device)
        {
            this.device = device;
            this.AppPath = appPath;
            this.DataPath = dataPath;
            this.MapPath = mapPath;

            this.AppCheck = device.FolderExist(appPath);
            this.DataCheck = device.FolderExist(dataPath);
            this.MapCheck = device.FolderExist(mapPath);

            this.FixData = new DelegateCommand(OnFixData, () => !this.DataCheck);
        }

        private void OnFixData()
        {
            AdbDevice dev = this.device as AdbDevice;
            if (dev != null)
            {
                dev.ExcecuteCommand("mount -t ext4 /dev/block/vold/179:11 /mnt/media_rw/sdcard0");
                this.DataCheck = true;
                NotifyPropertyChanged(nameof(DataCheck));
            }
        }
        public bool AppCheck { get; set; }
        public string AppPath { get; set; }
        public bool DataCheck { get; set; }
        public string DataPath { get; set; }
        public bool MapCheck { get; set; }
        public string MapPath { get; set; }

    }
}
