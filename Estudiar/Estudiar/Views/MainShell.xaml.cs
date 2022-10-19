using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainShell : Shell
    {
        public MainShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.PDFViewer), typeof(Views.PDFViewer));
            Routing.RegisterRoute($"{nameof(Views.CourseInfo)}", typeof(Views.CourseInfo));
            Routing.RegisterRoute($"{nameof(Views.UploadPage)}", typeof(Views.UploadPage));
            Routing.RegisterRoute($"{nameof(Views.Payment)}", typeof(Views.Payment));
            Routing.RegisterRoute($"{nameof(Views.BPDFview)}", typeof(Views.BPDFview));

            Routing.RegisterRoute(nameof(Views.FacultySelect), typeof(Views.FacultySelect));
            Routing.RegisterRoute(nameof(Views.DepartmentSelect), typeof(Views.DepartmentSelect));
        }
        public async void RestoreApp()
        {
            try
            {
                string current = Preferences.Get("AppNavStack", "");
                await Shell.Current.GoToAsync(current);
            }
            catch (Exception)
            {
                //could not load last saved page
            }
        }
        protected async override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);
            if (args != null && args.Current != null && args.Current.Location != null)
            {
                Preferences.Set("AppNavStackTemp", args.Current.Location.OriginalString);
                Preferences.Set("Shell", "Main");
                if (args.Current.Location.OriginalString != $"//{nameof(Views.Courses)}")
                {
                    Preferences.Set("Resume", false);
                }

            }
        }
    }
}