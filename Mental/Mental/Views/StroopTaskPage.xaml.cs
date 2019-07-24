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
    public partial class StroopTaskPage : ContentPage
    {
        public BaseStroopTaskVM BaseStroopTaskVM;

        public StroopTaskPage(StroopTaskOptions _stroopTaskOptions, ITimeOption _timeOption)
        {
            InitializeComponent();
            if (_stroopTaskOptions.StroopTaskType == StroopTaskType.FindOneCorrect)
            {
                BaseStroopTaskVM = new StroopFindOneCorrectTaskVM(this.Navigation, _stroopTaskOptions, _timeOption,this);
                this.Title = "Find 1 Correct";
            }
            else if (_stroopTaskOptions.StroopTaskType == StroopTaskType.TrueOrFalse)
            {
                BaseStroopTaskVM = new StroopTrueOrFalseTaskVM(this.Navigation, _stroopTaskOptions, _timeOption,this);
                this.Title = "True / False";
            }
            else if (_stroopTaskOptions.StroopTaskType == StroopTaskType.FindColorByText)
            {
                BaseStroopTaskVM = new StroopFindColorByTextTaskVM(this.Navigation, _stroopTaskOptions, _timeOption,this);
                this.Title = "Find Color by Text";
            }
            BindingContext = BaseStroopTaskVM;
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