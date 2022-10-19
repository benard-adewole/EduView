using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Estudiar.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Notifications : ContentPage
    {
        public Notifications()
        {
            InitializeComponent();
        }
        public Notifications(MainVM vm)
        {

        }
    }
}