using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.ViewModels;
using Mental.Models;

namespace Mental.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MathTasksOptionsPage : ContentPage
    {
        public MathTasksOptionsPage()
        {
            BindingContext = new MathTasksOptionsVM(this.Navigation);
            InitializeComponent();
        }

        private void InitializeSliders()
        {
            App app = (App)App.Current;
            MathTasksOptions mathTasksOptions = app.GetStoredMathTaskOptions();
            SpecialModeXDigitSlider.Value = mathTasksOptions.AmountOfXDigits;
            DigitsAfterDotSignSlider.Value = mathTasksOptions.DigitsAfterDotSign;
            TimerStartValueSlider.Value = mathTasksOptions.AmountOfMinutes;
        }
    }
}