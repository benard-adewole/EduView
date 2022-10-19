using Estudiar.Helpers;
using Estudiar.Models;
using Estudiar.ViewModels;
using Estudiar.XInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;
using Syncfusion.Core;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLogin : ContentPage
    {
        VM_SignUp_In VM;
        private bool Animaring = false;
        public event EventHandler GotoSignUpClicked;
        public NewLogin()
        {
            VM = new VM_SignUp_In();//this.BindingContext as VM_SignUp_In;
            
            InitializeComponent();
            BindingContext = VM;
            VM.SignUpResponseReceived += VM_SignUpResponseReceived;
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
            }
            Xamarin.Forms.Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
        }


        private async void VM_SignUpResponseReceived(object sender, string e)
        {
            if (e == "No User Found!")
            {
                GotoSignUp_Tapped(null, null);
            }
            await DisplayAlert("Alert", e, "Ok");
        }
        private DateTime prev;
        private async void doubleTapToExit()
        {
            if (prev != null && (DateTime.Now - prev).TotalMilliseconds < 500)
            {
                //exit
                DependencyService.Get<ITerminate>().closeApp();
            }
            else
            {
                prev = DateTime.Now;
                DependencyService.Get<IToast>().LongToast("Doube tap to exit");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //doubleTapToExit();
            return false;
        }
        protected async override void OnAppearing()
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
        }
        
        private async void SignBtn_Tapped(object sender, EventArgs e)
        {

            
        }

        private async void GotoSignUp_Tapped(object sender, EventArgs e)
        {
            var page = new Views.NewRegister();
            NavigationPage.SetHasNavigationBar(page, false);
            await Shell.Current.GoToAsync($"{nameof(Views.NewRegister)}");
            //await Shell.Current.GoToAsync($"{nameof(MainPage)}");
            //await Shell.Current.Navigation.PushAsync(new MainPage());
            //CToggle.Button1Clicked.Execute(null);
        }

        private async void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            var page = new ForgotPassword();
            NavigationPage.SetHasNavigationBar(page, false);
            await Navigation.PushModalAsync(page);
            //await Shell.Current.GoToAsync($"{nameof(Views.ForgotPassword)}");
        }

        
    }
}