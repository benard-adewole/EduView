using Xamarin.Forms;
using Android.Widget;
using Android.Graphics;
using Estudiar.XInterface;

[assembly: Dependency(typeof(Estudiar.Droid.toastAndroid))]

namespace Estudiar.Droid
{
    public class toastAndroid:IToast
    {
        public void ShortToast(string message)
        {
            var toast = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short);
            /*Android.Graphics.Color c = Android.Graphics.Color.Black;
            ColorMatrixColorFilter CM = new ColorMatrixColorFilter(
                new float[] {
                    0,0,0,0f,c.R,
                    0,0,0,0f,c.G,
                    0,0,0,0f,c.B,
                    0,0,0,1f,0
                });
            toast.View.Background.SetColorFilter(CM);*/
            toast.Show();
        }
        public void LongToast(string message)
        {
            var toast = Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long);
            /*Android.Graphics.Color c = Android.Graphics.Color.Black;
            ColorMatrixColorFilter CM = new ColorMatrixColorFilter(
                new float[] {
                    0,0,0,0f,c.R,
                    0,0,0,0f,c.G,
                    0,0,0,0f,c.B,
                    0,0,0,1f,0
                });
            toast.View.Background.SetColorFilter(CM);*/
            toast.Show();
        }
    }
}