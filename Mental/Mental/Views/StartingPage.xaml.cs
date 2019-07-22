using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;
using Mental.Models;
using System.Diagnostics;

namespace Mental.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StartingPage : ContentPage
	{
        private StartingPageVM _StartingPageVM;

		public StartingPage ()
		{
            try
            {
                _StartingPageVM = new StartingPageVM(this.Navigation);
                this.BindingContext = _StartingPageVM;
                InitializeComponent();             
            }
            catch(Exception ex)
            {
                Debug.Write(ex.Message);
            }
		}

        private void CarouselView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            CarouselViewListItem item = e.SelectedItem as CarouselViewListItem;
            _StartingPageVM.SelectedCarouselListItem = item;
            _StartingPageVM.UpdateView();
        }
    }
}