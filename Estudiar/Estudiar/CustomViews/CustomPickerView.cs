using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.CustomViews
{
    public class CustomPickerView:Picker
    {
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(string),
        defaultValue: string.Empty);

       
        public static readonly BindableProperty ChangeBackgroundStyleProperty =
            BindableProperty.Create(
                propertyName: "ChangeBackgroundStyle",
                returnType: typeof(bool),
                declaringType: typeof(CustomPickerView),
                defaultValue: true);
        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(
                propertyName: "PlaceholderColor",
                returnType: typeof(Color),
                declaringType: typeof(CustomPickerView),
                defaultValue: Color.Gray);
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(
                propertyName: "BorderColor",
                returnType: typeof(Color),
                declaringType: typeof(CustomPickerView),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(
                propertyName: "Padding",
                returnType: typeof(Thickness),
                declaringType: typeof(CustomPickerView),
                defaultValue: new Thickness(0));
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(
                propertyName: "BorderWidth",
                returnType: typeof(int),
                declaringType: typeof(CustomPickerView),
                defaultValue: 0);
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                propertyName: "CornerRadius",
                returnType: typeof(float),
                declaringType: typeof(CustomPickerView),
                defaultValue: 0f);
        public static readonly BindableProperty BackColorProperty =
            BindableProperty.Create(
                propertyName: "BackColor",
                returnType: typeof(Color),
                declaringType: typeof(CustomPickerView),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty FocusedBorderColorProperty =
            BindableProperty.Create(
                propertyName: "FocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(CustomPickerView),
                defaultValue: Color.Transparent);
        public static readonly BindableProperty UnfocusedBorderColorProperty =
            BindableProperty.Create(
                propertyName: "UnfocusedBorderColor",
                returnType: typeof(Color),
                declaringType: typeof(CustomPickerView),
                defaultValue: Color.Transparent);
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
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
        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
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
        public bool ChangeBackgroundStyle
        {
            get { return (bool)GetValue(ChangeBackgroundStyleProperty); }
            set { SetValue(ChangeBackgroundStyleProperty, value); }
        }
    }
}
