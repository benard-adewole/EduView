using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace Estudiar.Converters
{
    public class ItemTemplateToItemLayoutConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                //return new GridItemsLayout(1, ItemsLayoutOrientation.Vertical);
                ResourceDictionary keys = new ResourceDictionary();
                
                keys.TryGetValue("CourseListTemplate", out var data2);
                
                if (((DataTemplate)value) == (DataTemplate)data2)
                {
                    return new GridItemsLayout(2,ItemsLayoutOrientation.Vertical);
                }
                keys.TryGetValue("CourseGridTemplate", out data2);
                if ((DataTemplate)value == (DataTemplate)data2)
                {
                    return new GridItemsLayout(3, ItemsLayoutOrientation.Vertical);
                }
            }
            return new GridItemsLayout(1, ItemsLayoutOrientation.Vertical);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
