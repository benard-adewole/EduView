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
    public partial class SemesterOptionPopUp : Popup
    {
        public SemesterOptionPopUp()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        private void infoTapped(object sender, EventArgs e)
        {
            Dismiss("info");
        }
        private void deleteTapped(object sender, EventArgs e)
        {
            Dismiss("delete");
        }
    }
}