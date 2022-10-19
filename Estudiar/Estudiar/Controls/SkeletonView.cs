using System;
using Xamarin.Forms;

namespace Estudiar.Controls
{
    public class SkeletonView : BoxView
    {
        public static readonly BindableProperty LengthProperty =
            BindableProperty.Create(
                propertyName: "Length",
                returnType: typeof(int),
                declaringType: typeof(SkeletonView),
                defaultValue: 750);


        public int Length
        {
            get { return (int)GetValue(LengthProperty); }
            set { SetValue(LengthProperty, value); }
        }
        public SkeletonView()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1.5), () =>
            {
                this.FadeTo(0.5, (uint)Length, Easing.CubicInOut).ContinueWith((x) =>
                {
                    this.FadeTo(1, (uint)Length, Easing.CubicInOut);
                });

                return true;
            });
        }
    }
}
