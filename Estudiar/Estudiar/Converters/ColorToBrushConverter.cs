using System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace Estudiar.Converters
{
    
    public class ColorToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if ((Color)value == (Color)Application.Current.Resources["SelectedAnchorColor"])
                {
                    return new SolidColorBrush { Color = (Color)Application.Current.Resources["SelectedAnchorColor"] };
                }
            }
            return new SolidColorBrush { Color = Color.Black };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
