using Estudiar.Models;
using Estudiar.ViewModels;
using Estudiar.XInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.CGPA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedGradesCV : ContentView
    {
        private calcVm vm;

        public calcVm VM
        {
            get { return vm; }
            set { vm = value; }
        }
        savedDataVm Context;
        public SavedGradesCV()
        {
            InitializeComponent();
            hideMenu();
            Context = BindingContext as savedDataVm;
            //(BindingContext as savedDataVm).EditClicked += SavedGrades_EditClicked;
        }
        public void OnAppearing()
        {
            Context.GetData();
            //base.OnAppearing();
        }

        private async void SavedGrades_EditClicked(object sender, SemesterGroup semesterGroup)
        {
            //var SemesterGroup = ((sender as ImageButton).CommandParameter as SemesterGroup);
            VM.SelectedSavedGradeToLoad = semesterGroup;
            VM.CalcOtherParams();
            await Shell.Current.Navigation.PopAsync();
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            var SemesterGroup = ((sender as ImageButton).CommandParameter as SemesterGroup);
            Entry entry = ((sender as ImageButton).Parent as Grid).Children[0] as Entry;
            string text = entry.Text;

            if (!string.IsNullOrEmpty(text) && text != SemesterGroup.Name)
            {
                SemesterGroup.Name = text.Trim().ToLower();
                Context.SemesterGroup = Context.SemesterGroup;
                entry.Unfocus();
                DependencyService.Get<IToast>().LongToast("Success: File name changed");
            }
            else if (string.IsNullOrEmpty(text))
            {
                entry.Unfocus();
                DependencyService.Get<IToast>().LongToast("Text field cannot be empty");
            }
            else
            {
                entry.Unfocus();
                DependencyService.Get<IToast>().LongToast("File name not changed");
            }
        }
        private void saveNewName(SemesterGroup semesterGroup)
        {

        }
        private async void hideMenu()
        {
            await Task.Delay(100);

            //closing menu that was  initially opened by xaml
            MoreMenu_Clicked(null, null);
        }
        private void MoreMenu_Clicked(object sender, EventArgs e)
        {
            MenuOptionGrid.IsVisible ^= true;
            MenuExpander.IsExpanded ^= true;
        }

        private void MenuOptionGrid_Tapped(object sender, EventArgs e)
        {
            MoreMenu_Clicked(null, null);
        }
        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    MoreMenu_Clicked(null, null);
                    break;
                case GestureStatus.Running:
                    break;
                case GestureStatus.Completed:
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    break;
            }
        }
        private void entry_Completed(object sender, EventArgs e)
        {
            var SemesterGroup = ((sender as Entry).ReturnCommandParameter as SemesterGroup);
            if (!string.IsNullOrEmpty((sender as Entry).Text) && (sender as Entry).Text != SemesterGroup.Name)
            {
                SemesterGroup.Name = (sender as Entry).Text.Trim().ToLower();
                Context.SemesterGroup = Context.SemesterGroup;
                (sender as Entry).Unfocus();
                DependencyService.Get<IToast>().LongToast("Success: File name changed");
            }
            else if (string.IsNullOrEmpty((sender as Entry).Text))
            {
                (sender as Entry).Unfocus();
                DependencyService.Get<IToast>().LongToast("Text field cannot be empty");
            }
            else
            {
                (sender as Entry).Unfocus();
                DependencyService.Get<IToast>().LongToast("File name not changed");
            }

        }
        
    }
}