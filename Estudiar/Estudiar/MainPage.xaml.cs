using Syncfusion.ListView.XForms;
using Syncfusion.XForms.Expander;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Estudiar
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //myImage.Source = "https://drive.google.com/uc?id=19IE7Eg2oC9oOKcG5BK7q46qf5zKWDUO8&export=download";
            //MyLazyView.LoadViewAsync();
        }
        private void listView_ScrollStateChanged(object sender, Syncfusion.ListView.XForms.ScrollStateChangedEventArgs e)
        {
            if (e.ScrollState == ScrollState.Idle)
            {
                foreach (var item in this.viewModel.ContactsInfo)
                {
                    item.Animation = AnimationEasing.Linear;
                }
            }
            else
            {
                foreach (var item in this.viewModel.ContactsInfo)
                {
                    item.Animation = AnimationEasing.None;
                }
            }
        }
    }
}
