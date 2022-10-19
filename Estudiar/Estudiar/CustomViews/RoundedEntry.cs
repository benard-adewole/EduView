using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.CustomViews
{
    public class RoundedEntry:Entry
    {

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(
                propertyName: "BorderColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(
                propertyName: "Padding",
                returnType: typeof(Thickness),
                declaringType: typeof(RoundedEntry),
                defaultValue: new Thickness(0));
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(
                propertyName: "BorderWidth",
                returnType: typeof(int),
                declaringType: typeof(RoundedEntry),
                defaultValue: 0,
                BindingMode.TwoWay);
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                propertyName: "CornerRadius",
                returnType: typeof(float),
                declaringType: typeof(RoundedEntry),
                defaultValue: 0f);
        public static readonly BindableProperty BackColorProperty =
            BindableProperty.Create(
                propertyName: "BackColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty FocusedBorderColorProperty =
            BindableProperty.Create(
                propertyName: "FocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty UnfocusedBorderColorProperty =
            BindableProperty.Create(
                propertyName: "UnfocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(RoundedEntry),
                defaultValue: Color.Transparent);

        public Color FocusedBorderColor
        {
            get { return (Color)GetValue(FocusedBorderColorProperty); }
            set { SetValue(FocusedBorderColorProperty, value); }
        }
        public Color UnfocusedBorderColor
        {
            get { return (Color)GetValue(UnfocusedBorderColorProperty); }
            set { SetValue(UnfocusedBorderColorProperty, value); }
        }
        public Color BackColor 
        {
            get { return (Color)GetValue(BackColorProperty); }
            set { SetValue(BackColorProperty, value); }
        }
        public Thickness Padding 
        {
            get { return (Thickness)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        public int BorderWidth 
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }
        public float CornerRadius 
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        /*public Color BorderColor { get; set; }
        public Color BackColor { get; set; }
        public Thickness Padding { get; set; }
        public int BorderWidth { get; set; }
        public float CornerRadius { get; set; }*/
    }
}
