using Estudiar.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OnBoarding : CarouselPage
    {
        
        public ICommand OpenShellApp
        {
            get
            {
                return new Command(() =>
                {
                    OpenShell_Clicked(null, null);
                });
            }
            private set { }
        }
        public OnBoarding()
        {
            InitializeComponent();
            BindingContext = this;
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Settings.ChangeStatus((Color)Application.Current.Resources["AppColor"], false);
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColorDark"], false);
            }
            else
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColor"], true);
            }

        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            
        }

        public void OpenShell_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new Views.AppShell();
            //(Application.Current.MainPage as Estudiar.Views.AppShell).setEntry();
        }

        int page = 1;
        private void CarouselOne_Clicked(object sender, EventArgs e)
        {
            switch (page)
            {
                case 1:
                    Task t2 = View1.FadeTo(0);
                    Task t3 = View1.ScaleTo(0);

                    Ind1.WidthRequest = 10;
                    Ind2.WidthRequest = 30;

                    View1.IsVisible = false;
                    View2.Opacity = 0;
                    View2.Scale = 0;
                    View2.IsVisible = true;
                    Task t4 = View2.FadeTo(1);
                    Task t5 = View2.ScaleTo(1);


                    Task i1 = image1.TranslateTo(1000,0);
                    Task i2 = image1.FadeTo(0,500);
                    image2.Opacity = 0;
                    image2.IsVisible = true;
                    image2.TranslationX =200;
                    image2.TranslateTo(0, 0);
                    image2.FadeTo(1);
                    break;
                case 2:
                    Task t7 = View2.FadeTo(0);
                    Task t8 = View2.ScaleTo(0);
                    View2.IsVisible = false;
                    View3.Opacity = 0;
                    View3.Scale = 0;
                    View3.IsVisible = true;
                    Task t9 = View3.FadeTo(1);
                    Task t10 = View3.ScaleTo(1);

                    Ind2.WidthRequest = 10;
                    Ind3.WidthRequest = 30;

                    Task i3 = image2.TranslateTo(1000, 0);
                    Task i4 = image2.FadeTo(0, 500);
                    image3.Opacity = 0;
                    image3.IsVisible = true;
                    image3.TranslationX = 200;
                    image3.TranslateTo(0, 0);
                    image3.FadeTo(1);
                    CarouselOne.Text = "Get Started";
                    SkipBtn.IsEnabled = false;
                    SkipText.Text = "";
                    break;
                case 3:
                    OpenShell_Clicked(null,null);
                    break;
                default:
                    break;
            }
            //MyCarousel.CurrentPage = Content2;
            page++;
        }

        private void MyCarousel_SizeChanged(object sender, EventArgs e)
        {
            if (this.Height < 500)
            {
                headerSVG.IsVisible = false;
            }
            else if (this.Height < 700)
            {
                headerSVG.IsVisible = true;
            }
            else
            {
                //headerSVG.IsVisible = true;
                headerSVG.IsVisible = true;
            }
        }
    }
}