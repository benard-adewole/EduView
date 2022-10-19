using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Estudiar.Views.CourseSample;
using Estudiar.CustomViews;
using Estudiar.Droid.GeneralRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

//[assembly: ExportRenderer(typeof(NavigationPage), typeof(MyNavPageRenderer))]

namespace Estudiar.Droid.GeneralRenderers
{
    public class MyNavPageRenderer : NavigationPageRenderer
    {
        //private Android.Support.V7.Widget.Toolbar _toolbar;
        Context GetContext;
        public MyNavPageRenderer(Context context) : base(context)
        {
            GetContext = context;
            
        }
        /*protected override Task<bool> OnPopViewAsync(Page page, bool animated)
        {
            // check if the current item id 	
            // is equals to the back button id 	

            if (page is A_Course)
            {
                if (!(page as A_Course).IsTitleEntryClosed())
                {
                    (page as A_Course)?.CustomBackButtonAction.Invoke();
                    return Task.FromResult<bool>(false);
                }
            }
            return base.OnPopViewAsync(page, animated);
        }*/
        /*public override void OnViewAdded(Android.Views.View child)
        {
            base.OnViewAdded(child);

            if (child.GetType() == typeof(Android.Support.V7.Widget.Toolbar))
            {
                _toolbar = (Android.Support.V7.Widget.Toolbar)child;
                _toolbar.ChildViewAdded += Toolbar_ChildViewAdded;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
            }
        }
        private void Toolbar_ChildViewAdded(object sender, ChildViewAddedEventArgs e)
        {
            var view = e.Child.GetType();

            if (e.Child.GetType() == typeof(Android.Widget.TextView))
            {
                var textView = (Android.Widget.TextView)e.Child;
                var spaceFont = Typeface.CreateFromAsset(GetContext.ApplicationContext.Assets, "VollkornRegular.ttf");
                var systemFont = Typeface.Default;
                var systemBoldFont = Typeface.DefaultBold;
                textView.Typeface = spaceFont;
                _toolbar.ChildViewAdded -= Toolbar_ChildViewAdded;
            }
        }*/
    }
}