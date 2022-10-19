using Estudiar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Estudiar.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PDFCustomView : ContentView
    {
        sfPdfViewVM sfPdfViewVM;

        public PDFCustomView()
        {
            InitializeComponent();
            sfPdfViewVM = (BindingContext as sfPdfViewVM);
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
            }
            //sfPdfViewVM.GetStream();
        }

        private async void ClosePage_Clicked(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync($"{nameof(Views.CourseInfo)}");

            pdfViewerControl1.Dispose();
            await Shell.Current.Navigation.PopAsync();
        }

        private void textSearchEntry_Completed(object sender, EventArgs e)
        {
            SearchbtnCommand.IsChecked = true;
        }

        public void disappearing()
        {
            pdfViewerControl1.Unload();
        }
    }
}