using Estudiar.XInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfReader : ContentView
    {
        public PdfReader(string url)
        {
            InitializeComponent();
            OpenPdf(url);
        }
        private async void OpenPdf(string url)
        {
            string localPath = await DependencyService.Get<FilePickCaller>().GetFilePath();
            //await DisplayAlert("", localPath, "Ok");

            if (Device.RuntimePlatform == Device.Android)
            {

                //MyPDFView.Source = $"file:///android_asset/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(localPath)}";
                //MyPDFView.Source = $"http://www.mdpi.com/2076-0760/6/4/126/pdf&ved=2ahUKEwjv-I7j1bDwAhUsSBUIHUdSCO8QFjALegQIEBAC&usg=AOvVaw3EmLog_CBfvdNWF44ait-n";
                MyPDFView.Source = "http://www.google.com";
                MyPDFView.BackgroundColor = Color.Purple;
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                //Assets / Fonts / Vollkorn - Regular.ttf#Vollkorn
                MyPDFView.Source = $"http://www.mdpi.com/2076-0760/6/4/126/pdf&ved=2ahUKEwjv-I7j1bDwAhUsSBUIHUdSCO8QFjALegQIEBAC&usg=AOvVaw3EmLog_CBfvdNWF44ait-n";

                //MyPDFView.Source = $"ms-appx-web:///Assets/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(localPath)}";
            }
            else
            {
                MyPDFView.Source = url;
            }
        }
    }
}