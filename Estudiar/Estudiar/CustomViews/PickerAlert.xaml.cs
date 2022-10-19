using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudiar.Views;
using Estudiar.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Estudiar.ViewModels;
using Estudiar.Helpers;

namespace Estudiar.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PickerAlert : ContentPage
    {
        public event EventHandler<object> OnSelected;
        private object myVar;

        public object SelectedItem
        {
            get { return myVar; }
            set { myVar = value; }
        }

        protected override bool OnBackButtonPressed()
        {
            CloseThisView();
            return true;
        }
        public PickerAlert()
        {
            InitializeComponent();
            BindingContext = this;
            MainVM mainVM = new MainVM();
            CustomPicker.MyItemSource = mainVM.Faculty_Department;

            if (mainVM.SelectedFac != null)
            {
                //SelectedItem = CurrentSelectedItem;
                CustomPicker.SelectedItem = mainVM.SelectedFac;
            }
            CustomPicker.OnSelectedItemChanged += CustomPicker_OnSelectedItemChanged;
        }
        public PickerAlert(object Iterable, object CurrentSelectedItem)
        {
            
            InitializeComponent();
            BindingContext = this;
            CustomPicker.MyItemSource = Iterable;
            if (CurrentSelectedItem != null)
            {
                //SelectedItem = CurrentSelectedItem;
                CustomPicker.SelectedItem = CurrentSelectedItem;
            }
            CustomPicker.OnSelectedItemChanged += CustomPicker_OnSelectedItemChanged;
        }

        private void CustomPicker_OnSelectedItemChanged(object sender, object e)
        {
            SelectedItem = e;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(150);
            Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["AppColor"], false);
            CustomPicker.Toggle = true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        private void CustomButton_Clicked(object sender, EventArgs e)
        {

            CustomPicker.Toggle ^= true;

        }

        private void CustomPicker_OnToggleChanged(object sender, bool e)
        {
            //await DisplayAlert("",e.ToString(),"Ok");

        }

        private void CancelBtn_Clicked(object sender, EventArgs e)
        {
            CloseThisView();
        }

        private async void OkBtn_Clicked(object sender, EventArgs e)
        {
            if (SelectedItem == null)
            {
                await DisplayAlert("","Please Select an Item or Cancel","OK");
                return;
            }
            EventHandler<object> eventHandler = OnSelected;
            if (eventHandler != null)
            {
                eventHandler(this,SelectedItem);
            }

            CloseThisView();
        }

        private async void CloseThisView()
        {
            CustomPicker.Toggle = false;
            await Task.Delay(160);
            await Navigation.PopModalAsync(false);
        }

        private  async void CancelBtn_Pressed(object sender, EventArgs e)
        {
            await ((ImageButton)sender).ScaleTo(0.9,100);
        }

        private async void CancelBtn_Released(object sender, EventArgs e)
        {
            await((ImageButton)sender).ScaleTo(1.0,100);
        }
    }
}