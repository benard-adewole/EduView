using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Estudiar.CustomViews;
using Estudiar.Droid.CustomViews;
using Xamarin.Forms.Platform.Android;
using System.Runtime.Remoting.Messaging;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(AndroidWebViewRenderer))]
namespace Estudiar.Droid.CustomViews
{
	public class AndroidWebViewRenderer : WebViewRenderer
	{
		Context context1;
		public AndroidWebViewRenderer(Context context) : base(context)
		{
			context1 = context;
			//MainActivity.GetMainActivity.Window.AddFlags(WindowManagerFlags.TranslucentStatus);
			//MainActivity.GetMainActivity.Window.ClearFlags(WindowManagerFlags.ForceNotFullscreen);


		}

		protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
		{
			base.OnElementChanged(e);
			
			if (e.NewElement != null)
			{
				Control.Settings.AllowFileAccess = true;
				Control.Settings.AllowFileAccessFromFileURLs = true;
				Control.Settings.AllowUniversalAccessFromFileURLs = true;
				Android.Webkit.WebView webview = (Android.Webkit.WebView)Control;
				Android.Webkit.WebSettings settings = webview.Settings;
				settings.JavaScriptCanOpenWindowsAutomatically = false;
				settings.BuiltInZoomControls = true;
				settings.SetSupportZoom(true);
				settings.JavaScriptEnabled = true;

				//var headers = new Dictionary<string,string>();
				//headers.Add("Authorization", Token);
				//Control.LoadUrl("https://drive.google.com/viewerng/viewer?embedded=true&url=" + "http://46.101.26.207:5000/course/GEG115?name=Geg412.pdf", headers);
				//Control.LoadUrl(String.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}",string.Format(""), WebUtility.UrlEncode("https://www.adobe.com/content/dam/acom/en/devnet/pdf/pdfs/PDF32000_2008.pdf")));
			}
		}
		
	}
}