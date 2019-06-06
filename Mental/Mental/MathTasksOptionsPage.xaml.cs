using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models;

namespace Mental
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MathTasksOptionsPage : ContentPage
    {
        private MathTasksOptions MathTasksOptions;

        public MathTasksOptionsPage()
        {
            MathTasksOptions = new MathTasksOptions() { TaskType = TaskType.CountResult, TimeOptions = TimeOptions.CountdownTimer };       
            InitializeComponent();
            LimitedTasksOptions.IsVisible = false;
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
                await Navigation.PushAsync(new CountResultTaskView(MathTasksOptions, timeOption));
            else
                await Navigation.PushAsync(new CountVariableTaskView(MathTasksOptions, timeOption));      
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
            }
        }

        private void SpecialModeButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text == "-")
            {
                button.Text = "+";
                SpecialModeLayout.IsVisible = true;
                MathTasksOptions.IsSpecialModeActivated = true;
            }
            else if(button.Text == "+")
            {
                button.Text = "-";
                SpecialModeLayout.IsVisible = false;
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