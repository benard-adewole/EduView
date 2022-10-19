using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Estudiar.Models;

namespace Estudiar.Converters
{
    public class CollecctionControlConverter : IValueConverter
    {
        int max = 3;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return value;
            }
            max = int.Parse( parameter.ToString());
            var list = new ObservableCollection<Material>();
            for (int i = 0; i < Math.Min(max, (value as ObservableCollection<Material>).Count); i++)
            {
                list.Add((value as ObservableCollection<Material>)[i]);
            }
            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
