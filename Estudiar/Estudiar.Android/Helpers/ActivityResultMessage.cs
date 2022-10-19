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

namespace Estudiar.Droid.Helpers
{
    public class ActivityResultMessage
    {
        public static string Key = "arm";
        public int RequestCode { get; set; }
        public object ResultCode { get; set; }
        public Intent MyIntent { get; set; }
    }
}