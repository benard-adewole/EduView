using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Estudiar.Controls
{
    public class CardViewInfo : ContentView
    {
        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create(
                propertyName: "Value",
                returnType: typeof(string),
                declaringType: typeof(CardViewInfo),
                defaultValue: string.Empty);
        public string Value
        {
            get =>  (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value); 
        }


        public static readonly BindableProperty DescriptionProperty =
            BindableProperty.Create(
                propertyName: "Description",
                returnType: typeof(string),
                declaringType: typeof(CardViewInfo),
                defaultValue: string.Empty);
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }
    }
}