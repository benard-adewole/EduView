using Estudiar.Helpers;
using Estudiar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DepartmentSelect : ContentPage
    {
        VM_SignUp_In vM_SignUp_In;
        
        public DepartmentSelect(VM_SignUp_In vM)
        {
            InitializeComponent();
            BindingContext = vM;
            vM_SignUp_In = vM;
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
        }
        protected override void OnAppearing()
        {
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["BackgroundColorDark"], false);
            }
            else
            {
                Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["BackgroundColor"], true);
            }
            base.OnAppearing();
            //Settings.ChangeStatus((Color)Application.Current.Resources["AppColor"], false);
            
        }
        
        private async void Button_Clicked(object sender, EventArgs e)
        {
            //vM_SignUp_In.Faculty_Department
            //(
            if (vM_SignUp_In.SelectedFac.Departments == null)
            {
                return;
            }
            foreach (var ClickedDepartment in vM_SignUp_In.SelectedFac.Departments)
            {
                if (((Button)sender).CommandParameter.ToString().ToLower() == ClickedDepartment.Department.ToLower())
                {
                    vM_SignUp_In.SelectedDept = ClickedDepartment;
                    
                    break;
                }

            }
        }
        private async void Next_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"../..");
        }
    }
}