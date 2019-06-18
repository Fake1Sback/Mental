using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models;
using Mental.ViewModels;

namespace Mental.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MathTasksPage : ContentPage
	{
		public MathTasksPage (MathTasksOptions _mathTasksOptions,ITimeOption _timeOption)
		{
            if (_mathTasksOptions.TaskType == TaskType.CountResult && _mathTasksOptions.IsIntegerNumbers)
                BindingContext = new CountResultIntVM(this.Navigation,_mathTasksOptions,_timeOption);

			InitializeComponent ();
		}
	}
}