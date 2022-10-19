using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using Estudiar.Models;

namespace Estudiar.Converters
{
    public class ColorAndDeptToColorConverter : IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                foreach (var faculty in FacDeptImg.RetrieveData())
                {
                    foreach (var dept in faculty.Departments)
                    {
                        if (dept.Department.ToLower() == parameter.ToString().ToLower() && (Color)value == (Color)Application.Current.Resources["AppColor"])
                        {
                            return Color.FromHex(dept.Color);
                        }
                    }
                }
                /*if ((Color)value == (Color)Application.Current.Resources["AppColor"])
                {
                    return (Color)Application.Current.Resources["AppColor"];
                }*/
            }
            return Color.Transparent;
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null)
            {
                foreach (var faculty in FacDeptImg.RetrieveData())
                {
                    foreach (var dept in faculty.Departments)
                    {
                        string department = values[1].ToString().ToLower();
                        //Color color = (Color)values[0];
                        if (dept.Department.ToLower() == department /*&& color == (Color)Application.Current.Resources["AppColor"]*/)
                        {
                            return Color.FromHex(dept.Color);
                        }
                    }
                }
                /*if ((Color)value == (Color)Application.Current.Resources["AppColor"])
                {
                    return (Color)Application.Current.Resources["AppColor"];
                }*/
            }
            return Color.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
