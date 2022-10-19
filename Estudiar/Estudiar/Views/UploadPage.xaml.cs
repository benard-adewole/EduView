using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudiar.CustomViews;
using Estudiar.Helpers;
using Estudiar.Models;
using Estudiar.ViewModels;
using Estudiar.XInterface;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadPage : ContentPage
    {
        IncomingFile Uploadableimage = new IncomingFile();
        MainVM VM;
        public PickerAlert FacultyAlert;

        public PickerAlert DeptAlert;

        public UploadPage()
        {
            //MainVM mainVM = new MainVM();
            //BindingContext = mainVM;
            
            InitializeComponent();
            VM = BindingContext as MainVM;
            FacultyAlert = new PickerAlert(
                    VM.Faculty_Department, VM.SelectedFac);

            FacultyAlert.OnSelected += Alert_OnSelected;
            VM.UploadButtonClicked += VM_CustomButtomClicked;
            VM.SelectMaterialsButtonClicked += VM_SelectMaterialsButtonClicked;
            VM.MaterialUploadedEvent += VM_MaterialUploadedEvent;
            VM.ClearUploadEvent += VM_ClearUploadEvent;

            if (Device.RuntimePlatform == Device.Android)
            {
                Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Pan);
            }
        }
       
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["AppColor"], false);
        }


        private void VM_ClearUploadEvent(object sender, EventArgs e)
        {
            MaterialYrPicker.SelectedItem = null; 
        }

        private async void VM_MaterialUploadedEvent(object sender, string e)
        {
            //calls this method after material hhas been uploaded
            await DisplayAlert("Alert", e, "Ok");
        }


        private async void VM_SelectMaterialsButtonClicked(object sender, EventArgs e)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.Android:
                    VM.UploadModel.Material = await DependencyService.Get<FilePickCaller>().GetFileStream();
                    break;
                case Device.UWP:
                    VM.UploadModel.Material = await DependencyService.Get<FilePickCaller>().GetFileStream();
                    break;
                default:
                    break;
            }   

            
            VM.UploadModel = VM.UploadModel;
            
            //await DisplayAlert("",Uploadableimage[0].filePath,"OK");
        }

        private async void VM_CustomButtomClicked(object sender, EventArgs e)
        {
            FacultyAlert = new PickerAlert(
                    VM.Faculty_Department, VM.SelectedFac);
            FacultyAlert.OnSelected += Alert_OnSelected;
            await Navigation.PushModalAsync(FacultyAlert, false);
            
            
            //await Shell.Current.GoToAsync(nameof(CustomViews.PickerAlert));

        }

        private void Alert_OnSelected(object sender, object e)
        {
            VM.SelectedFac = e as Model_Fac_dept;
        }

        private async void RequestAdmin_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}