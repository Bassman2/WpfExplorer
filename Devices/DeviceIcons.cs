using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Devices
{
    internal static class DeviceIcons
    {
        private static ImageSource imgWindows;
        private static ImageSource imgAndroid;
        private static ImageSource imgTerminal;
        private static ImageSource imgDevice;
        private static ImageSource imgFolder;
        private static ImageSource imgFile;
        private static ImageSource imgLink;
        //private static ImageSource imgDefault;

        static DeviceIcons()
        {
            try
            {
                imgWindows = new BitmapImage(new Uri("pack://application:,,,/Devices;component/Images/Windows.png"));
                imgAndroid = new BitmapImage(new Uri("pack://application:,,,/Devices;component/Images/Android.png"));
                imgTerminal = new BitmapImage(new Uri("pack://application:,,,/Devices;component/Images/Terminal.png"));
                imgDevice = new BitmapImage(new Uri("pack://application:,,,/Devices;component/Images/Device.png"));
                imgFolder = new BitmapImage(new Uri("pack://application:,,,/Devices;component/Images/Folder.png"));
                imgFile = new BitmapImage(new Uri("pack://application:,,,/Devices;component/Images/File.png"));
                imgLink = new BitmapImage(new Uri("pack://application:,,,/Devices;component/Images/Link.png"));
                //imgDefault = new BitmapImage(new Uri("pack://application:,,,/Images/Default.png"));
            }
            catch (Exception ex)
            {
            }
        }

        public static ImageSource Windows { get { return imgWindows; } }
        public static ImageSource Android { get { return imgAndroid; } }
        public static ImageSource Terminal { get { return imgTerminal; } }
        public static ImageSource Device { get { return imgDevice; } }
        public static ImageSource Folder { get { return imgFolder; } }
        public static ImageSource File { get { return imgFile; } }
        public static ImageSource Link { get { return imgLink; } }
        //public static ImageSource Default { get { return imgDefault; } }
    }
}
