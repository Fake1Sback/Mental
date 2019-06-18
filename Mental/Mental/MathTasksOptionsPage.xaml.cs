using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models;
using Mental.Views;

namespace Mental
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MathTasksOptionsPage : ContentPage
    {
        private MathTasksOptions MathTasksOptions;
        private App app;

        public MathTasksOptionsPage()
        {
           // MathTasksOptions = new MathTasksOptions() { TaskType = TaskType.CountResult, TimeOptions = TimeOptions.CountdownTimer };
            app = (App)App.Current;
            MathTasksOptions = app.GetStoredMathTaskOptions();
            InitializeComponent();
            InitializeFields();
        }

        private void InitializeFields()
        {
            if(MathTasksOptions.Operations.Contains("+"))
                PlusButton.BackgroundColor = Color.Aqua;
            if(MathTasksOptions.Operations.Contains("-"))
                MinusButton.BackgroundColor = Color.Aqua;
            if(MathTasksOptions.Operations.Contains("*"))
                MultiplyButton.BackgroundColor = Color.Aqua;
            if(MathTasksOptions.Operations.Contains("/"))
                DivideButton.BackgroundColor = Color.Aqua;

            if (MathTasksOptions.IsChainLengthFixed)
                FixedChainLengthButton.Text = "+";
            else
                FixedChainLengthButton.Text = "-";

            MaximumChainLengthValueLabel.Text = MathTasksOptions.MaxChainLength.ToString();
            MaximumChainLengthValueSlider.Value = (double)MathTasksOptions.MaxChainLength;

            if (MathTasksOptions.IsSpecialModeActivated)
            {
                SpecialModeButton.Text = "+";
                SpecialModeLayout.IsVisible = true;
            }
            else
            {
                SpecialModeButton.Text = "-";
                SpecialModeLayout.IsVisible = false;
            }

            SpecialModeXDigitLabel.Text = MathTasksOptions.AmountOfXDigits.ToString();
            SpecialModeXDigitSlider.Value = (double)MathTasksOptions.AmountOfXDigits;

            if (MathTasksOptions.IsIntegerNumbers)
            {
                IntegerNumberTypeButton.BackgroundColor = Color.Aqua;
                FractionalNumbersOptions.IsVisible = false;
            }
            else
            {
                FractionalNumberTypeButton.BackgroundColor = Color.Aqua;
                FractionalNumbersOptions.IsVisible = true;
            }

            DigitsAfterDotSignLabel.Text = MathTasksOptions.DigitsAfterDotSign.ToString();
            DigitsAfterDotSignSlider.Value = (double)MathTasksOptions.DigitsAfterDotSign;

            MinValueEditor.Text = MathTasksOptions.MinValue.ToString();
            MaxValueEditor.Text = MathTasksOptions.MaxValue.ToString();

            if (MathTasksOptions.TaskType == TaskType.CountResult)
                CountResultButton.BackgroundColor = Color.Aqua;
            else
                CountVariableButton.BackgroundColor = Color.Aqua;

            if (MathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
            {
                CountDownTimeOptionButton.BackgroundColor = Color.Aqua;
                CountdownTimerOptions.IsVisible = true;
                TimerStartValueLabel.Text = MathTasksOptions.AmountOfMinutes.ToString();
                TimerStartValueSlider.Value = (double)MathTasksOptions.AmountOfMinutes;
            }
            else
            {
                LimitedTasksOptionButton.BackgroundColor = Color.Aqua;
                LimitedTasksOptions.IsVisible = true;
                AmountOfOperationsEditor.Text = MathTasksOptions.AmountOfTasks.ToString();
            }
        }

        private void ChainLengthFixedButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if(MathTasksOptions.IsChainLengthFixed)
            {
                button.Text = "-";
                MathTasksOptions.IsChainLengthFixed = false;
            }
            else
            {
                button.Text = "+";
                MathTasksOptions.IsChainLengthFixed = true;
            }
        }

        private void OperationButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            AddOperation(button.Text,button);
        }

        private void AddOperation(string operation, Button sender)
        {
            if (!MathTasksOptions.Operations.Contains(operation))
            {
                MathTasksOptions.Operations.Add(operation);
                sender.BackgroundColor = Color.Aqua;
            }
            else
            {
                MathTasksOptions.Operations.Remove(operation);
                sender.BackgroundColor = Color.Gray;
            }
        }

        private async void StartButtonClicked(object sender,EventArgs e)
        {
            app.SaveMathTaskOptions(MathTasksOptions);

            MathTasksOptions.MinValue = Int32.Parse(MinValueEditor.Text);
            MathTasksOptions.MaxValue = Int32.Parse(MaxValueEditor.Text);

            ITimeOption timeOption = null;
            if (MathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
                timeOption = new CountdownTimeOption(MathTasksOptions);
            else if (MathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
            {
                MathTasksOptions.AmountOfTasks = Int32.Parse(AmountOfOperationsEditor.Text);
                timeOption = new LimitedTasksTimeOption(MathTasksOptions);
            }
            
            if (MathTasksOptions.TaskType == TaskType.CountResult)
                await Navigation.PushAsync(new MathTasksPage(MathTasksOptions, timeOption));
            else
                await Navigation.PushAsync(new MathTasksPage(MathTasksOptions, timeOption));
        }

        private void MaximumChainLengthValueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            MathTasksOptions.MaxChainLength = (int)e.NewValue;
            MaximumChainLengthValueLabel.Text = MathTasksOptions.MaxChainLength.ToString();
        }

        private void TimerStartValueSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            MathTasksOptions.AmountOfMinutes = (int)e.NewValue;
            TimerStartValueLabel.Text = MathTasksOptions.AmountOfMinutes.ToString();
        }

        private void TimeOptionButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text == "Countdown")
            {
                LimitedTasksOptionButton.BackgroundColor = Color.LightGray;
                CountDownTimeOptionButton.BackgroundColor = Color.Aqua;
                CountdownTimerOptions.IsVisible = true;
                LimitedTasksOptions.IsVisible = false;
                MathTasksOptions.TimeOptions = TimeOptions.CountdownTimer;
            }
            else if(button.Text == "Limited Tasks")
            {
                CountDownTimeOptionButton.BackgroundColor = Color.LightGray;
                LimitedTasksOptionButton.BackgroundColor = Color.Aqua;
                LimitedTasksOptions.IsVisible = true;
                CountdownTimerOptions.IsVisible = false;
                MathTasksOptions.TimeOptions = TimeOptions.FixedAmountOfOperations;           
            }
        }

        private void TaskTypeButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text == "Count the result")
            {
                CountResultButton.BackgroundColor = Color.Aqua;
                CountVariableButton.BackgroundColor = Color.LightGray;
                MathTasksOptions.TaskType = TaskType.CountResult;
            }
            else if(button.Text == "Count the variable")
            {
                CountResultButton.BackgroundColor = Color.LightGray;
                CountVariableButton.BackgroundColor = Color.Aqua;
                MathTasksOptions.TaskType = TaskType.CountVariable;
            }
        }

        private void NumberTypeButtonClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text == "Integer")
            {
                FractionalNumbersOptions.IsVisible = false;
                IntegerNumberTypeButton.BackgroundColor = Color.Aqua;
                FractionalNumberTypeButton.BackgroundColor = Color.LightGray;
                MathTasksOptions.IsIntegerNumbers = true;
            }
            else if(button.Text == "Fractional")
            {
                FractionalNumbersOptions.IsVisible = true;
                IntegerNumberTypeButton.BackgroundColor =  Color.LightGray;
                FractionalNumberTypeButton.BackgroundColor = Color.Aqua;
                MathTasksOptions.IsIntegerNumbers = false;
                MathTasksOptions.DigitsAfterDotSign = (int)DigitsAfterDotSignSlider.Value;
            }
        }

        private void SpecialModeButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text == "-")
            {
                button.Text = "+";
                SpecialModeLayout.IsVisible = true;
              //  SpecialModeLayout.ForceLayout();
                MathTasksOptions.IsSpecialModeActivated = true;
                MathTasksOptions.AmountOfXDigits = (int)SpecialModeXDigitSlider.Value;
            }
            else if(button.Text == "+")
            {
                button.Text = "-";
                SpecialModeLayout.IsVisible = false;
              //  SpecialModeLayout.ForceLayout();
                MathTasksOptions.IsSpecialModeActivated = false;
            }
        }

        private void SpecialModeXDigitSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            MathTasksOptions.AmountOfXDigits = (int)e.NewValue;
            SpecialModeXDigitLabel.Text = MathTasksOptions.AmountOfXDigits.ToString();
        }

        private void DigitsAfterDotSignSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            MathTasksOptions.DigitsAfterDotSign = (int)e.NewValue;
            DigitsAfterDotSignLabel.Text = MathTasksOptions.DigitsAfterDotSign.ToString();
        }
    }
}