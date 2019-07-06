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
                BaseStroopTaskVM = new StroopFindOneCorrectTaskVM(this.Navigation, _stroopTaskOptions, _timeOption);
            else if (_stroopTaskOptions.StroopTaskType == StroopTaskType.TrueOrFalse)
                BaseStroopTaskVM = new StroopTrueOrFalseTaskVM(this.Navigation, _stroopTaskOptions, _timeOption);
            else if (_stroopTaskOptions.StroopTaskType == StroopTaskType.FindColorByText)
                BaseStroopTaskVM = new StroopFindColorByTextTaskVM(this.Navigation, _stroopTaskOptions, _timeOption);
            BindingContext = BaseStroopTaskVM;
        }      
    }
}