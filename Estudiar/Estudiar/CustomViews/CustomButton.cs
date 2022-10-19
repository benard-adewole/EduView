using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.CustomViews
{
    public class CustomButton:Button
    {
        public static readonly BindableProperty PressedColorProperty =
            BindableProperty.Create(
                propertyName: "PressedColor",
                returnType: typeof(Color),
                declaringType: typeof(CustomButton),
                defaultValue: Color.Transparent);
        

        public Color PressedColor
        {
            get { return (Color)GetValue(PressedColorProperty); }
            set { SetValue(PressedColorProperty, value); }
        }

        public static readonly BindableProperty ReleasedColorProperty =
            BindableProperty.Create(
                propertyName: "ReleasedColor",
                returnType: typeof(Color),
                declaringType: typeof(CustomButton),
                defaultValue: Color.Transparent);


        public Color ReleasedColor
        {
            get { return (Color)GetValue(ReleasedColorProperty); }
            set { SetValue(ReleasedColorProperty, value); }
        }

    }
}
