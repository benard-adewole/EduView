using Estudiar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.CGPA
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultInfoPopUp : Popup
    {
        public ResultInfoPopUp(universitygrade universitygrade, string description)
        {
            InitializeComponent();
            Title.Text = "Semester " + universitygrade.semester.ToString();
            Description.Text = description;
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            this.Dismiss(null);
        }
    }
}