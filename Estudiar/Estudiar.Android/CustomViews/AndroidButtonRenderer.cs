using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using Estudiar.CustomViews;
using Estudiar.Droid.CustomViews;
using Android.Graphics.Drawables;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(CustomButton), typeof(AndroidButtonRenderer))]
namespace Estudiar.Droid.CustomViews
{
    public class AndroidButtonRenderer:ButtonRenderer
    {
        Context GetContext; 
        public AndroidButtonRenderer(Context context) : base(context)
        {
            GetContext = context;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                CustomButton button = Element as CustomButton;

                int[][] states = new int[][]
                    {
                        new int[] {-Android.Resource.Attribute.StatePressed}, //enabled
                        new int[] {Android.Resource.Attribute.StatePressed } //Disabled
                    };

                //var gradientDrawable = new GradientDrawable();
                //gradientDrawable.SetCornerRadius(entry.CornerRadius);
                //gradientDrawable.SetStroke(entry.BorderWidth, stateList);
                //gradientDrawable.SetColor(Xamarin.Forms.Color.Red.ToAndroid());

                //int[] attrs = new int[] {Resource.Attribute.selectableItemBackground}
                int[] stateColors = new int[] { button.ReleasedColor.ToAndroid(),button.PressedColor.ToAndroid() };
                ColorStateList stateList = new ColorStateList(states, stateColors);
                Control.Elevation = 200f;
                
                Control.Background = GetContext.GetDrawable(Resource.Drawable.btn_rounded);
                Control.BackgroundTintMode = PorterDuff.Mode.SrcAtop;
                Control.BackgroundTintList = stateList;
                //Control.Background = GetContext.ObtainStyledAttributes(Resource.Attribute.selectableItemBackground);

                //Control.SetOutlineAmbientShadowColor(Xamarin.Forms.Color.Green.ToAndroid());
                //Control.SetOutlineSpotShadowColor(Xamarin.Forms.Color.Red.ToAndroid());
                //Control.SetBackground(new RippleDrawable(stateList, gradientDrawable,gradientDrawable));
                Control.SetPadding(0,0,0,0);

                
            }
        }
    }
}