using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;

namespace Mental.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StroopTaskOptionsPage : ContentPage
	{
		public StroopTaskOptionsPage ()
		{
			InitializeComponent ();
            this.BindingContext = new StroopTaskOptionsVM(this.Navigation); 
		}

        private void ButtonsAmountExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            ButtonsAmountFrame.IsVisible = !ButtonsAmountFrame.IsVisible;
        }

        private void StroopTaskTypeExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            StroopTaskTypeFrame.IsVisible = !StroopTaskTypeFrame.IsVisible;
        }
    }
}