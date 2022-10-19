using Estudiar.Helpers;
using Estudiar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRegister : ContentPage
    {
        VM_SignUp_In VM;
        public NewRegister()
        {
            InitializeComponent();
            InitializeView();
            VM = new VM_SignUp_In();//this.BindingContext as VM_SignUp_In;
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
            if (e == "user already exists!")
            {
                await Shell.Current.Navigation.PopAsync();
                await DisplayAlert("Account Exists", "Please Login", "Ok");
                return;
            }
            if (e.Trim().ToLower() == "confirm registration from e-mail!")
            {
                await Shell.Current.Navigation.PopAsync();
                await DisplayAlert("Success", "You Can Login", "Ok");
                return;
            }
            await DisplayAlert("Alert", e, "Ok");
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
        private async void GotoSignIn_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            //CToggle.Button2Clicked.Execute(null);
        }
        private void InitializeView()
        {
            

        }
        private async void RepasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            WrongPassword();
        }
        private async Task<bool> WrongPassword()
        {
            if (PasswordEntry.Text != RepasswordEntry.Text && !String.IsNullOrEmpty(RepasswordEntry.Text))
            {
                PasswordEntry.TextColor = Color.Red;
                RepasswordEntry.TextColor = Color.Red;
                Task t = MismatchLabel.FadeTo(1);
                await MismatchLabel.TranslateTo(30, 0);
                await MismatchLabel.TranslateTo(0, 0);
                //Vibration.Vibrate(100);
                //await Task.Delay(50);
                //Vibration.Vibrate(100);
                return true;
            }
            else
            {
                Task t = MismatchLabel.FadeTo(0);
                if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
                {
                    PasswordEntry.TextColor = Color.White;
                    RepasswordEntry.TextColor = Color.White;
                }
                else
                {
                    PasswordEntry.TextColor = Color.Black;
                    RepasswordEntry.TextColor = Color.Black;
                }
                
                return false;
            }
        }
        private void PasswordEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            WrongPassword();
        }
    }
}