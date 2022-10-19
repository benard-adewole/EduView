using Estudiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Estudiar.Controls
{

    public class courseSearchHandler:SearchHandler
    {
        public static readonly BindableProperty GetCoursesProperty = BindableProperty.Create(
            propertyName: nameof(GetCourses),
            returnType: typeof(List<Course>),
            declaringType: typeof(List<Course>),
            defaultValue: null);
        public List<Course> GetCourses
        {
            get { return (List<Course>)GetValue(GetCoursesProperty); }
            set { SetValue(GetCoursesProperty, value); }
        }
        //public IList<Course> GetCourses { get; set; }
        public Type SelectedItemNavigationTarget { get; set; }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            if (string.IsNullOrWhiteSpace(newValue))
            {
                ItemsSource = null;
            }
            else
            {
                if (GetCourses == null)
                {
                    return;
                }
                ItemsSource = GetCourses
                    .Where(course => course.code.ToLower().Contains(newValue.ToLower()))
                    .ToList<Course>();
            }
        }

        protected override async void OnItemSelected(object item)
        {
            base.OnItemSelected(item);

            // Let the animation complete
            await Task.Delay(1000);

            ShellNavigationState state = (App.Current.MainPage as Shell).CurrentState;
            // The following route works because route names are unique in this application.
            await Shell.Current.GoToAsync($"{GetNavigationTarget()}?name={((Course)item).code}");
        }

        string GetNavigationTarget()
        {
            return null;//(Shell.Current as AppShell).Routes.FirstOrDefault(route => route.Value.Equals(SelectedItemNavigationTarget)).Key;
        }
    }
}
