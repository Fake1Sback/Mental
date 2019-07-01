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
	public partial class SimilarSchulteTableTasksStatisticsPage : ContentPage
	{
		public SimilarSchulteTableTasksStatisticsPage (DbSchulteTableTask _dbSchulteTableTask,bool _save)
		{
			InitializeComponent ();
            this.BindingContext = new SimilarSchulteTableTasksStatisticsVM(this.Navigation,_dbSchulteTableTask,_save);
		}
	}
}