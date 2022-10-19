using Estudiar.ViewModels.Course_sample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Estudiar.ViewModels;
using Estudiar.Views.CourseSample;
using Estudiar.Models;
using Estudiar.Helpers;

namespace Estudiar.Views.CourseSample
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class A_Course : ContentPage
    {
        public Action CustomBackButtonAction { get; set; }
        MainVM VM;
        double previousScrollValue = 0;
        enum CourseDisplayViews
        {
            List, Grid
        }
        CourseDisplayViews SelectedCourseDisplayView;
        
        public A_Course(MainVM mainVM)
        {
            Restore();
            InitializeComponent();


            BindingContext = mainVM;
            VM = mainVM;
            
            CustomBackButtonAction = () =>
            {
                if (IsTitleEntryClosed())
                {
                    //closes this page
                    closePage();
                }
            };

            //VM = BindingContext as VM_Course;
            //CourseMaterial.ItemsSource = ThisCourseMaterials;

            switch (SelectedCourseDisplayView)
            {
                case CourseDisplayViews.List:
                    CourseMaterial.ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { HorizontalItemSpacing = 5, VerticalItemSpacing = 5 };
                    CourseMaterial.ItemTemplate = (DataTemplate)Resources["CourseListTemplate"];
                    ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewGrid, Color = Color.White, FontFamily = "MF" }; ;
                    break;
                case CourseDisplayViews.Grid:
                    CourseMaterial.ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical) { HorizontalItemSpacing = 5, VerticalItemSpacing = 5 };
                    CourseMaterial.ItemTemplate = (DataTemplate)Resources["CourseGridTemplate"];
                    ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewList, Color = Color.White, FontFamily = "MF" }; ;
                    break;
                default:
                    break;
            }
            //Navigation.NavigationStack.LastOrDefault();
        }
        public A_Course()
        {
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
            Course myCourse = AppState<Course>.DeserializeAndRestore("SelectedCourse");
            if (myCourse != null)
            {
                MainVM mainVM = new MainVM();
                mainVM.SelectedCourse = myCourse;
                BindingContext = mainVM;
                VM = mainVM;
            }

            Restore();
            InitializeComponent();
            CustomBackButtonAction = () =>
            {
                if (IsTitleEntryClosed())
                {
                    //closes this page
                    closePage();
                }
            };


            switch (SelectedCourseDisplayView)
            {
                case CourseDisplayViews.List:
                    CourseMaterial.ItemsLayout = new GridItemsLayout(1, ItemsLayoutOrientation.Vertical) { HorizontalItemSpacing = 0, VerticalItemSpacing = 0 };
                    CourseMaterial.ItemTemplate = (DataTemplate)Resources["CourseListTemplate"];
                    ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewGrid, Color = Color.White, FontFamily = "MF" }; ;
                    break;
                case CourseDisplayViews.Grid:
                    CourseMaterial.ItemsLayout = new GridItemsLayout(3, ItemsLayoutOrientation.Vertical) { HorizontalItemSpacing = 0, VerticalItemSpacing = 0 };
                    CourseMaterial.ItemTemplate = (DataTemplate)Resources["CourseGridTemplate"];
                    ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewList, Color = Color.White, FontFamily = "MF" };
                    break;
                default:
                    break;
            }
        }

       

        private async void VM_OpenPdfEvent(object sender, EventArgs e)
        {
            //When nothing selected Stop

            await Task.Delay(200);
            var page = new PDFViewer(VM);
            if (Device.RuntimePlatform == Device.UWP)
            {
                NavigationPage.SetHasNavigationBar(page, false);//removes Navigation bar page to be loaded
            }
            //VM.OpenPdfEvent -= VM_OpenPdfEvent;
            if (Navigation.NavigationStack[Navigation.NavigationStack.Count - 1] is A_Course)
            {
                await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(page);//loads page

            }

        }
    


        //protected override On
        private void Restore()
        {
            if (Xamarin.Forms.Application.Current.Properties.ContainsKey("SelectedCourseDisplayView_A_Course"))
            {
                SelectedCourseDisplayView = (CourseDisplayViews)(int)Xamarin.Forms.Application.Current.Properties["SelectedCourseDisplayView_A_Course"];
            }
            else
            {
                SelectedCourseDisplayView = CourseDisplayViews.List;
            }
        }
        public bool IsTitleEntryClosed()
        {
            if (TitleEntry.ScaleX == 1)
            {
                TitleEntry.Text = "";
                TitleEntry.TranslateTo(Width / 2 - 40, 0, 300);
                TitleEntry.ScaleXTo(0, 300);
                PageTitle.FadeTo(1, 300);
                SearchCourseBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.Magnify, Color = Color.White, FontFamily = "MF" };
                return false;
            }
            //Title view was opened
            return true;
        }
        protected override bool OnBackButtonPressed()
        {
            //return true;
            if (TitleEntry.ScaleX == 1)
            {
                TitleEntry.Text = "";
                TitleEntry.TranslateTo(Width / 2 - 40, 0, 300);
                TitleEntry.ScaleXTo(0, 300);
                PageTitle.FadeTo(1, 300);
                return true;
            }
            /*if (IsTitleEntryClosed())
            {
                return false;
            }*/
            else
            {
                closePage();
            }
            return false;
            //return base.OnBackButtonPressed();
        }
        private async void closePage()
        {
            Application.Current.Properties.Remove("SelectedCourse");

            //(Application.Current.MainPage as MasterDetailPage).Detail.Navigation
            //wait (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PopAsync();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Settings.ChangeStatus((Color)Application.Current.Resources["AppColor"], false);

            Device.BeginInvokeOnMainThread(async() => 
            {
                Course myCourse = AppState<Course>.DeserializeAndRestore("SelectedCourse");
                if (myCourse != null)
                {
                    MainVM mainVM = new MainVM();
                    mainVM.SelectedCourse = myCourse;
                    //BindingContext = mainVM;
                    VM = mainVM;
                }
                CourseMaterial.SelectedItem = null;
            });
            
            /*VM.OpenPdfEvent += VM_OpenPdfEvent;
            Material material = AppState<Material>.DeserializeAndRestore("SelectedMaterial");
            if (material != null)
            {
                //await Task.Delay(10);
                VM.SelectedMaterial = material;
            }
            CourseMaterial.SelectedItem = null;*/
        }
        
        private void ChangeViewBtn_Clicked(object sender, EventArgs e)
        {
            switch (SelectedCourseDisplayView)
            {
                case CourseDisplayViews.List:
                    SelectedCourseDisplayView = CourseDisplayViews.Grid;
                    (CourseMaterial.ItemsLayout as GridItemsLayout).Span = 3;
                    CourseMaterial.ItemTemplate = (DataTemplate)Resources["CourseGridTemplate"];
                    ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewList, Color = Color.White, FontFamily = "MF" }; ;

                    Xamarin.Forms.Application.Current.Properties["SelectedCourseDisplayView_A_Course"] = (int)SelectedCourseDisplayView;
                    break;
                case CourseDisplayViews.Grid:
                    SelectedCourseDisplayView = CourseDisplayViews.List;
                    (CourseMaterial.ItemsLayout as GridItemsLayout).Span = 1;
                    CourseMaterial.ItemTemplate = (DataTemplate)Resources["CourseListTemplate"];
                    ChangeViewBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.ViewGrid, Color = Color.White, FontFamily = "MF" }; ;
                    Xamarin.Forms.Application.Current.Properties["SelectedCourseDisplayView_A_Course"] = (int)SelectedCourseDisplayView;
                    break;
                default:
                    break;
            }
        }
        private async void Q_And_A_Btn_Clicked(object sender, EventArgs e)
        {
            await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new Q_And_A());
        }


        //animates the search field
        private void SearchCourseBtn_Clicked(object sender, EventArgs e)
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
            SearchCourseBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.Close, Color = Color.White, FontFamily = "MF" }; ;
            TitleEntry.Focus();
            
        }

        private async void QuizBtn_Clicked(object sender, EventArgs e)
        {
            await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(new Quiz());//opens the Quiz page
        }
        private async void CourseMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

        }

        private void TitleEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                IEnumerable<Material> materials = VM.SelectedCourse.materials.Where(material => material.title.ToLower().Contains(e.NewTextValue.ToLower()) ||
                material.name.ToLower().Contains(e.NewTextValue.ToLower()) || material.year.ToString().Contains(e.NewTextValue.ToLower()));
                CourseMaterial.ItemsSource = materials;
            }
            else
            {
                CourseMaterial.SetBinding(CollectionView.ItemsSourceProperty, new Binding { Source = VM.SelectedCourse.materials });
            }
        }
        private void TitleEntry_Unfocused(object sender, FocusEventArgs e)
        {
            /*if (string.IsNullOrEmpty(TitleEntry.Text))
            {
                //CloseSearchView();   
                CourseMaterial.SetBinding(CollectionView.ItemsSourceProperty, new Binding { Source = VM.SelectedCourse.materials });
            }*/
        }

        private void CloseSearchView()
        {
            if (TitleEntry.ScaleX == 1)
            {
                TitleEntry.Text = "";
                TitleEntry.TranslateTo(Width / 2 - 40, 0, 300);
                TitleEntry.ScaleXTo(0, 300);
                PageTitle.FadeTo(1, 300);
                SearchCourseBtn.IconImageSource = new FontImageSource { Glyph = MaterialDesign.FontIcon.Magnify, Color = Color.White, FontFamily = "MF" };
            }
        }

        
    }
}