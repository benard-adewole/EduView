using Estudiar.ViewModels;
using Estudiar.Views;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Expander;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.CustomViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseContentView : ContentView
    {
        bool closed = false;
        CourseVM courseVM;
        public CourseContentView()
        {
            InitializeComponent();
            courseVM = this.BindingContext as CourseVM;
        }
        private async void closePage_Clicked(object sender, EventArgs e)
        {
            if (!closed)
            {
                await Shell.Current.Navigation.PopAsync();
            }
            closed = true;
        }
        private void listView_ScrollStateChanged(object sender, Syncfusion.ListView.XForms.ScrollStateChangedEventArgs e)
        {
            if (courseVM == null)
            {
                return;
            }
            
            if (e.ScrollState == ScrollState.Idle)
            {
                foreach (var item in courseVM.DbCourseStructure.types)
                {
                    item.Animation = AnimationEasing.Linear;
                }
            }
            else
            {
                foreach (var item in courseVM.DbCourseStructure.types)
                {
                    item.Animation = AnimationEasing.None;
                }
            }
        }

        
    }
}