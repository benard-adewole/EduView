using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Estudiar.Droid.CustomViews;   
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Label), typeof(MyLabelRenderer))]
namespace Estudiar.Droid.CustomViews
{
    public class MyLabelRenderer:LabelRenderer
    {
        public MyLabelRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            Control?.SetIncludeFontPadding(false);
        }
    }
}