using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Estudiar.CustomViews
{
    public class CustomFrame: Frame
    {
        public static readonly BindableProperty HasRippleProperty =
            BindableProperty.Create(
                propertyName: "HasRipple",
                returnType: typeof(bool),
                declaringType: typeof(CustomFrame),
                defaultValue: false);
        public static readonly BindableProperty LoginProperty =
            BindableProperty.Create(
                propertyName: "Login",
                returnType: typeof(bool),
                declaringType: typeof(CustomFrame),
                defaultValue: false);

        public bool Login
        {
            get { return (bool)GetValue(LoginProperty); }
            set { SetValue(LoginProperty, value); }
        }
        public bool HasRipple
        {
            get { return (bool)GetValue(HasRippleProperty); }
            set { SetValue(HasRippleProperty, value); }
        }

    }
}
