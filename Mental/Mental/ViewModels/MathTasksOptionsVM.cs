using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models;
using Mental.Views;
using Xamarin.Forms;
using Mental.ViewModels.PartialViewModels;

namespace Mental.ViewModels
{
    public class MathTasksOptionsVM : INotifyPropertyChanged
    {
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;
        private App app;
        private RestrictionsPVM _restPVM;

        private double _SliderMaxChainLengthValue;
        private double _DigitsAfterDotSignSliderValue;
        private double _TimerCountdownSliderValue;
        private double _LastAnswerSliderValue;

        public MathTasksOptionsVM(INavigation _navigation)
        {
            navigation = _navigation;
            app = (App)App.Current;
            mathTasksOptions = app.GetStoredMathTaskOptions();
            _restPVM = new RestrictionsPVM(mathTasksOptions);

            _SliderMaxChainLengthValue = mathTasksOptions.MaxChainLength;
            _DigitsAfterDotSignSliderValue = mathTasksOptions.DigitsAfterDotSign;
            _TimerCountdownSliderValue = mathTasksOptions.AmountOfMinutes;
            _LastAnswerSliderValue = mathTasksOptions.AmountOfSecondsForAnswer;

            ListOfOperationsChangedCommand = new Command(OperationButtonClick);
            ChainLengthFixedChangedCommand = new Command(ChainLengthFixedChanged);
            NumbersTypeChangedCommand = new Command(NumberTypeChanged);
            TypeOfTaskChangedCommand = new Command(TypeOfTaskChanged);
            TimeOptionsChangedCommand = new Command(TimeOptionsChanged);
            StartCommand = new Command(Start);
        }

        public RestrictionsPVM RestPVM
        {
            get
            {
                return _restPVM;
            }
            set
            {
                _restPVM = value;
            }
        }

        public Color PlusButtonColor
        {
            get
            {
                if (mathTasksOptions.Operations.Contains("+"))
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }

            private set { }
        }

