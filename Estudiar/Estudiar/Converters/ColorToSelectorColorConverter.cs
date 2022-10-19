using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace Estudiar.Converters
{
    public class ColorToSelectorColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((Color)value == (Color)Application.Current.Resources["SelectedColor"])
                {

                    return (Color)Application.Current.Resources["SelectedAnchorColor"]; 
                    //return new SolidColorBrush { Color = (Color)Application.Current.Resources["SelectedColor"] };
                }
            }
            return Color.White ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
