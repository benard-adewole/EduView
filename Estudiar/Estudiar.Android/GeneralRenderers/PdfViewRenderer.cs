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
using Estudiar.Droid.GeneralRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Xamarin.Forms.WebView), typeof(PdfViewRenderer))]
namespace Estudiar.Droid.GeneralRenderers
{
    public class PdfViewRenderer: Xamarin.Forms.Platform.Android.WebViewRenderer
    {
        public PdfViewRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {


                //var pdfView = Element as PDFView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;
                //if (pdfView.IsPDF)
                {
                    var url = "https://drive.google.com/viewerng/viewer?embedded=true&url=" + "www.mdpi.com/2076-0760/6/4/126/pdf&ved=2ahUKEwjv-I7j1bDwAhUsSBUIHUdSCO8QFjALegQIEBAC&usg=AOvVaw3EmLog_CBfvdNWF44ait-n";

                    Control.LoadUrl(url);
                }
                //else
                {
                    //Control.LoadUrl(pdfView.Uri);
                }

            }
        }
    }
}