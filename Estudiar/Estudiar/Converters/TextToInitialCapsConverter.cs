using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using Estudiar.Models;

namespace Estudiar.Converters
{
    public class TextToInitialCapsConverter : IValueConverter
    {
        List<string> preposition = new List<string> { "TO","AND","IN","&","WITH"};

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string ancronym = "";
            if (value != null)
            {
                var listOfWord = value.ToString().Trim().ToUpper().Split(' ');

                foreach (var item in listOfWord)
                {
                    if (!preposition.Contains(item) && char.IsLetter(item[0]))
                    {
                        if (ancronym.Length == 2)
                        {
                            return ancronym;
                        }
                        ancronym += item[0];
                    }
                    
                }
                if (ancronym != "")
                {
                    return ancronym;
                }
                return value.ToString().ToUpper()[0];    
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
