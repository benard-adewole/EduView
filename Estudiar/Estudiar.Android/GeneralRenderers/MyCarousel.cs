
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
using Xamarin.Forms;
using Estudiar.Droid.GeneralRenderers;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(Xamarin.Forms.CarouselView), typeof(MyCarousel))]
namespace Estudiar.Droid.GeneralRenderers
{
    public class MyCarousel:Xamarin.Forms.Platform.Android.CarouselViewRenderer
    {
        Context GetContext;
        public MyCarousel(Context context) : base(context)
        {
            GetContext = context;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<ItemsView> elementChangedEvent)
        {
            base.OnElementChanged(elementChangedEvent);
            OverScrollMode = OverScrollMode.Never;
        }
    }
}