using Estudiar.Helpers;
using Estudiar.ViewModels;
using Syncfusion.XForms.Backdrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseInfo : SfBackdropPage
    {
        CourseVM courseVM;

        public CourseInfo()
        {
            InitializeComponent();
            courseVM = this.BindingContext as CourseVM;
            courseVM.MaterialChangedEvent += CourseVM_MaterialChangedEvent; ;

        }

        private void CourseVM_MaterialChangedEvent(object sender, Models.Resource e)
        {
            @this.FrontLayer.IsVisible = true;
            @this.FrontLayer.RevealedHeight = 200;
            @this.IsBackLayerRevealed = false;
        }

        protected async override void OnAppearing()
        {
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["BackgroundColorDark"], false);
            }
            else
            {
                Settings.ChangeStatus(Color.Black, false);
            }

            LoadPageAsync();
            
            base.OnAppearing();


        }

        public void RevealFrontLayer()
        {
            @this.IsBackLayerRevealed = false;

        }

        private async void LoadPageAsync()
        {   
            try
            {
                await Task.Delay(700);
                await testLazyview.LoadViewAsync();
            }
            catch (Exception)
            {

            }
           
        }

        private void closePopOverBtn_Clicked(object sender, EventArgs e)
        {
            @this.FrontLayer.RevealedHeight = 0;
            @this.IsBackLayerRevealed = true;
        }
    }
}