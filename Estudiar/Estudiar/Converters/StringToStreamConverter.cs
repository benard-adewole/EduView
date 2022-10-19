using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using Xamarin.Forms;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Estudiar.Converters
{
    public class stringToStreamConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            
            return GetStream((string)value);
        }

        private async Task<Stream> GetStream(string url)
        {
            HttpClient httpClient = new HttpClient();
            var buff = await httpClient.GetByteArrayAsync(url);


            Stream image = new MemoryStream(buff);
            return image;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
