using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models;
using Mental.Views;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public class MathTasksOptionsVM : INotifyPropertyChanged
    {
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;
        private App app;

        public MathTasksOptionsVM(INavigation _navigation)
        {
            navigation = _navigation;
            app = (App)App.Current;
            mathTasksOptions = app.GetStoredMathTaskOptions();

            ListOfOperationsChangedCommand = new Command(OperationButtonClick);
            ChainLengthFixedChangedCommand = new Command(ChainLengthFixedChanged);
            SpecialModeChangedCommand = new Command(SpecialModeChanged);
            NumbersTypeChangedCommand = new Command(NumberTypeChanged);
            TypeOfTaskChangedCommand = new Command(TypeOfTaskChanged);
            TimeOptionsChangedCommand = new Command(TimeOptionsChanged);
            StartCommand = new Command(Start);
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

        public int MaxChainLength
        {
            get
            {
                return mathTasksOptions.MaxChainLength;
            }
            set
            {
                mathTasksOptions.MaxChainLength = value;
                OnProperyChanged("MaxChainLength");

            }
        }

        public string SpecialModeButtonText
        {
            get
            {
                if (mathTasksOptions.IsSpecialModeActivated)
                    return "+";
                else
                    return "-";
            }
            private set { }
        }

        public bool SpecialModeLayoutVisibility
        {
            get
            {
                if (mathTasksOptions.IsSpecialModeActivated)
                    return true;
                else
                    return false;
            }
            private set { }
        }

        public int AmountOfXDigits
        {
            get
            {
                return mathTasksOptions.AmountOfXDigits;
            }
            set
            {
                mathTasksOptions.AmountOfXDigits = value;
                OnProperyChanged("AmountOfXDigits");
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

        public int DigitsAfterDotSign
        {
            get
            {
                return mathTasksOptions.DigitsAfterDotSign;
            }
            set
            {
                mathTasksOptions.DigitsAfterDotSign = value;
                OnProperyChanged("DigitsAfterDotSign");
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
            }
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

        public int AmountOfMinutes
        {
            get
            {
                return mathTasksOptions.AmountOfMinutes;
            }
            set
            {
                mathTasksOptions.AmountOfMinutes = value;
                OnProperyChanged("AmountOfMinutes");
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

        public Command SpecialModeChangedCommand { get; set; }

        private void SpecialModeChanged()
        {
            if (mathTasksOptions.IsSpecialModeActivated)
                mathTasksOptions.IsSpecialModeActivated = false;
            else
                mathTasksOptions.IsSpecialModeActivated = true;
            OnProperyChanged("SpecialModeButtonText");
            OnProperyChanged("SpecialModeLayoutVisibility");
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
            OnProperyChanged("CountdownTimeOptionsLayoutVisibility");
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
            OnProperyChanged("CountdownTimeOptionButtonColor");
            OnProperyChanged("FixedAmountOfOperationsTimeOptionButtonColor");
            OnProperyChanged("CountdownTimeOptionsLayoutVisibility");
            OnProperyChanged("FixedAmountOfOperationsLayoutVisibility");
        }

        public Command StartCommand { get; set; }

        private async void Start()
        {
            app.SaveMathTaskOptions(mathTasksOptions);

            ITimeOption timeOption = null;
            if (mathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
                timeOption = new CountdownTimeOption(mathTasksOptions);
            else if (mathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
            {
                timeOption = new LimitedTasksTimeOption(mathTasksOptions);
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
