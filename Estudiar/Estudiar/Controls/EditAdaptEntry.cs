using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.Controls
{
    public class EditAdaptEntry:ContentView
    {
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                propertyName: "Value",
                returnType: typeof(string),
                declaringType: typeof(EditAdaptEntry),
                defaultValue: string.Empty,
                defaultBindingMode: BindingMode.TwoWay);
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }


        public static readonly BindableProperty HeaderProperty =
            BindableProperty.Create(
                propertyName: "Header",
                returnType: typeof(string),
                declaringType: typeof(EditAdaptEntry),
                defaultValue: string.Empty);
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                propertyName: "Icon",
                returnType: typeof(string),
                declaringType: typeof(EditAdaptEntry),
                defaultValue: string.Empty);
        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly BindableProperty IsEditableProperty =
            BindableProperty.Create(
                propertyName: "IsEditable",
                returnType: typeof(bool),
                declaringType: typeof(EditAdaptEntry),
                defaultValue: false);
        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        public static readonly BindableProperty IsPasswordProperty =
            BindableProperty.Create(
                propertyName: "IsPassword",
                returnType: typeof(bool),
                declaringType: typeof(EditAdaptEntry),
                defaultValue: false);
        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

    }
}
