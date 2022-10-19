using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterCview : ContentView
    {
        public RegisterCview()
        {
            InitializeComponent();
        }
        private async void VM_SignUpResponseReceived(object sender, string e)
        {
            if (e == "user already exists!")
            {
                await Shell.Current.Navigation.PopAsync();
                await Shell.Current.DisplayAlert("Account Exists", "Please Login", "Ok");
                return;
            }
            if (e.Trim().ToLower() == "confirm registration from e-mail!")
            {
                await Shell.Current.Navigation.PopAsync();
                await Shell.Current.DisplayAlert("Success", "You Can Login", "Ok");
                return;
            }
            await Shell.Current.DisplayAlert("Alert", e, "Ok");
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