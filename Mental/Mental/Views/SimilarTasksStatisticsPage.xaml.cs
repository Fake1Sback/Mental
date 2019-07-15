using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models;
using Mental.Models.DbModels;
using Mental.ViewModels;

namespace Mental.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SimilarTasksStatisticsPage : ContentPage
	{
        public SimilarTasksStatisticsPage(DbMathTask _dbMathTask, bool _save)
        {
            InitializeComponent();
            this.BindingContext = new SimilarMathTasksStatisticsVM(this.Navigation, _dbMathTask, _save);
        }
    }
}