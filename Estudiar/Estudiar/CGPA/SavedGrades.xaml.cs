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
    public partial class SavedGrades : ContentPage
    {
        public SavedGrades()
        {
            InitializeComponent();
            LoadPageAsync();
        }
        protected override void OnAppearing()
        {
            //sfPdfViewVM.GetStream();
            //LoadPageAsync();
            if (MyLazyView.Content !=  null && MyLazyView.Content.BindingContext as savedDataVm != null)
            {
                (MyLazyView.Content.BindingContext as savedDataVm).GetData();
            }
            
            base.OnAppearing();
        }
        private async void LoadPageAsync()
        {
            await Task.Delay(500);
            await MyLazyView.LoadViewAsync();
            MyLazyView.Content.BindingContext = new savedDataVm();
            //(MyLazyView.Content as DashBoardCV).OnAppearing();
        }
    }
}