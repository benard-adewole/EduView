using Estudiar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.CGPA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashBoard : ContentPage
    {
        public DashBoard()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            //sfPdfViewVM.GetStream();
            LoadPageAsync();
            base.OnAppearing();
        }
        private async void LoadPageAsync()
        {
            await Task.Delay(500);
            await MyLazyView.LoadViewAsync();
            MyLazyView.Content.BindingContext = new calcVm();
            (MyLazyView.Content as DashBoardCV).OnAppearing();
        }
    }
}