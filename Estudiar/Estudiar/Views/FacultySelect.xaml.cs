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
    public partial class FacultySelect : ContentPage
    {
        VM_SignUp_In vM_SignUp_In;
        
        public FacultySelect(VM_SignUp_In  vM)
        {
            InitializeComponent();
            //BindingContext = vM;
            BindingContext = vM;
            vM_SignUp_In = vM;
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
            //vM_SignUp_In.SelectedFac = new Models.Model_Fac_dept();
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
            //Settings.ChangeStatus((Color)Application.Current.Resources["AppColor"], false);
            base.OnAppearing();
            
        }
        

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //vM_SignUp_In.Faculty_Department
            //(
            if (vM_SignUp_In.Faculty_Department == null)
            {
                return;
            }
            foreach (var ClickedFaculty in vM_SignUp_In.Faculty_Department)
            {
                if (((Button)sender).CommandParameter.ToString().ToLower() == ClickedFaculty.Faculty.ToLower())
                {
                    vM_SignUp_In.SelectedFac = ClickedFaculty;
                    break;
                }
                
            }
        }

        private async void Next_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new DepartmentSelect(vM_SignUp_In));
            //await Shell.Current.GoToAsync($"{nameof(DepartmentSelect)}");
        }
    }
}