using System;
using System.Globalization;
using System.Windows.Data;
using Diary.Repositories.Enums;

namespace Diary.Controls.Converters
{
    internal class BooleanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
                return Sex.Male;
            return Sex.Female;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((Sex) value == Sex.Male)
                return true;
            return false;
        }
    }
}