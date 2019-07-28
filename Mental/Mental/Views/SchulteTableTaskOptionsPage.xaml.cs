using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;
using Mental.Models;

namespace Mental.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchulteTableTaskOptionsPage : ContentPage
	{
		public SchulteTableTaskOptionsPage ()
		{
			InitializeComponent ();
            BindingContext = new SchulteTableTaskOptionsVM(this.Navigation);
		}

        public SchulteTableTaskOptionsPage(SchulteTableTaskOptions FavouriteSchulteTableTaskOptions)
        {
            InitializeComponent();
            BindingContext = new SchulteTableTaskOptionsVM(this.Navigation, FavouriteSchulteTableTaskOptions);
        }

        private void GridSizeExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            GridSizeFrame.IsVisible = !GridSizeFrame.IsVisible;
        }

        private void ModeExpandMoreButton_Clicked(object sender, EventArgs e)
        {
            ModeFrame.IsVisible = !ModeFrame.IsVisible;
        }
    }
}