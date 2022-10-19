using Android.Content;
using Estudiar.Droid;
using Xamarin.Forms;
using Estudiar.CustomViews;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using System.ComponentModel;
using Android.Content.Res;

[assembly: ExportRenderer(typeof(RoundedEntry), typeof(RoundedEntryRenderer))]

namespace Estudiar.Droid
{
    
    public class RoundedEntryRenderer : EntryRenderer
    {
        public RoundedEntryRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                RoundedEntry entry = Element as RoundedEntry;

                int[][] states = new int[][]
                    {
                        new int[] {-Android.Resource.Attribute.StateFocused}, //enabled
                        new int[] {Android.Resource.Attribute.StateFocused } //Disabled
                    };

                int[] stateColors = new int[]{ entry.UnfocusedBorderColor.ToAndroid(), entry.FocusedBorderColor.ToAndroid() };
                ColorStateList stateList = new ColorStateList(states,stateColors);

                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(entry.CornerRadius);
                gradientDrawable.SetStroke(entry.BorderWidth,stateList);
                gradientDrawable.SetColor(entry.BackColor.ToAndroid());

                Control.SetElevation(0f);
                Control.SetBackground(gradientDrawable);
                Control.SetPadding((int)entry.Padding.Left, (int)entry.Padding.Top, (int)entry.Padding.Right, (int)entry.Padding.Bottom);
            }
            
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
        }
    }
}