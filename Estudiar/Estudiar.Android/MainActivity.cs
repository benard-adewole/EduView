using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.OS;
using Estudiar.Views.CourseSample;
using Estudiar.Droid.Helpers;
using System.Linq;
using FFImageLoading.Forms.Platform;
using Estudiar.Models;
using System.Diagnostics;
using Estudiar.XInterface;
using System.IO;
using Xamarin.Essentials;
using Estudiar.Helpers;
using Xamarin.Forms;
using Estudiar.Droid;
using Google.Android.Material.Snackbar;
using System.Threading.Tasks;
using Xamarin.Forms.Platform.Android;
using Plugin.Fingerprint;
//using Xamarin.Forms;

[assembly: Dependency(typeof(Estudiar.Droid.Environment))]
[assembly: Dependency(typeof(Estudiar.Droid.MySnackBar))]
[assembly: Dependency(typeof(Estudiar.Droid.Exit))]
namespace Estudiar.Droid
{
    [Activity(Label = "EduView", RoundIcon = "@mipmap/icon", Icon = "@mipmap/icon", Exported =true, Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    /*[IntentFilter(
        new[] { Intent.ActionView, Intent.ActionEdit },
        Categories = new[]
        {
            Intent.CategoryDefault,
            Intent.CategoryBrowsable,
        },
        //DataScheme = "content",
        DataSchemes = new string[] { "content", "file" },
        //DataScheme = "file",
        DataHost = "*",
        DataMimeTypes = new string[] { "application/pdf" }
    )]*/
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public EventHandler<ActivityResultMessage> ActivityResultReady;
        // Instantiate an Information object for this page instance.
        ActivityResultMessage activityResult = new ActivityResultMessage();
        public static MainActivity GetMainActivity { get; private set; }

        protected override void OnDestroy()
        {

            //Preferences.Set("LoginResponse", "");
            base.OnDestroy();

        }

        protected override void OnStop()
        {

            //Xamarin.Forms.DependencyService.Get<IToast>().LongToast("ESTUDIAR: The scholars' companion");
            base.OnStop();
        }
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            GetMainActivity = this;


            //MobileAds.Initialize(ApplicationContext);
            //MobileAds.Initialize(ApplicationContext, "ca-app-pub-7656672211380452~7290171501");
            //MobileAds.Initialize(ApplicationContext, "ca-app-pub-7656672211380452~7569499723");

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            CrossFingerprint.SetCurrentActivityResolver(() => Xamarin.Essentials.Platform.CurrentActivity);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            CachedImageRenderer.Init(true);

            //XamEffects.Droid.Effects.Init();
            //Window.SetFlags(WindowManagerFlags.Secure, WindowManagerFlags.Secure);
            //LoadApplication(new App());

            var application = new App();

            string action = Intent.Action;
            string type = Intent.Type;

            if (Intent.ActionView.Equals(action) && !String.IsNullOrEmpty(type))
            {
                var incomingFile = new IncomingFile();
                Android.Net.Uri fileUri = Intent.Data;
                if (Intent.Data.Scheme.ToString() == "file")
                {
                    var status = await CheckAndStoragePermission();
                    if (status == PermissionStatus.Granted)
                    {
                        // Notify user permission was denied
                        incomingFile = new IncomingFile { Stream = ContentResolver.OpenInputStream(fileUri), fileName = "gotten" };

                    }
                    else
                    {
                        this.FinishAffinity();
                    }
                    //FileStream to_strem = new FileStream(fileUri.Path, FileMode.Open);
                    //incomingFile = new IncomingFile { Stream = to_strem, fileName = "gotten" };
                }
                else
                {
                    incomingFile = new IncomingFile { Stream = ContentResolver.OpenInputStream(fileUri), fileName = "gotten" };
                }


                application.IncomingFile = incomingFile;

                LoadApplication(application);
            }
            else
            {
                //Preferences.Set("LoginResponse", "");
                LoadApplication(application);
            }

        }
        public async Task<PermissionStatus> CheckAndStoragePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            if (status == PermissionStatus.Granted)
                return status;

            if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
            {
                // Prompt the user to turn on in settings
                // On iOS once a permission has been denied it may not be requested again from the application
                return status;
            }

            if (Permissions.ShouldShowRationale<Permissions.StorageRead>())
            {
                // Prompt the user with additional information as to why the permission is needed
            }

            status = await Permissions.RequestAsync<Permissions.StorageRead>();

            return status;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


            activityResult.RequestCode = requestCode;
            activityResult.ResultCode = resultCode;
            activityResult.MyIntent = data;
            EventHandler<ActivityResultMessage> handler = ActivityResultReady;
            if (handler != null)
                handler(this, activityResult);
        }



    }
    public class Exit:ITerminate
    {
        Activity GetActivity = Xamarin.Essentials.Platform.CurrentActivity;
        public void closeApp()
        {
            GetActivity.FinishAffinity();
            Java.Lang.JavaSystem.Exit(0);//.FinishAndRemoveTask();
        }
    }
    public class MySnackBar : ISnackBar
    {
        public void SnackbarShow(string str)
        {
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            Android.Views.View view = activity.FindViewById(Android.Resource.Id.Content);
            Snackbar snackbar = Snackbar.Make(view, str, Snackbar.LengthLong);
            snackbar.SetAction("OK", (v) =>
            {
                //v.Text = "";
                
            });
            snackbar.SetActionTextColor(Android.Resource.Color.White);
            snackbar.Show();

            //action button size

            //message size

        }
    }
    public class Environment : IEnvironment
    {
        Activity GetActivity = Xamarin.Essentials.Platform.CurrentActivity;
        private WindowManagerFlags _original;
        public void HideStatusBar()
        {
            
            /*var activity = Xamarin.Essentials.Platform.CurrentActivity;

            var attribute = activity.Window.Attributes;
            _original = attribute.Flags;
            attribute.Flags |= Android.Views.WindowManagerFlags.Fullscreen;
            activity.Window.Attributes = attribute;*/

        }

        public void setAccentColor()
        {
            //GetActivity.SetTheme(Resource.Style.DarkTheme);
        }

        public void ChangeTheme(string theme)
        {
            //Change the elements based on darkmode
            if (theme.ToLower() == "light")
            {
                // my custom theme which inherits from light there
                GetActivity.SetTheme(Resource.Style.LightTheme);
            }
            else
            {
                GetActivity.SetTheme(Resource.Style.DarkTheme);
            }
        }
       
        public void SetStatusBarColor(Xamarin.Forms.Color color, bool darkStatusBarTint, Xamarin.Forms.Color navBarColor)
        {
           
            if (Build.VERSION.SdkInt < Android.OS.BuildVersionCodes.Lollipop)
            {
                return;
            }

            try
            {
                var activity = Xamarin.Essentials.Platform.CurrentActivity;
                var window = activity.Window;
                window.AddFlags(Android.Views.WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(Android.Views.WindowManagerFlags.TranslucentStatus);

                window.SetStatusBarColor(((System.Drawing.Color)color).ToPlatformColor());
                window.SetNavigationBarColor(navBarColor.ToAndroid());

                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
                {
                    var flag = (Android.Views.StatusBarVisibility)Android.Views.SystemUiFlags.LightStatusBar;
                    window.DecorView.SystemUiVisibility = darkStatusBarTint ? flag : 0;
                    //
                }
                
                
            }
            catch
            {

            }
        }

        public void ShowStatusBar()
        {
            return;
            var activity = Xamarin.Essentials.Platform.CurrentActivity;
            var attribute = activity.Window.Attributes;
            attribute.Flags = _original;
            activity.Window.Attributes = attribute;
        }
    }
}