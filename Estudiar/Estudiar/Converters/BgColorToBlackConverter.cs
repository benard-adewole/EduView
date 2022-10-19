using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace Estudiar.Converters
{
    public class BgColorToBlackConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((Color)value == (Color)Application.Current.Resources["AppColor"])
                {
                    if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
                    {
                        return Color.White;
                    }
                    else
                    {
                        return Color.Black;
                    }
                    
                }
            }
            //Default
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                return (Color)Application.Current.Resources["MyGray"];
            }
            else
            {
                return Color.LightGray;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
