using Estudiar.Helpers;
using Estudiar.Models;
using Estudiar.Services;
using Estudiar.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
//using Unity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using Microsoft.AppCenter;
//using Microsoft.AppCenter.Analytics;
//using Microsoft.AppCenter.Crashes;

namespace Estudiar
{
    public partial class App : Application
    {
        private IncomingFile _incomingFile;
        public IncomingFile IncomingFile
        {
            get { return _incomingFile; }
            set
            {
                _incomingFile = value;
                //MainPage = new Views.PDFViewer();
                //MainPage = new NavigationPage(new Views.UploadPage());
                //MainPage = new NavigationPage(new Views.Loading(value));

            }
        }


        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NjQwNDA4QDMyMzAyZTMxMmUzMGdDMVRydGNMTC93V1JNdTZDTzlpS1drbXJyQlpDVS9ZR1NHenFYVWFMOUk9");


            InitializeComponent();



            if (this.IncomingFile != null)
            {
                return;
            }
            TheTheme.setTheme();
            //DependencyService.Register<MockDataStore>();
            //MainPage = new Estudiar.Views.Register_Login();

            MainPage = new Estudiar.Views.MainShell();
            return;
            if (Preferences.Get("OnBoard", "") == "")
            {
                MainPage = new Estudiar.Views.OnBoarding();
                return;
            }
            else
            {
                
                string response = Preferences.Get("LoginResponse", "");

                //with this if else, app alsways starts from login
                if (response != "")
                {
                    MainPage = new Estudiar.Views.MainShell();
                }
                else
                {
                    MainPage = new Estudiar.Views.AppShell();
                }
                
                return;
            }

            //return;
            var x = Preferences.Get("LoginResponse", "");
            

            Xamarin.Forms.Application.Current.MainPage = new Views.AppShell();
            (MainPage as Views.AppShell).setEntry();


            /*if (Preferences.Get("LoginResponse", "") != "" && y != $"\\{nameof(Views.LogOut)}" )
            {
                
            }
            else
            {
                MainPage = new NavigationPage(new Views.Register_Login());
            }*/
            //Preferences.Set("Resume", false);







            //var page = new Views.MainAppPage(new Models.LoginResponse { user = new Models.User { privileges = false } });
            //var page = new Views.Register_Login();
            //NavigationPage.SetHasNavigationBar(page, false);
            //MainPage = new NavigationPage(new Views.PDFViewer());
            //return;
            /*if (IncomingFile == null)
            {
                MainPage = page;
                //MainPage = new NavigationPage(page);
            }
            else
            {
                MainPage = new Views.PDFViewer();
                //MainPage = new NavigationPage(page);
                (MainPage as Views.PDFViewer).SetIncomingFile(IncomingFile);
            }*/

            //MainPage = new Views.Home(new MainVM() { LoginResponse = new Models.LoginResponse { user = new Models.User { privileges = false} } });
            //MainPage = new NavigationPage(new Views.PDFViewer(""));
            //MainPage = new Views.UploadPage(new ViewModels.MainVM());
            //MainPage =  new Views.MainAppPage(new Models.LoginResponse());
        }



        protected override void OnStart()
        {
            /*var page = new Views.AppShell();
            MainPage = page;*/

            Application.Current.Properties.Remove("ExtPdfUsed");
            Xamarin.Forms.Application.Current.Properties["OnStart"] = true;

            //MainPage = new NavigationPage(new Views.Register_Login());
        }

        protected override void OnSleep()
        {
            try
            {
                string temp = Preferences.Get("AppNavStackTemp", "");

                Preferences.Set("AppNavStack", temp);
                //Application.Current.Properties["AppNavStack"] = Application.Current.Properties["AppNavStackTemp"].ToString();
            }
            catch (Exception)
            {

            }
            TheTheme.setTheme();
            RequestedThemeChanged -= App_RequestedThemeChanged;
            //Application.Current.Properties["OnStart"] = false;
        }

        protected async override void OnResume()
        {
            return;

            Preferences.Set("Resume", true);
            string temp = Preferences.Get("AppNavStackTemp", "");
            string current = Preferences.Get("AppNavStack", "");
            //TheTheme.setTheme();
            
            RequestedThemeChanged += App_RequestedThemeChanged;
            var appNavStack = Preferences.Get("AppNavStack", "");
            if (appNavStack != "")
            {
                (MainPage as Estudiar.Views.AppShell).RestoreApp();
            }
        }

        private void App_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() => { TheTheme.setTheme(); });
        }
    }
}
