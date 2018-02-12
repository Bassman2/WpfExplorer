using ExplorerCtrl.Internal;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ExplorerCtrl.Converter
{
    /// <summary>
    /// Convert IExplorerItem to size string
    /// </summary>
    [ValueConversion(typeof(ExplorerItem), typeof(string))]
    internal class ItemToSizeStringConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ExplorerItem item = value as ExplorerItem;
            if (item != null && item.Type == ExplorerItemType.File)
            {
                long k = (long)Math.Ceiling(item.Size / 1024.0);
                return $"{k:N0} KB";

                //long m = (long)Math.Ceiling(item.Size / (1024.0 * 1024.0));
                //long g = (long)Math.Ceiling(item.Size / (1024.0 * 1024.0 * 1024.0));

                //if (m >= 1000000)
                //{
                //    return $"{g:N0} GB";
                //}
                //else if (k >= 1000000)
                //{
                //    return $"{m:N0} MB";
                //}
                //else
                //{
                //    return $"{k:N0} KB";
                //}
            }
            return null;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A converted value. If the method returns null, the valid null value is used.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
