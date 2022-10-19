using Estudiar.Models;
using Estudiar.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loading : Popup
    {
        LoadingVM LoadingVM;
        public Loading(LoadingVM vM)
        {
            InitializeComponent();
            BindingContext = vM;
            LoadingVM = BindingContext as LoadingVM;
            LoadingVM.DismissReceived += LoadingVM_DismissReceived;
        }

        private void LoadingVM_DismissReceived(object sender, string e)
        {
            Dismiss(null);
        }

        public Loading(IncomingFile incomingFile)
        {
            InitializeComponent();
            Device.BeginInvokeOnMainThread(async() => {await SaveAndLoadExtPdf(incomingFile); }); 
        }
        private async Task SaveAndLoadExtPdf(IncomingFile value)
        {
            Stream pdf = value.Stream;
            if (pdf != null)
            {
                using (var fileStream = File.Create(Path.Combine(FileSystem.AppDataDirectory, "Estudiar.pdf")))
                {
                    pdf.Seek(0, SeekOrigin.Begin);
                    await pdf.CopyToAsync(fileStream);
                }
            }
            

            await Navigation.PushAsync(new PDFViewer());
        }
        /*protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Application.Current.Properties.ContainsKey("ExtPdfUsed"))
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
            }
            
        }*/
    }
}