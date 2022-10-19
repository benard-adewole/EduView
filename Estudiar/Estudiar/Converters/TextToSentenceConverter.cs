using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;

namespace Estudiar.Converters
{
    public class TextToSentenceConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string text = "";
                int count = 0;
                foreach (var section in value.ToString().Trim())
                {
                    count++;
                    if (count == 1)
                    {
                        text += char.ToUpper(section);
                    }
                    else
                    {
                        text += char.ToLower(section);
                        if (section == ' ')
                        {
                            count = 0;
                        }
                    }
                    

                }
                return text;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
