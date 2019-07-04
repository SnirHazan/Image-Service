using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Infrastructure;

namespace GUI
{
    /// <summary>
    /// ColorTypeMessageConvertor class - parse the type of the log message to the appropriate color.
    /// </summary>
    class ColorTypeMessageConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(Brush)) {
                throw new InvalidOperationException("must convert to a brush");
            }
            MessageTypeEnum val = (MessageTypeEnum)value;
            if(val == MessageTypeEnum.INFO)
            {
                return Brushes.LightGreen;
            } else if(val == MessageTypeEnum.WARNING)
            {
                return Brushes.Yellow;
            } else
            {
                return Brushes.Coral;
            }


            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
