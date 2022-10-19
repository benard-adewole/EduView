using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using MaterialDesign;
using Xamarin.Forms;

namespace Estudiar.Converters
{
    public class ColorToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Color color = (Color)value;
                if (color == Color.Black)//(Color)Application.Current.Resources["AppColor"])
                {
                    return MaterialDesign.FontIcon.Account;
                }
            }
            return MaterialDesign.FontIcon.AccountAlert;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
