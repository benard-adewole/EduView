using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace Estudiar.Converters
{
    public class BoolToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
            {
                return (Color)Application.Current.Resources["AppColor"];
            }
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                return Color.Transparent;
            }
            else
            {
                return Color.Transparent;
            }
            //return ? : ("LightGray"==(parameter as string)?Color.LightGray:Color.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
