using Estudiar.Models;
using Estudiar.XInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Estudiar.Helpers;
using Estudiar.ViewModels;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    //[assembly: ExportFont("FontAwesome5Regular.otf", Alias = "FontAwesome5Regular")]
    public partial class AppShell : Shell
    {
        private List<String> masterCarouselImages;

        public List<String> MasterCarouselImages
        {
            get { return masterCarouselImages; }
            set { masterCarouselImages = value; }
        }
        private int position;

        public int Position
        {
            get { return position; }
            set { position = value; OnPropertyChanged(); }
        }
        private bool flyoutPresented;

        public bool FlyOutPresented
        {
            get { return flyoutPresented; }
            set
            {
                flyoutPresented = value;
                if (value)
                {
                    Position = (Position + 1) % MasterCarouselImages.Count;
                }
            }
        }
        /*public ICommand LogOutCommand
        {
            get
            {
                return new Command(() =>
                {
                    Preferences.Set("LoginResponse", "");
                    Application.Current.MainPage = new NavigationPage(new Views.Register_Login());
                });
            }
            private set { }
        }*/
        public ICommand ExitCommand
        {
            get
            {
                return new Command(() =>
                {
                    switch (Device.RuntimePlatform)
                    {
                        case Device.Android:
                            Preferences.Set("LoginResponse", "");
                            System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                            break;
                        case Device.UWP:
                            //Xamarin.Forms.Application.Current.MainPage = null;
                            App.Current.MainPage = null;
                            //Xamarin.Forms.Application.Current.MainPage = null;
                            break;
                        default:
                            break;
                    }
                });
            }
            private set { }
        }
        public AppShell()
        {
            BindingContext = this;
            MasterCarouselImages = new List<string> { "study.png", "study1.png", "study2.png", "study3.png", "study5.png" };
            InitializeComponent();
            Preferences.Set("OnBoard", "OnBoard");
            
            
           
            Routing.RegisterRoute(nameof(Views.FacultySelect), typeof(Views.FacultySelect));
            Routing.RegisterRoute(nameof(Views.DepartmentSelect), typeof(Views.DepartmentSelect));
            Routing.RegisterRoute(nameof(Views.ForgotPassword), typeof(Views.ForgotPassword));
            //Routing.RegisterRoute(nameof(Views.Courses), typeof(Views.Courses));
            //Routing.RegisterRoute(nameof(Views.NewLogin), typeof(Views.NewLogin));
            Routing.RegisterRoute(nameof(Views.NewRegister), typeof(Views.NewRegister));
            
            //Routing.RegisterRoute($"{nameof(Views.CustomTab)}", typeof(CustomTab));
            //DependencyService.Get<ISnackBar>().SnackbarShow("snack");
            
        }
        
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            
        }
        public async void setEntry()
        {
            try
            {
                //await Shell.Current.GoToAsync($"{nameof(Views.NewLogin)}");
                await Shell.Current.GoToAsync($"//{nameof(Views.CustomTab)}");
                //await Shell.Current.GoToAsync($"{nameof(Views.Courses)}");
            }
            catch (Exception)
            {
                //could not load last saved page
            }
        }
        public async void RestoreApp()
        {
            try
            {
                string current = Preferences.Get("AppNavStack", "");
                await Shell.Current.GoToAsync(current);
            }
            catch (Exception)
            {
                //could not load last saved page
            }
        }
        protected async override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            if (args != null && args.Current != null && args.Current.Location != null)
            {
                Preferences.Set("Shell", "Login");
                Preferences.Set("AppNavStackTemp", args.Current.Location.OriginalString);
                if (args.Current.Location.OriginalString != $"//{nameof(Views.Courses)}")
                {
                    Preferences.Set("Resume", false);
                }

            }
        }
        protected async override void OnNavigating(ShellNavigatingEventArgs args)
        {
            //await Task.Delay(300);
            base.OnNavigating(args);
            
        }
        
    }
}