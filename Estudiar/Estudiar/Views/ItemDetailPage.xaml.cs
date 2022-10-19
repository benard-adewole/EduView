using Estudiar.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Estudiar.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}