using Android.Content;
using Android.Views;
using Android.Widget;
using Estudiar.CustomViews;
using planner.Platforms.Android.CustomViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
//using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Dropdown), typeof(DropDownRenderer))]

namespace planner.Platforms.Android.CustomViews
{
    public class DropDownRenderer:ViewRenderer<Dropdown, Spinner>
    {
        Spinner spinner;
        public DropDownRenderer(Context context) : base(context)
        {

        }
        private void SetUpSpinner()
        {
            LayoutParams layout = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
            spinner.LayoutParameters = layout;
            var metrics = Resources.DisplayMetrics;
            spinner.DropDownWidth = metrics.WidthPixels;
            spinner.DropDownVerticalOffset = 100;
            spinner.SetForegroundGravity(GravityFlags.End);
            spinner.SetGravity(GravityFlags.End);
            spinner.SetPadding(0, 0, 0, 0);
            
            try
            {
                Java.Lang.Reflect.Field popup = spinner.Class.GetDeclaredField("mPopup");
                popup.Accessible = true;
                var popupWindow = (ListPopupWindow)popup.Get(spinner);
                popupWindow.Height = 700;
            }
            catch (Java.Lang.Exception)
            {
                // Failed...
            }
        }
        private void OnItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            var view = Element;
            if (view != null)
            {
                view.SelectedIndex = e.Position;
                view.OnItemSelected(e.Position);
            }
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Dropdown> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                spinner = new Spinner(Context);
                //spinner.SetGravity(GravityFlags.End);
                //spinner.SetPadding(0, 0, 0,0);
                //commented so that spinner doesn't fill horizontal width
                //SetUpSpinner();
                SetNativeControl(spinner);
            }

            if (e.OldElement != null)
            {
                Control.ItemSelected -= OnItemSelected;
            }
            if (e.NewElement != null)
            {
                var view = e.NewElement;
                ArrayAdapter adapter = new ArrayAdapter(
                context: Context,
                resource: Resource.Layout.select_dialog_item_material,
                objects: Element.ItemsSource);
                //commented so that spinner doesn't fill horizontal width
                adapter.SetDropDownViewResource(Estudiar.Droid.Resource.Layout.layout_custom_picker);
                Control.Adapter = adapter;
                //Control.SetPadding(0, 0, 0, 0);
                //Control.SetBackgroundResource(plan.Droid.Resource.Drawable.simple_spinner_item);
                if (view.SelectedIndex != -1)
                {
                    Control.SetSelection(view.SelectedIndex);
                }

                Control.ItemSelected += OnItemSelected;
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var view = Element;
            if (e.PropertyName == Dropdown.ItemsSourceProperty.PropertyName)
            {
                ArrayAdapter adapter = new ArrayAdapter(
                context: Context,
                resource: Resource.Layout.select_dialog_item_material,
                objects: Element.ItemsSource);
                //commented so that spinner doesn't fill horizontal width
                adapter.SetDropDownViewResource(Estudiar.Droid.Resource.Layout.layout_custom_picker);
                Control.Adapter = adapter;
                //ArrayAdapter adapter = new ArrayAdapter(Context,  plan.Droid.Resource.Layout.layout_custom_picker, view.ItemsSource);
                //Control.Adapter = adapter;
            }
            if (e.PropertyName == Dropdown.SelectedIndexProperty.PropertyName)
            {
                Control.SetSelection(view.SelectedIndex);
            }
            base.OnElementPropertyChanged(sender, e);
        }
    }
}
