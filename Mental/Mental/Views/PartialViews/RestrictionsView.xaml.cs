using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels.PartialViewModels;
using Mental.Models;

namespace Mental.Views.PartialViews
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RestrictionsView : ContentView
	{
		public RestrictionsView ()
		{
			InitializeComponent ();          
		}

        private void RestrictionsExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            RestrictionsFrame.IsVisible = !RestrictionsFrame.IsVisible;
        }
    }
}