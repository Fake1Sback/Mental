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
        public MathTasksPage(MathTasksOptions _mathTasksOptions, ITimeOption _timeOption)
        {
            if (_mathTasksOptions.TaskType == TaskType.CountResult)
                BindingContext = new CountResultVM(this.Navigation, _mathTasksOptions, _timeOption,this);
            else if (_mathTasksOptions.TaskType == TaskType.CountVariable)
                BindingContext = new CountVariableVM(this.Navigation, _mathTasksOptions, _timeOption,this);

            InitializeComponent();

            if (_mathTasksOptions.TaskType == TaskType.CountResult)
                this.Title = "Find Result";
            else if (_mathTasksOptions.TaskType == TaskType.CountVariable)
                this.Title = "Find X";
        }

        public async void HideTaskFrame()
        {
            await TaskFrame.FadeTo(0, 750);
            TaskFrame.IsVisible = false;
            AfterTaskFrame.IsVisible = true;
            await AfterTaskFrame.FadeTo(1, 750);
        }

        public async void ShowTaskFrame()
        {
            await AfterTaskFrame.FadeTo(0, 750);
            AfterTaskFrame.IsVisible = false;
            TaskFrame.IsVisible = true;
            await TaskFrame.FadeTo(1, 750);
        }
	}
}