using System;
using System.Globalization;
using System.Windows.Data;
using Diary.Framework.Help;

namespace Diary.Controls.Converters
{
    public class ByteToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = value as byte[];

            return ImageHelper.ByteToImage(image);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}