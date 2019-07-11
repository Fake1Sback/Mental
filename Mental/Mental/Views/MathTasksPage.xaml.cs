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
            if (_mathTasksOptions.TaskType == TaskType.CountResult)
                BindingContext = new CountResultVM(this.Navigation, _mathTasksOptions, _timeOption);
            else if (_mathTasksOptions.TaskType == TaskType.CountVariable)
                BindingContext = new CountVariableVM(this.Navigation, _mathTasksOptions, _timeOption);

			InitializeComponent ();

            if (_mathTasksOptions.TaskType == TaskType.CountResult)
                this.Title = "Find Result";
            else if (_mathTasksOptions.TaskType == TaskType.CountVariable)
                this.Title = "Find X";
		}
	}
}