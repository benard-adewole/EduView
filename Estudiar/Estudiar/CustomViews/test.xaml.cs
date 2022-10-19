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
    public partial class test : ContentView
    {
        public test()
        {
            InitializeComponent();
        }
        private async void closePage_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}