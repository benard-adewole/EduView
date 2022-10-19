using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using Estudiar.Models;

namespace Estudiar.Converters
{
    public class TextToColorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                foreach (var faculty in FacDeptImg.RetrieveData())
                {
                    foreach (var dept in faculty.Departments)
                    {
                        if (dept.Department.ToLower() == value.ToString().ToLower())
                        {
                            return Color.FromHex( dept.Color);
                        }
                    }
                }   
            }
            return (Color)Application.Current.Resources["AppColor"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
