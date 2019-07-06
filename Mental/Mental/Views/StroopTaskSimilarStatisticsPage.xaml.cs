using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;
using Mental.Models.DbModels;

namespace Mental.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StroopTaskSimilarStatisticsPage : ContentPage
	{
		public StroopTaskSimilarStatisticsPage (DbStroopTask _dbStroopTask,bool _Save)
		{
			InitializeComponent ();
            this.BindingContext = new StroopTaskSimilarStatisticsVM(this.Navigation, _dbStroopTask, _Save);
		}
	}
}