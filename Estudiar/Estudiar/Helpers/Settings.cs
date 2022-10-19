using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using System.Text;
//using System.Drawing;
using Xamarin.Forms;

namespace Estudiar.Helpers
{
    public interface IEnvironment
    {
        void SetStatusBarColor(Color color, bool darkStatusBarTint, Color navBarColor);
        void ShowStatusBar();
        void HideStatusBar();
        void ChangeTheme(string theme);
    }
    public static class TheTheme
    {
        public static void setTheme()
        {

            switch (Settings.Theme)
            { 
                //default
                case 0:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Unspecified;
                    break;

                //light
                case 1:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Light;
                    break;
                //dark
                case 2:
                    App.Current.UserAppTheme = Xamarin.Forms.OSAppTheme.Dark;

                    break;
                default:
                    break;
                
            }
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                //Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColorDark"], false);
                Settings.ChangeStatus(Color.Black, false);
                if (Device.RuntimePlatform == Device.Android)
                {
                    Xamarin.Forms.DependencyService.Get<IEnvironment>().ChangeTheme("dark");
                }

            }
            else
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColor"], true);
                if (Device.RuntimePlatform == Device.Android)
                {
                    Xamarin.Forms.DependencyService.Get<IEnvironment>().ChangeTheme("light");
                }
            }

        }

        
    }
    public static class Settings
    {
        //0 - default, 1 = light, 2 = dark
        const int theme = 0;
        public static int Theme 
        {
            get => Preferences.Get(nameof(Theme), theme);
            set => Preferences.Set(nameof(Theme), value);
        }
        public static void ChangeStatus(System.Drawing.Color newColor, bool darkBg)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Color navBarColor = (Color)Application.Current.Resources["BackgroundColor"];
                if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
                {
                    //Settings.ChangeStatus(System.Drawing.Color.Black, false);
                    navBarColor = Color.Black;
                }
                else
                {
                    //Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColor"], true);
                }

                
                Xamarin.Forms.DependencyService.Get<IEnvironment>().SetStatusBarColor(newColor, darkBg,navBarColor);
            }
        }
        public static void ShowStatusBar()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.DependencyService.Get<IEnvironment>().ShowStatusBar();
            }
        }
        public static void HideStatusBar()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.DependencyService.Get<IEnvironment>().HideStatusBar();
            }
        }
    }
}
