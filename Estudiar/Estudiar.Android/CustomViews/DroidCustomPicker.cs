using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Estudiar.Droid.CustomViews;
using Estudiar.CustomViews;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomPickerView), typeof(DroidCustomPicker))]

namespace Estudiar.Droid.CustomViews
{
    public class DroidCustomPicker:PickerRenderer
    {
        CustomPickerView picker = null;
        public DroidCustomPicker(Context context) : base(context)
        {
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (picker != null)
            {
                Control.SetHintTextColor(picker.PlaceholderColor.ToAndroid());
                if (e.PropertyName.Equals(CustomPickerView.PlaceholderProperty.PropertyName))
                {
                    UpdatePickerPlaceholder();
                }
            }
        }
        
        

        void UpdatePickerPlaceholder()
        {
            //CustomPickerView picker = Element as CustomPickerView;
            if (picker == null)
                picker = Element as CustomPickerView;
            if (picker.Placeholder != null)
            {
                Control.Hint = picker.Placeholder;
                Control.SetHintTextColor(picker.PlaceholderColor.ToAndroid());
            }
                
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                picker = Element as CustomPickerView;
                UpdatePickerPlaceholder();
                
                int[][] states = new int[][]
                    {
                        new int[] {-Android.Resource.Attribute.StateFocused}, //enabled
                        new int[] {Android.Resource.Attribute.StateFocused } //Disabled
                    };

                int[] stateColors = new int[] { picker.UnfocusedBorderColor.ToAndroid(), picker.FocusedBorderColor.ToAndroid() };
                ColorStateList stateList = new ColorStateList(states, stateColors);

                if (picker.ChangeBackgroundStyle)
                {
                    var gradientDrawable = new GradientDrawable();
                    gradientDrawable.SetCornerRadius(picker.CornerRadius);
                    gradientDrawable.SetStroke(picker.BorderWidth, stateList);
                    gradientDrawable.SetColor(picker.BackColor.ToAndroid());

                    Control.SetBackground(gradientDrawable);
                    //Control.TextSize = 40.0f;
                    Control.SetTextSize(Android.Util.ComplexUnitType.Sp, (float)picker.FontSize);
                    Control.SetPadding(
                        (int)picker.Padding.Left,
                        (int)picker.Padding.Top,
                        (int)picker.Padding.Right,
                        (int)picker.Padding.Bottom);
                }
                
                Control.SetMaxLines(1);
                Control.Hint = picker.Placeholder;
                Control.SetHintTextColor(picker.PlaceholderColor.ToAndroid());
                //Control.SetElevation(10f);
            }
        }
    }
}