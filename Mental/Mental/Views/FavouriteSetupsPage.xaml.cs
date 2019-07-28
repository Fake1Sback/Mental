using Mental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;
using System.Diagnostics;

namespace Mental.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteSetupsPage : ContentPage
    {
        public FavouriteSetupsPage(Favourites favouriteType)
        {
            InitializeComponent();
            this.BindingContext = new FavouriteSetupsVM(favouriteType,this.Navigation);
        }
    }
}