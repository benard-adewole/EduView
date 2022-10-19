using Estudiar.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Syncfusion.XForms.Buttons;
using Estudiar.ViewModels;
using Estudiar.XInterface;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomTab : ContentPage
    {
        CustomTabVM CustomTabVM;
        public CustomTab()
        {
            InitializeComponent();
            mainTab.SelectedIndex = 0;
            CustomTabVM = BindingContext as CustomTabVM;
        }

        private DateTime prev;
        private async void doubleTapToExit()
        {
            if (prev == DateTime.MinValue)
            {
                prev = DateTime.Now;
                DependencyService.Get<IToast>().LongToast("Double action to exit");
            }
            else if (prev != DateTime.MinValue && (DateTime.Now - prev).TotalMilliseconds < 500)
            {
                //exit
                prev = DateTime.MinValue;

                DependencyService.Get<ITerminate>().closeApp();
            }
            else if((DateTime.Now - prev).TotalMilliseconds > 500)
            {
                prev = DateTime.MinValue;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            doubleTapToExit();
            return true;
        }

        protected override void OnAppearing()
        {
            //mainTab.SelectedIndex = 0;
            prev = DateTime.MinValue;
            if (App.Current.RequestedTheme == Xamarin.Forms.OSAppTheme.Dark)
            {
                Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["BackgroundColorDark"], false);
            }
            else
            {
                //Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["BackgroundColorDark"], false);
                Settings.ChangeStatus((Color)Xamarin.Forms.Application.Current.Resources["BackgroundColor"], true);
            }

            //mainTab.SelectedIndex = 2;
            base.OnAppearing();

        }

        private async void searchEntry_Focused(object sender, FocusEventArgs e)
        {
            //(sender as Entry).Unfocus();
            //mainTab.SelectedIndex = 1;
            //await Task.Delay(100);
            //searchPageSearchEntry.Focus();
        }

        private void mainTab_SelectionChanged(object sender, Xamarin.CommunityToolkit.UI.Views.TabSelectionChangedEventArgs e)
        {
            switch (e.NewPosition)
            {
                case 1:
                    //searchPageSearchEntry.Focus();
                    break;
                default:
                    break;
            }
        }

        private async void FindCourseBtn_Clicked(object sender, EventArgs e)
        {
            //(sender as Entry).Unfocus();
            
            mainTab.SelectedIndex = 1;
            await Task.Delay(200);
            searchPageSearchEntry.Focus();
        }

        private async void EditToggleBtn_Clicked(object sender, EventArgs e)
        {
            if (!(sender as SfButton).IsChecked)
            {
                
                string name = NameEntry.Value.Trim().ToLower();
                string email = EmailEntry.Value.Trim().ToLower();
                string password = PasswordEntry.Value;
                int level = (int)LevelPicker.SelectedItem;
                string faculty = CustomTabVM.ProfileEditVM.SelectedFac.Faculty.ToLower()??"";
                string dept = CustomTabVM.ProfileEditVM.SelectedDept.Department.ToLower()??"";

                bool refreshCourses = false;
                if (CustomTabVM.LoginResponse.user.year != level || CustomTabVM.LoginResponse.user.faculty != faculty && string.IsNullOrEmpty(faculty) || CustomTabVM.LoginResponse.user.dept != dept && string.IsNullOrEmpty(dept))
                {
                    refreshCourses = true;
                }
                //change name
                if ( CustomTabVM.LoginResponse.user.name.ToLower().Trim() != name)
                {
                    if (string.IsNullOrEmpty(name))
                    {
                        await DisplayAlert("Field Missing", "Name is required", "Ok");
                        return;
                    }
                    await CustomTabVM.EditProfileAsync("name", name);
                }

                //change level
                if (CustomTabVM.LoginResponse.user.year != level)
                {
                    await CustomTabVM.EditProfileAsync("year", level.ToString());
                }

                //change department
                if (CustomTabVM.LoginResponse.user.faculty != faculty && string.IsNullOrEmpty(faculty))
                {
                    await CustomTabVM.EditProfileAsync("faculty", faculty);
                }
                if (CustomTabVM.LoginResponse.user.dept != dept)
                {
                    await CustomTabVM.EditProfileAsync("dept", dept);
                }

                if (refreshCourses)
                {
                    await CustomTabVM.GetCourses("");
                }

            }
            
        }
       
        private void AccountBox_Clicked(object sender, EventArgs e)
        {
            mainTab.SelectedIndex = 2;
        }
    }
}