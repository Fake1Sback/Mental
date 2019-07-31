using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models;

namespace Mental.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AltStartingPage : CarouselPage
    {
        public AltStartingPage()
        {
            InitializeComponent();
        }

        private async void MathTaskFavourite(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FavouriteSetupsPage(Favourites.MathOptionsFavourite));
        }
        private async void MathTaskCustomize(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MathTasksOptionsPage());
        }
        private async void MathTaskGeneralStatistics(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GeneralStatisticsPage());
        }
        private async void SchulteTableFavourite(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FavouriteSetupsPage(Favourites.SchulteTableOptionsFavourite));
        }
        private async void SchulteTableCustomize(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SchulteTableTaskOptionsPage());
        }
        private async void SchulteTableGeneralStatistics(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SchulteTableTasksGeneralStatisticsPage());
        }    
        private async void StroopTaskFavourite(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FavouriteSetupsPage(Favourites.StroopOptionsFavourite));
        }
        private async void StroopTaskCutomize(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StroopTaskOptionsPage());
        }
        private async void StroopTaskGeneralStatistics(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StroopTaskGeneralStatisticsPage());
        }    
    }
}