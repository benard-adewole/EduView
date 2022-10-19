using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
//using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Estudiar.CustomViews;
using Estudiar.Droid.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(AndroidFrameRenderer))]
namespace Estudiar.Droid.CustomViews
{
    public class AndroidFrameRenderer: Xamarin.Forms.Platform.Android.AppCompat.FrameRenderer
    {
        Context GetContext;
        public AndroidFrameRenderer(Context context):base(context)
        {
            GetContext = context;
        }
        private Color backgroundColor;

        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                backgroundColor = Element.BackgroundColor;
                //Control.Touch += Control_Touch;

                //ViewGroup.SetBackgroundResource(Resource.Drawable.Shadow);
                //ViewGroup.Elevation = 10f;
                //Control.SetBackgroundResource(Resource.Drawable.Shadow);
                //Control.Background = GetContext.GetDrawable(Resource.Drawable.RippleEffect);
                if ((Element as CustomFrame).Login)
                {
                    Control.SetBackgroundResource(Resource.Drawable.LoginShadow);
                    Control.Elevation = 15f;
                }
                else if ((Element as CustomFrame).HasShadow)
                {
                    //Control.SetBackgroundResource(Resource.Drawable.Shadow);
                    Control.Elevation = 10f;
                }
                else if ((Element as CustomFrame).HasRipple)
                {
                    //Control.Background = GetContext.GetDrawable(Resource.Drawable.RippleEffect);

                    TypedValue typedValue = new TypedValue();
                    GetContext.Theme.ResolveAttribute(Android.Resource.Attribute.SelectableItemBackground, typedValue, true);
                    //Control.SetBackgroundResource(Resource.Drawable.RippleEffect);
                    Control.SetBackgroundResource(typedValue.ResourceId);
                    Control.SetFocusable((Element as CustomFrame), true);
                    //Control.Background = GetContext.GetDrawable(Resource.Drawable.RippleEffect);
                }
                //Control.SetBackgroundResource(Resource.Drawable.Shadow);
                //Control.Elevation = 10f;
                //Control.Background = GetContext.GetDrawable(Resource.Drawable.RippleEffect);
                //Control.Background = new RippleDrawable()
                
                //Control.SetFocusable((Element as CustomFrame),true);


            }
            //ViewGroup.TranslationZ = 200f;

            //int[] attrs = new int[] { Resource.Attribute.selectableItemBackground };
            //Control.Background = GetContext.ObtainStyledAttributes();
            /*int[][] stateList = new int[][]{
                new int[]{Android.Resource.Attribute.StatePressed},
                new int[]{ Android.Resource.Attribute.StateFocused },
                new int[]{ Android.Resource.Attribute.StateActivated },
                new int[]{}
            };
            //deep blue
            int normalColor = Android.Graphics.Color.ParseColor("#303F9F");
            //red
            int pressedColor = Android.Graphics.Color.ParseColor("#FF4081");
            int[] stateColorList = new int[]{
                pressedColor,
                pressedColor,
                pressedColor,
                normalColor
            };
            ColorStateList colorStateList = new ColorStateList(stateList, stateColorList);
            RippleDrawable Ripple = new RippleDrawable(colorStateList, null, null);
            */
            //SetBackground(Element as CustomFrame);
            //Control.SetBackground(Ripple);
        }


        private void SetBackground(Android.Views.View rootLayout)
        {
            // Get the background color from Forms element
            var backgroundColor = Element.BackgroundColor.ToAndroid();

            // Create statelist to handle ripple effect
            var enabledBackground = new GradientDrawable(GradientDrawable.Orientation.LeftRight, new int[] { backgroundColor, backgroundColor });
            var stateList = new StateListDrawable();
            var rippleItem = new RippleDrawable(ColorStateList.ValueOf(Android.Graphics.Color.Red),
                                                enabledBackground,
                                                null);
            stateList.AddState(new[] { Android.Resource.Attribute.StateEnabled }, rippleItem);

            // Assign background
            rootLayout.Background = stateList;
        }


        private void Control_Touch(object sender, TouchEventArgs e)
        {
            switch (e.Event.Action)
            {
                case MotionEventActions.Down:
                    Element.BackgroundColor = Color.Green;
                    break;
                case MotionEventActions.Up:
                    Element.BackgroundColor = backgroundColor;
                    break;
            }
        }
    }
}