using Estudiar.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Estudiar.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Payment : ContentPage
	{
		public ICommand BackCommand
		{
			get
			{
				return new Command(async () =>
				{
					MainVM vm = new MainVM();

					await vm.UpdateSubscription();
					await Shell.Current.Navigation.PopAsync();
				});
			}
			private set { }
		}
		public Payment (string url)
		{
			InitializeComponent ();
			paymentPortal.Source = url;
			Shell.SetBackButtonBehavior(this, new BackButtonBehavior
			{
				Command = BackCommand
			}) ;
		}
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
			
		}
        protected override bool OnBackButtonPressed()
        {
			//vm.UpdateSucriptionStatus.Execute(null);
			BackCommand.Execute(null);
			return true;
        }
       
    }
}