using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Estudiar.RestClients;
using Estudiar.XInterface;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Estudiar.Helpers;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        private bool newPasswordFieldIsvisible;

        public bool NewPasswordFieldIsvisible
        {
            get { return newPasswordFieldIsvisible; }
            set 
            {
                newPasswordFieldIsvisible = value;
                if ((bool)value)
                {
                    ShowNewPasswordField();
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async() => 
                    {
                        await NewPasswordFieldContainer.FadeTo(0, 300);
                    });
                    
                }
                
                OnPropertyChanged();
            }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set 
            {
                email = value;
                Xamarin.Forms.Application.Current.Properties["ForgotPasswordEmail"] = email.ToLower().Trim();
                OnPropertyChanged(); }
        }

        private string token;

        public string Token
        {
            get { return token; }
            set { token = value;OnPropertyChanged(); }
        }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value;OnPropertyChanged(); }
        }

        public ICommand VerifyTokenClicked
        {
            get
            {
                return new Command(() => 
                {
                    ValidateTokenAsync();
                });
            }
            private set { }
        }
        public ICommand ContinueClicked
        {
            get
            {
                return new Command(() =>
                {
                    SendForgotPass();
                });
            }
            private set { }
        }
        public ICommand CloseThisPage
        {
            get
            {
                return new Command(() =>
                {
                    Xamarin.Forms.Application.Current.Properties.Remove("ForgotPasswordEmail");
                    //Device.BeginInvokeOnMainThread(async () => { await Shell.Current.GoToAsync(".."); });
                    Device.BeginInvokeOnMainThread(async () => { await Shell.Current.Navigation.PopAsync(); });
                });
            }
            private set { }
        }
        public ICommand CloseChangePasswordView
        {
            get
            {
                return new Command(() =>
                {
                    NewPasswordFieldIsvisible = false;
                });
            }
            private set { }
        }
        public ICommand ChangePassword
        {
            get
            {
                return new Command(() =>
                {
                    ResetPasswordAsync();
                });
            }
            private set { }
        }
        public ForgotPassword()
        {
            BindingContext = this;
            InitializeComponent();
            newPasswordFieldIsvisible = true;

            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("ForgotPasswordEmail"))
            {
                Email = Xamarin.Forms.Application.Current.Properties["ForgotPasswordEmail"].ToString();
                AnimateValidateView();
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
            }
        }
        private async void ShowNewPasswordField()
        {
            NewPasswordFieldContainer.Opacity = 0;
            OnPropertyChanged();
            await NewPasswordFieldContainer.FadeTo(1, 300);
        }
        private async Task CloseNewPasswordField()
        {
            await NewPasswordFieldContainer.FadeTo(0, 500);
            //OnPropertyChanged();
        }
        private async Task SendForgotPass()
        {
            if ( Email == null || Email.Length == 0)
            {
                DependencyService.Get<IToast>().ShortToast("Please enter your email");
                return;
            }
            RestClient<String> restClient = new RestClient<string>();
            string x = await restClient.SendForgotPassAsync(Email.Trim());

            if (x == "Check email for reset link!")
            {
                AnimateValidateView();
                await Task.Delay(1000);
                await DisplayAlert("", x, "Ok");
            }
            else
            {
                DependencyService.Get<IToast>().ShortToast(x);
            }

            
        }
        private void AnimateValidateView()
        {
            VerifyContainer.IsVisible = true;
            List<Task> tasks = new List<Task>();
            tasks.Add(VerifyContainer.ScaleTo(1));
            tasks.Add(VerifyContainer.FadeTo(1));
            tasks.Add(EmailContinueContainer.FadeTo(0));
            tasks.Add(EmailContinueContainer.ScaleTo(0));
            //await Task.WhenAll(tasks);
        }
        protected override void OnAppearing()
        {
            Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["AppColor"], false);
            base.OnAppearing();

        }
        private async Task ValidateTokenAsync()
        {
            RestClient<bool> restClient = new RestClient<bool>();
            bool result = await restClient.SendValidateTokenAsync(token.Trim(),Email.ToLower().Trim());

            if (result)
            {
                NewPasswordFieldIsvisible = true;
            }
            else
            {
                DependencyService.Get<IToast>().ShortToast("Token is not valid");
            }
        }

        private async Task ResetPasswordAsync()
        {
            RestClient<String> restClient = new RestClient<string>();
            string result = await restClient.ResetPasswordAsync(token.Trim(), Email.ToLower().Trim(), Password);
            CloseChangePasswordView.Execute(null);
            await DisplayAlert("", result, "Ok");
            //close the view
           
        }

        private void ForgotEmailEntry_Completed(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            SendForgotPass();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void ForgotTokenEntry_Completed(object sender, EventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ValidateTokenAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
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
                Task t = MismatchNotif.FadeTo(1);
                //Vibration.Vibrate(100);
                //await Task.Delay(50);
                //Vibration.Vibrate(100);
                return true;
            }
            else
            {
                Task t= MismatchNotif.FadeTo(0);
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