using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Estudiar.XInterface;
using System.IO;
using Estudiar.RestClients;
using Xamarin.Essentials;
using Estudiar.ViewModels;
using Estudiar.Models;
using Estudiar.Views.CourseSample;
using System.Windows.Input;
using Estudiar.Helpers;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PDFViewer : ContentPage
    {
        MainVM mainVM;
        private bool extPdf;

        public bool ExtPdf
        {
            get { return extPdf; }
            set { extPdf = value; }
        }

        
        public PDFViewer()
        {
            //BindingContext = this;
            InitializeComponent();
            Preferences.Set("ExtPdfUsed", true);
            //Application.Current.Properties[] = true;
            LoadPdf();
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Settings.ChangeStatus((Color)Application.Current.Resources["AppColor"], false);

        }
        
        public PDFViewer(MainVM VM)
        {
            InitializeComponent();
            structureNavStack();
            BindingContext = VM;
            mainVM = VM;
            if (mainVM != null)
            {
                OpenPdf();
            }
            
        }
        
        private async void structureNavStack()
        {
            
        }
        protected override bool OnBackButtonPressed()
        {
            if (Application.Current.MainPage is NavigationPage)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                        break;
                    case Device.UWP:
                        //Xamarin.Forms.Application.Current.MainPage = null;
                        App.Current.MainPage = null;
                        //Xamarin.Forms.Application.Current.MainPage = null;
                        break;
                    default:
                        break;
                }
            }
            return base.OnBackButtonPressed();
        }
        /*protected override void OnAppearing()
        {
            base.OnAppearing();
        }*/
        internal void SetIncomingFile(IncomingFile value)
        {
            if (value == null)
            {
                LoadPdf();
                return;
            }
            saveAndLoad(value.Stream);
        }

        /*protected override bool OnBackButtonPressed()
        {
            
            Application.Current.Properties.Remove("SelectedMaterial");
            return base.OnBackButtonPressed();
        }*/

        private async void OpenPdf()
        {
            //Last opened pdf, before app minimizes
            //if (Application.Current.Properties.ContainsKey("SelectedMaterial"))
            if (Preferences.Get("SelectedMaterial","") != "")
            {
                LoadPdf();
                return;
            }

            string token = mainVM.LoginResponse.token;
            string code = mainVM.SelectedCourse.code;
            string name = mainVM.SelectedMaterial.name;
            RestClient<Stream> restClients = new RestClient<Stream>();
            Stream pdf = await restClients.GetMaterialStreamAsync(token, code, name);


            //sApplication.Current.Properties["pdf"] = pdf;

            saveAndLoad(pdf);
        }

        private async void saveAndLoad(Stream pdf)
        {
            if (pdf == null)
            {
                Close_Clicked(null, null);
                return;
            }

            using (var fileStream = File.Create(Path.Combine(FileSystem.AppDataDirectory, "Estudiar.pdf")))
            {

                pdf.Seek(0, SeekOrigin.Begin);
                await pdf.CopyToAsync(fileStream);
            }
            LoadPdf();
            AppState<Material>.SerializeAndSave(mainVM.SelectedMaterial, "SelectedMaterial");


        }
        private async void LoadPdf()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                MyPDFView.Source = $"file:///android_asset/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(Path.Combine(FileSystem.AppDataDirectory, "Estudiar.pdf"))}";
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                var path = Path.Combine(FileSystem.AppDataDirectory, "Estudiar.pdf");
                //string sFile = string.Format("ms-appx-web://{0}/{1}", pdfView.Uri.Replace(pdfView.FileName, ""), WebUtility.UrlEncode(pdfView.FileName));
                //MyPDFView.Source = new Uri(string.Format("ms-appx-web:///Assets/pdfjs/web/viewer.html?file={0}", path));

                MyPDFView.Source = $"ms-appx-web:///Assets/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(path)}";
                //MyPDFView.Source = $"ms-appdata:///localcache/cacheSubfolder/pdfjs/web/viewer.html?file={WebUtility.UrlEncode(path)}";
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                MyPDFView.Source = Path.Combine(FileSystem.AppDataDirectory, "Estudiar.pdf");
            }
            
        }
        

        private async void Close_Clicked(object sender, EventArgs e)
        {
            if ((Application.Current.MainPage as MasterDetailPage).Detail.Navigation.NavigationStack.Count == 1)
            {
                switch (Device.RuntimePlatform)
                {
                    case Device.Android:
                        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                        break;
                    case Device.UWP:
                        //Xamarin.Forms.Application.Current.MainPage = null;
                        App.Current.MainPage = null;
                        //Xamarin.Forms.Application.Current.MainPage = null;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PopAsync(true);
            }
            
        }
    }
}