using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Estudiar.Models;

namespace Estudiar.Converters
{
    public class SubStringConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string[] start_length = parameter.ToString().Split(' ');
            try
            {
                return value.ToString().Substring(int.Parse(start_length[0]), int.Parse(start_length[1]));

            }
            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
