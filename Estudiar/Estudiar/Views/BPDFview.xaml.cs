using Estudiar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudiar.CustomViews;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BPDFview : ContentPage
    {
        //sfPdfViewVM sfPdfViewVM;
        public BPDFview()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
            }
            //sfPdfViewVM = (BindingContext as sfPdfViewVM);
        }
        protected override void OnAppearing()
        {
            //sfPdfViewVM.GetStream();
            LoadPageAsync();
            base.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            //(MyLazyView.Content as PDFCustomView).disappearing();
            base.OnDisappearing();  
        }
        private async void LoadPageAsync()
        {
            await Task.Delay(500);
            await MyLazyView.LoadViewAsync();
            MyLazyView.Content.BindingContext = new sfPdfViewVM();
            await (MyLazyView.Content.BindingContext as sfPdfViewVM).GetStream();
        }
    }
}