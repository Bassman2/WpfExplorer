using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Devices
{
    internal static class DeviceIcons
    {
        private static ImageSource imgDevice;
        private static ImageSource imgFolder;
        private static ImageSource imgFile;
        private static ImageSource imgLink;
        private static ImageSource imgDefault;

        static DeviceIcons()
        {
            imgDevice = new BitmapImage(new Uri("pack://application:,,,/Images/Device.png"));
            imgFolder = new BitmapImage(new Uri("pack://application:,,,/Images/Folder.png"));
            imgFile = new BitmapImage(new Uri("pack://application:,,,/Images/File.png"));
            imgLink = new BitmapImage(new Uri("pack://application:,,,/Images/Link.png"));
            imgDefault = new BitmapImage(new Uri("pack://application:,,,/Images/Default.png"));
        }

        public static ImageSource Device { get { return imgDevice; } }
        public static ImageSource Folder { get { return imgFolder; } }
        public static ImageSource File { get { return imgFile; } }
        public static ImageSource Link { get { return imgLink; } }
        public static ImageSource Default { get { return imgDefault; } }
    }
}
