using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estudiar.Helpers;
using Estudiar.Models;
using Estudiar.ViewModels;
using Estudiar.XInterface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        MainVM VM;
        MasterDetailPage MasterDetailPage;
        public Home()
        {
            InitializeComponent();
            Application.Current.RequestedThemeChanged += (s, a) =>
            {
                TheTheme.setTheme();
            };
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

        private async void Test_RefreshComplete()
        {
            MainVM test = new MainVM();
            VM = test;
            BindingContext = test;
            await test.GetCourses("home");
            /*test.MyCourses = new MyCourses() { 
                courses = new System.Collections.ObjectModel.ObservableCollection<Course>() 
                {
                    new Course 
                    {
                        dept="systems", code="ssg121",faculty="engineering",
                        materials = new System.Collections.ObjectModel.ObservableCollection<Material> 
                        {
                            new Material 
                            {
                                code ="ssg123",name="excerpt",title = "Engineering mechanics",year= 100
                            },
                            new Material
                            {
                                code ="ssg123",name="excerpt",title = "Engineering mechanics",year= 100
                            },
                            new Material
                            {
                                code ="ssg123",name="excerpt",title = "Engineering mechanics",year= 100
                            },
                        }
                    },
                    new Course
                    {
                        dept="systems", code="ssg221",faculty="engineering",
                        materials = new System.Collections.ObjectModel.ObservableCollection<Material>
                        {
                            new Material
                            {
                                code ="ssg323",name="excerpt",title = "Engineering mechanics",year =100
                            },
                            new Material
                            {
                                code ="ssg123",name="excerpt",title = "Engineering mechanics",year= 100
                            },
                        }
                    }
                }
            };
            test.HomeFilterByCode();*/
            try
            {
                //VM.SelectedCourse = VM.MyCourses.courses[0];
                //test.HomeFilterByCode();
            }
            catch (Exception)
            {

            }
        }

        

        private async void ScrollCarouselByTime()
        {
            return;
            Device.StartTimer(TimeSpan.FromMilliseconds(4), () =>
            {
                //AnimatedCView.Position = (AnimatedCView.Position + 1) % VM.Homefeed.Count;
                return true;
            });
        }
        private void VM_MenuButtonClicked(object sender, EventArgs e)
        {
            if (MasterDetailPage != null)
            {
                MasterDetailPage.IsPresented = true;
            }
        }

        protected override void OnAppearing()
        {

            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColorDark"], false);
            }
            else
            {
                Settings.ChangeStatus((Color)Application.Current.Resources["BackgroundColor"], true);
            }
            

            MyCourses myCourses = AppState<MyCourses>.DeserializeAndRestore("MyCourses");

            if (myCourses != null && myCourses.courses != null && myCourses.courses.Count != 0)
            {
                MainVM mainVM = new MainVM() { MyCourses = myCourses };
                BindingContext = mainVM;
                mainVM.HomeFilterByCode();
                VM = mainVM;
                
                
                //
                //mainVM.SortCourses();
                //VM.SelectedCourse = VM.MyCourses.courses[0];
            }
            else
            {
                Test_RefreshComplete();
            }

            //Settings.ChangeStatus(Color.White, true);
            CourseMaterial.SelectedItem = null;
            base.OnAppearing();

        }

    }
}