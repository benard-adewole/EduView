using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Estudiar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;
using Estudiar.Helpers;
using Estudiar.XInterface;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        MainVM mainVM;

        public Profile()
        {
            InitializeComponent();
            Shell.SetBackButtonBehavior(this, new BackButtonBehavior
            {
                Command = new Command(() =>
                {

                })
            }) ;
            mainVM = BindingContext as MainVM;
            mainVM.SaveIDClicked += MainVM_SaveIDClicked;
            switch (Settings.Theme)
            {
                case 0:
                    RBDefault.IsChecked = true;
                    break;
                case 1:
                    RBLight.IsChecked = true;
                    break;
                case 2:
                    RBDark.IsChecked = true;
                    break;
                default:
                    break;
            }
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
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
            doubleTapToExit();
            return true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Settings.ShowStatusBar();
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColorDark"], false);
            }
            else
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColor"], true);
            }
            BindingContext = new MainVM();
        }
        private async void MainVM_SaveIDClicked(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                DependencyService.Get<IToast>().LongToast("Platform not supported");
                return;
            }
            /*var imageStream = await IDcard.CaptureImageAsync();
            using (var fileStream = File.Create(Path.Combine(FileSystem.AppDataDirectory, "IDcard.png")))
            {

                imageStream.Seek(0, SeekOrigin.Begin);
                await imageStream.CopyToAsync(fileStream);
            }*/
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Show others your ID",
                File = new ShareFile(Path.Combine(FileSystem.AppDataDirectory, "IDcard.png"))
            }); ;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //App.Current.Resources[""] =  
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            switch (((RadioButton)sender).Value)
            {
                case "Default":
                    Settings.Theme = 0;
                    break;
                case "Light":
                    Settings.Theme = 1;
                    break;
                case "Dark":
                    Settings.Theme = 2;
                    break;
                default:
                    break;
            }
            TheTheme.setTheme();
        }

        private async void SaveName_Clicked(object sender, EventArgs e)
        {
            string newName = NameEntry.Text.ToLower().Trim();
            if ((sender as Button).Text.ToLower() == "save" && mainVM.LoginResponse.user.name.ToLower().Trim() != newName.ToLower().Trim())
            {
                if (string.IsNullOrEmpty(NameEntry.Text))
                {
                    await DisplayAlert("Field Missing", "Name is required", "Ok");
                    return;
                }
                string x = mainVM.LoginResponse.user.name;
                await mainVM.EditProfileAsync("name", newName);
                BindingContext = new MainVM();
            }
            
        }

        private async void SaveLevel_Clicked(object sender, EventArgs e)
        {
            string newLevel = LevelPicker.SelectedItem.ToString();
            if ((sender as Button).Text.ToLower() == "save" && mainVM.LoginResponse.user.year.ToString() != newLevel.ToLower().Trim())
            {
                string x = mainVM.LoginResponse.user.name;
                await mainVM.EditProfileAsync("year", newLevel.ToString());
                BindingContext = new MainVM();
            }
        }
        private void ReflectProfileUpdate()
        {

        }
    }
}