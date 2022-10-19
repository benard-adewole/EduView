using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudiar.ViewModels.Course_sample;
using Estudiar.Views.CourseSample;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Estudiar.ViewModels;
using Estudiar.Models;
using Estudiar.XInterface;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Estudiar.Helpers;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(MainVM), "VM")]
    public partial class Courses : ContentPage
    {
        Random random = new Random();
        MainVM VM;
        enum CourseDisplayViews
        {
            List,Grid
        }

        CourseDisplayViews SelectedCourseDisplayView;

        private int _SelectedCourseDisplayView;

        public int SelectedCourseDisplayViewIndex
        {
            get { return _SelectedCourseDisplayView; }
            set { _SelectedCourseDisplayView = value; }
        }

        public Courses(MainVM vm)
        {
            RunfromConstructor();
            VM = vm;
            BindingContext = VM;
            
        }

        private async void VM_SelecetedCourseChanged(object sender, EventArgs e)
        {
            await Task.Delay(200);


            //prevents opening next page twice
            if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 1] is Courses)
            {
                await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new A_Course(VM)); ;//loads page
            }
            
            //VM.SelecetedCourseChanged -= VM_SelecetedCourseChanged;


        }

        public Courses()
        {
            RunfromConstructor();
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
        }
        private async Task RunfromConstructor()
        {
            InitializeComponent();
            



            //BindingContext =  ServiceLocator.Current.GetInstance<MainVM>();
            Restore();
            
            
            
            /*switch (SelectedCourseDisplayView)
            {
                case CourseDisplayViews.List:
                    RegCourses.ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { HorizontalItemSpacing = 0, VerticalItemSpacing = 0 };
                    RegCourses.ItemTemplate = (DataTemplate)Resources["CourseListTemplate"];
                    //ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewGrid, Color = Color.White, FontFamily = "MF" }; ;
                    break;
                case CourseDisplayViews.Grid:
                    RegCourses.ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical) { HorizontalItemSpacing = 0, VerticalItemSpacing = 0 };
                    RegCourses.ItemTemplate = (DataTemplate)Resources["CourseGridTemplate"];
                    //ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewList, Color = Color.White, FontFamily="MF" };
                    break;
                default:
                    break;
            }*/
            /*CrossMTAdmob.Current.OnInterstitialLoaded += Current_OnInterstitialLoaded;
            CrossMTAdmob.Current.OnRewardedVideoAdLoaded += Current_OnRewardedVideoAdLoaded;
            CrossMTAdmob.Current.OnRewardedVideoAdOpened += Current_OnRewardedVideoAdOpened;
            CrossMTAdmob.Current.OnInterstitialOpened += Current_OnRewardedVideoAdOpened;*/
            //RegCourses.ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) {HorizontalItemSpacing=5,VerticalItemSpacing=5 };
            //RegCourses.ItemTemplate = (DataTemplate)Resources["CourseListTemplate"];
        }
       
        private void Restore()
        {
            /*if (Xamarin.Forms.Application.Current.Properties.ContainsKey("SelectedCourseDisplayView_Course"))
            {
                SelectedCourseDisplayView = (CourseDisplayViews)(int)Xamarin.Forms.Application.Current.Properties["SelectedCourseDisplayView_Course"];
            }
            else
            {
                SelectedCourseDisplayView = CourseDisplayViews.List;
            }*/
        }
        private DateTime prev;
        private async void doubleTapToExit()
        {
            if (prev != null && (DateTime.Now - prev).TotalMilliseconds < 500)
            {
                //exit
                DependencyService.Get<ITerminate>().closeApp();
            }
            else
            {
                prev = DateTime.Now;
                DependencyService.Get<IToast>().LongToast("Doube tap to exit");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            doubleTapToExit();
            return true;
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //VM.SelectedCourse = null;
            //CloseSearchView();
        }
        /*private void Current_OnRewardedVideoAdLoaded(object sender, EventArgs e)
        {
            CrossMTAdmob.Current.ShowRewardedVideo();
        }

        private void Current_OnInterstitialLoaded(object sender, EventArgs e)
        {
            CrossMTAdmob.Current.ShowInterstitial();
        }*/
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //VM.SelectedCourse = new Course();
            //Settings.ShowStatusBar();

            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColorDark"], false);
            }
            else
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColor"], true);
            }
            MyCourses myCourses = AppState<MyCourses>.DeserializeAndRestore("MyCourses");

            if (myCourses!= null && myCourses.courses != null)
            {
                MainVM mainVM = new MainVM() { MyCourses = myCourses };
                mainVM.CoursesFilterByCode();
                BindingContext = mainVM;
                VM = mainVM;
            }
            else
            {
                MainVM test = new MainVM();
                BindingContext = test;
                await test.GetCourses("courses");
                //test.CoursesFilterByCode();
            }
            /*MyCourses myCourses = AppState<MyCourses>.DeserializeAndRestore("MyCourses");
            if (myCourses != null && VM != null && myCourses != VM.MyCourses)
            {
                MainVM mainVM = new MainVM() { MyCourses = myCourses };
                mainVM.SortCourses();
                VM = mainVM;
            }
            try
            {
                RegCourses.SetBinding(CollectionView.ItemsSourceProperty, new Binding { Source = VM.MyCourses.courses });
            }
            catch (Exception)
            {
            }
            

            VM.SelectedCourse = null;*/

            if (!(bool)Preferences.Get("Resume",false) && Device.RuntimePlatform != Device.UWP)
            {
                
                switch (random.Next(4))
                {
                    case 0:
                        //interterrestial ad
                        //CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-7656672211380452/8312157877");
                        //CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-7656672211380452/1746327403");
                        //CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-3940256099942544/1033173712");
                        break;
                    case 1:
                        //video ad
                        //CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-7656672211380452/8032956278");
                        //CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-7656672211380452/6228497891");
                        //CrossMTAdmob.Current.LoadRewardedVideo("ca-app-pub-3940256099942544/5224354917");
                        break;
                    default:
                        break;
                }
            }
            
           
            
            
        }

        private void Current_OnRewardedVideoAdOpened(object sender, EventArgs e)
        {
            Application.Current.Properties["Resume"] = true;
        }

        private async void closePage()
        {
            await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PopAsync();
        }
        private void ChangeViewBtn_Clicked(object sender, EventArgs e)
        {
            switch (SelectedCourseDisplayView)
            {
                case CourseDisplayViews.List:
                    SelectedCourseDisplayView = CourseDisplayViews.Grid;
                    (RegCourses.ItemsLayout as GridItemsLayout).Span = 3;
                    
                    RegCourses.ItemTemplate = (DataTemplate)Resources["CourseGridTemplate"];
                    //ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewList, Color = Color.White, FontFamily = "MF" };//"list.png";
                    Xamarin.Forms.Application.Current.Properties["SelectedCourseDisplayView_Course"] = (int)SelectedCourseDisplayView;
                    break;
                case CourseDisplayViews.Grid:
                    SelectedCourseDisplayView = CourseDisplayViews.List;
                    (RegCourses.ItemsLayout as GridItemsLayout).Span = 1;
                    
                    RegCourses.ItemTemplate = (DataTemplate)Resources["CourseListTemplate"];
                    //ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewGrid, Color = Color.White, FontFamily = "MF" };
                    Xamarin.Forms.Application.Current.Properties["SelectedCourseDisplayView_Course"] = (int)SelectedCourseDisplayView;
                    break;
                default:
                    break;
            }
        }
        /*private void SearchCourseBtn_Clicked(object sender, EventArgs e)
        {
            if (TitleEntry.ScaleX == 1)//if search field is already open
            {
                CloseSearchView();
                return;
            }
            TitleEntry.IsVisible = true;
            TitleEntry.TranslationX = Width / 2 - 40; //position view
            Task t2 = TitleEntry.TranslateTo(0, 0, 300); //aniate view
            Task t = PageTitle.FadeTo(0, 300);
            Task t1 = TitleEntry.ScaleXTo(1, 300);

            SearchCourseBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.Close, Color = Color.White, FontFamily = "MF" };
            TitleEntry.Focus();
        }*/

        private void TitleEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                IEnumerable<Course> courses = VM.MyCourses.courses.Where(course => course.code.ToLower().Contains(e.NewTextValue.ToLower()) );
                RegCourses.ItemsSource = courses;
            }
            else
            {
                
                RegCourses.SetBinding(CollectionView.ItemsSourceProperty, new Binding { Source = VM.MyCourses.courses });
            }
            
        }
        

        private void TitleEntry_Unfocused(object sender, FocusEventArgs e)
        {
            /*if (string.IsNullOrEmpty(TitleEntry.Text))
            {
                //CloseSearchView();
                RegCourses.SetBinding(CollectionView.ItemsSourceProperty, new Binding { Source = VM.MyCourses.courses });
            }*/
        }
        /*private void CloseSearchView()
        {
            if (TitleEntry.ScaleX == 1)
            {
                TitleEntry.Text = "";
                TitleEntry.TranslateTo(Width / 2 - 40, 0, 300);
                TitleEntry.ScaleXTo(0, 300);
                PageTitle.FadeTo(1, 300);

                SearchCourseBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.Magnify, Color = Color.White, FontFamily = "MF" }; ;
            }
        }*/

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            
            await VM.GetCourses("courses");
            MyCourses myCourses = AppState<MyCourses>.DeserializeAndRestore("MyCourses");
            if (myCourses != null && VM != null && myCourses != VM.MyCourses)
            {
                MainVM mainVM = new MainVM() { MyCourses = myCourses};
                mainVM.CoursesFilterByCode();
                VM = mainVM;
            }
            RegCourses.SetBinding(CollectionView.ItemsSourceProperty, new Binding { Source = VM.MyCourses.courses });
            (sender as RefreshView).IsRefreshing = false;
        }

        private void ViewBtn_Clicked(object sender, EventArgs e)
        {
            Button selectedBtn = (Button)sender;
            
            foreach (var ClickedCourse in VM.MyCourses.courses)
            {
                if (((Button)sender).CommandParameter.ToString().ToLower() == ClickedCourse.title.ToLower())
                {
                    VM.SelectedCourse = ClickedCourse;
                    VM.OpenACourse();
                    break;
                }

            }
        }
    }
}