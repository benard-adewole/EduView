﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Estudiar.XInterface;
using Xamarin.Essentials;

namespace Estudiar.Droid
{
    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true, NoHistory = true)]

    //[Activity(Label = "SplashActivity", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Preferences.Set("LoginResponse", "");
            //SetContentView(Resource.Layout.SplashUi);
            StartActivity(typeof(MainActivity));
            Finish();
            // Create your application here
        }
        
    }
}