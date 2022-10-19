using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace Estudiar.Droid.Helpers
{
    public class ActivityResultListener
    {
        private TaskCompletionSource<bool> Complete = new TaskCompletionSource<bool>();
        public Task<bool> TaskDone { get { return this.Complete.Task; } }
        public ActivityResultMessage ActivityMessageObtained { get; set; }
        MainActivity Activity;
        public ActivityResultListener(MainActivity mainActivity)
        {
            Activity = mainActivity;
            mainActivity.ActivityResultReady += OnActivityResultObtained;
        }
        private void OnActivityResultObtained(object sender, ActivityResultMessage args)
        {
            //Unsubscribe from event
            Activity.ActivityResultReady -= OnActivityResultObtained;

            if ((Result)args.ResultCode == Result.Ok)
            {
                ActivityMessageObtained = args;
                this.Complete.TrySetResult(true);
            }
            else
            {
                this.Complete.TrySetResult(false);
            }

        }
    }
}