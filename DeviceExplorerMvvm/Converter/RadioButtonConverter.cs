using System;
using System.Windows.Data;

namespace DeviceExplorer.Converter
{
    /// <summary>
    /// Converter to bind radio buttons to an enum.
    /// </summary>
    /// <example>
    /// &lt;Window.Resources&gt;
    ///     &lt;RadioButtonConverter x:Key="RadioButtonConverter"/&gt;
    /// &lt;/Window.Resources&gt;
    /// ...
    /// &lt;RadioButton Content="A" IsChecked="{Binding BindVar, Converter={StaticResource RadioButtonConverter}, ConverterParameter={x:Static local:MyEnum.A}}" /&gt;
    /// &lt;RadioButton Content="B" IsChecked="{Binding BindVar, Converter={StaticResource RadioButtonConverter}, ConverterParameter={x:Static local:MyEnum.B}}" /&gt;
    /// &lt;RadioButton Content="C" IsChecked="{Binding BindVar, Converter={StaticResource RadioButtonConverter}, ConverterParameter={x:Static local:MyEnum.C}}" /&gt;
    /// </example>
    [ValueConversion(typeof(object), typeof(bool))]    
    public class RadioButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return parameter.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return parameter;
        }
    }
}