        public Color MinusButtonColor
        {
            get
            {
                if(mathTasksOptions.Operations.Contains("-"))
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public Color MultiplyButtonColor
        {
            get
            {
                if(mathTasksOptions.Operations.Contains("*"))
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public Color DivideButtonColor
        {
            get
            {
                if(mathTasksOptions.Operations.Contains("/"))
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public string ChainLengthButtonText
        {
            get
            {
                if (mathTasksOptions.IsChainLengthFixed)
                    return "+";
                else
                    return "-";
            }
            private set { }
        }

        public int IntMaxChainLength
        {
            get
            {
                return mathTasksOptions.MaxChainLength;
            }
            private set { }
        }

        public double MaxChainLength
        {
            get
            {
                return _SliderMaxChainLengthValue;
            }
            set
            {
                _SliderMaxChainLengthValue = value;
                mathTasksOptions.MaxChainLength = (int)_SliderMaxChainLengthValue;
                OnProperyChanged("MaxChainLength");
                OnProperyChanged("IntMaxChainLength");
            }
        }

        public Color IntegerDataTypeButtonColor
        {
            get
            {
                if (mathTasksOptions.IsIntegerNumbers)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public Color FractionalDataTypeButtonColor
        {
            get
            {
                if (mathTasksOptions.IsIntegerNumbers)
                    return Color.LightGray;
                else
                    return Color.Aqua;

            }
        }

        public bool FractionalNumbersOptionsLabeleVisibility
        {
            get
            {
                if (mathTasksOptions.IsIntegerNumbers)
                    return false;
                else
                    return true;
            }
            private set { }
        }

        public int IntDigitsAfterDotSign
        {
            get
            {
                return mathTasksOptions.DigitsAfterDotSign;
            }
            private set { }
        }

        public double DigitsAfterDotSign
        {
            get
            {
                return _DigitsAfterDotSignSliderValue;
            }
            set
            {
                _DigitsAfterDotSignSliderValue = value;
                mathTasksOptions.DigitsAfterDotSign = (int)value;
                OnProperyChanged("DigitsAfterDotSign");
                OnProperyChanged("IntDigitsAfterDotSign");
            }
        }

        public int MinValue
        {
            get
            {
                return mathTasksOptions.MinValue;
            }
            set
            {
                mathTasksOptions.MinValue = value;
                OnProperyChanged("MinValue");
                RestPVM.MinimumDigitValue = value;

              //  OnProperyChanged("RestPVM");
              //  OnProperyChanged("RestPVM.MaximumDigitValue");
              //  OnProperyChanged("RestPVM");
               // RestPVM.RenewSliderValues();
            }
        }

        public int MaxValue
        {
            get
            {
                return mathTasksOptions.MaxValue;
            }
            set
            {
                mathTasksOptions.MaxValue = value;
                OnProperyChanged("MaxValue");
                RestPVM.MaximumDigitValue = value;

             //   OnProperyChanged("RestPVM.MaximumDigitValue");
              //  OnProperyChanged("RestPVM");
              //  RestPVM.RenewSliderValues();
            }
        }

        private int FindAmountOfDigits(int Value)
        {
            int AmountOfDigits = 0;
            if (Value < 0)
                Value = Value * -1;
            while (Value >= 0)
            {
                Value = Value / 10;
                AmountOfDigits += 1;
                if (Value == 0)
                    break;
            }
            return AmountOfDigits;
        }

        public Color CountResultTaskOptionButtonColor
        {
            get
            {
                if (mathTasksOptions.TaskType == TaskType.CountResult)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public Color CountVariableOptionButtonColor
        {
            get
            {
                if (mathTasksOptions.TaskType == TaskType.CountVariable)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public Color CountdownTimeOptionButtonColor
        {
            get
            {
                if (mathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public Color FixedAmountOfOperationsTimeOptionButtonColor
        {
            get
            {
                if (mathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public Color LastTaskTimeOptionButtonColor
        {
            get
            {
                if (mathTasksOptions.TimeOptions == TimeOptions.LastTask)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
            private set { }
        }

        public bool CountdownTimeOptionsLayoutVisibility
        {
            get
            {
                if (mathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
                    return true;
                else
                    return false;
            }
            private set { }
        }

        public int IntAmountOfMinutes
        {
            get
            {
                return mathTasksOptions.AmountOfMinutes;
            }
            private set { }
        }

        public double AmountOfMinutes
        {
            get
            {
                return _TimerCountdownSliderValue;
            }
            set
            {
                _TimerCountdownSliderValue = value;
                mathTasksOptions.AmountOfMinutes = (int)value;
                OnProperyChanged("AmountOfMinutes");
                OnProperyChanged("IntAmountOfMinutes");
            }
        }

        public bool FixedAmountOfOperationsLayoutVisibility
        {
            get
            {
                if (mathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                    return true;
                else
                    return false;
            }
            private set { }
        }

        public int AmountOfTasks
        {
            get
            {
                return mathTasksOptions.AmountOfTasks;
            }
            set
            {
                mathTasksOptions.AmountOfTasks = value;
                OnProperyChanged("AmountOfTasks");
            }
        }

        public bool LastTaskLayoutVisibility
        {
            get
            {
                if (mathTasksOptions.TimeOptions == TimeOptions.LastTask)
                    return true;
                else
                    return false;
            }
            private set { }
        }

        public int IntAmountOfSecondsForAnswer
        {
            get
            {
                return mathTasksOptions.AmountOfSecondsForAnswer;
            }
            private set { }
        }

        public double AmountOfSecondsForAnswer
        {
            get
            {
                return _LastAnswerSliderValue;
            }
            set
            {
                _LastAnswerSliderValue = value;
                mathTasksOptions.AmountOfSecondsForAnswer = (int)value;
                OnProperyChanged("AmountOfSecondsForAnswer");
                OnProperyChanged("IntAmountOfSecondsForAnswer");
            }
        }


        public Command ListOfOperationsChangedCommand { get; set; }

        private void OperationButtonClick(object obj)
        {
            Button button = obj as Button;
            if (button.Text == "+")
            {
                if (mathTasksOptions.Operations.Contains("+"))
                    mathTasksOptions.Operations.Remove("+");
                else
                    mathTasksOptions.Operations.Add("+");
                OnProperyChanged("PlusButtonColor");
            }
            else if (button.Text == "-")
            {
                if (mathTasksOptions.Operations.Contains("-"))
                    mathTasksOptions.Operations.Remove("-");
                else
                    mathTasksOptions.Operations.Add("-");
                OnProperyChanged("MinusButtonColor");
            }
            else if (button.Text == "*")
            {
                if (mathTasksOptions.Operations.Contains("*"))
                    mathTasksOptions.Operations.Remove("*");
                else
                    mathTasksOptions.Operations.Add("*");
                OnProperyChanged("MultiplyButtonColor");
            }
            else if (button.Text == "/")
            {
                if (mathTasksOptions.Operations.Contains("/"))
                    mathTasksOptions.Operations.Remove("/");
                else
                    mathTasksOptions.Operations.Add("/");
                OnProperyChanged("DivideButtonColor");
            }
        }

        public Command ChainLengthFixedChangedCommand { get; set; }

        private void ChainLengthFixedChanged()
        {
            if (mathTasksOptions.IsChainLengthFixed)
                mathTasksOptions.IsChainLengthFixed = false;
            else
                mathTasksOptions.IsChainLengthFixed = true;
            OnProperyChanged("ChainLengthButtonText");
        }

        public Command NumbersTypeChangedCommand { get; set; }

        private void NumberTypeChanged(object obj)
        {
            Button button = obj as Button;
            if(button.Text == "Integer")
            {
                mathTasksOptions.IsIntegerNumbers = true;
            }
            else if(button.Text == "Fractional")
            {
                mathTasksOptions.IsIntegerNumbers = false;
            }
            OnProperyChanged("IntegerDataTypeButtonColor");
            OnProperyChanged("FractionalDataTypeButtonColor");
            OnProperyChanged("FractionalNumbersOptionsLabeleVisibility");
        }

        public Command TypeOfTaskChangedCommand { get; set; }

        private void TypeOfTaskChanged(object obj)
        {
            Button button = obj as Button;
            if(button.Text == "Count the result")
            {
                mathTasksOptions.TaskType = TaskType.CountResult;
            }
            else if(button.Text == "Count the variable")
            {
                mathTasksOptions.TaskType = TaskType.CountVariable;
            }
            OnProperyChanged("CountResultTaskOptionButtonColor");
            OnProperyChanged("CountVariableOptionButtonColor");
        }

        public Command TimeOptionsChangedCommand { get; set; }

        private void TimeOptionsChanged(object obj)
        {
            Button button = obj as Button;
            if(button.Text == "Countdown")
            {
                mathTasksOptions.TimeOptions = TimeOptions.CountdownTimer;
            }
            else if(button.Text == "Limited Tasks")
            {
                mathTasksOptions.TimeOptions = TimeOptions.FixedAmountOfOperations;
            }
            else if(button.Text == "Last Task")
            {
                mathTasksOptions.TimeOptions = TimeOptions.LastTask;
            }
            OnProperyChanged("CountdownTimeOptionButtonColor");
            OnProperyChanged("FixedAmountOfOperationsTimeOptionButtonColor");
            OnProperyChanged("LastTaskTimeOptionButtonColor");
            OnProperyChanged("CountdownTimeOptionsLayoutVisibility");
            OnProperyChanged("FixedAmountOfOperationsLayoutVisibility");
            OnProperyChanged("LastTaskLayoutVisibility");
        }

        public Command StartCommand { get; set; }

        private async void Start()
        {
            app.SaveLatestMathTaskOptions(mathTasksOptions);

            ITimeOption timeOption = null;
            if (mathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
                timeOption = new CountdownTimeOption(mathTasksOptions);
            else if (mathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
            {
                timeOption = new LimitedTasksTimeOption(mathTasksOptions);
            }
            else if(mathTasksOptions.TimeOptions == TimeOptions.LastTask)
            {
                timeOption = new LastTaskTimeOption(mathTasksOptions);
            }

            await navigation.PushAsync(new MathTasksPage(mathTasksOptions, timeOption));
        }
            
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnProperyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
