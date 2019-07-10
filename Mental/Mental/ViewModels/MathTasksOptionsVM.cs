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
    public class MathTasksOptionsVM : BaseVM
    {
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;
        private App app;
        private RestrictionsPVM _restPVM;
        private TimeOptionsPVM _TimeOptionsPVM;

        private double _SliderMaxChainLengthValue;
        private double _DigitsAfterDotSignSliderValue;

        private Color ActiveColor = Color.FromHex("#6699ff");

        private Color DefaultOptionButtonBackgroundColor = Color.FromHex("#80aaff");
        private Color ActiveOptionButtonBackgroundColor = Color.FromHex("#99ffcc");

        public MathTasksOptionsVM(INavigation _navigation)
        {
            navigation = _navigation;
            app = (App)App.Current;
            mathTasksOptions = app.GetStoredMathTaskOptions();
            _restPVM = new RestrictionsPVM(mathTasksOptions);
            _TimeOptionsPVM = new TimeOptionsPVM(mathTasksOptions.TaskTimeOptions);

            _SliderMaxChainLengthValue = mathTasksOptions.MaxChainLength;
            _DigitsAfterDotSignSliderValue = mathTasksOptions.DigitsAfterDotSign;

            ListOfOperationsChangedCommand = new Command(OperationButtonClick);
            ChainLengthFixedChangedCommand = new Command(ChainLengthFixedChanged);
            NumbersTypeChangedCommand = new Command(NumberTypeChanged);
            TypeOfTaskChangedCommand = new Command(TypeOfTaskChanged);
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
                OnPropertyChanged("RestPVM");
            }
        }

        public TimeOptionsPVM TimeOptionsPVM
        {
            get
            {
                return _TimeOptionsPVM;
            }
            set
            {
                _TimeOptionsPVM = value;
                OnPropertyChanged("TimeOptionsPVM");
            }
        }

        public Color PlusButtonColor
        {
            get
            {
                if (mathTasksOptions.Operations.Contains("+"))
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }

            private set { }
        }

        public Color MinusButtonColor
        {
            get
            {
                if (mathTasksOptions.Operations.Contains("-"))
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
        }

        public Color MultiplyButtonColor
        {
            get
            {
                if (mathTasksOptions.Operations.Contains("*"))
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
        }

        public Color DivideButtonColor
        {
            get
            {
                if (mathTasksOptions.Operations.Contains("/"))
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
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

        public string ChainLengthImageSrc
        {
            get
            {
                if (mathTasksOptions.IsChainLengthFixed)
                    return "done_black_18.png";
                else
                    return "";
            }
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
                OnPropertyChanged("MaxChainLength");
                OnPropertyChanged("IntMaxChainLength");
            }
        }

        public Color IntegerDataTypeButtonColor
        {
            get
            {
                if (mathTasksOptions.IsIntegerNumbers)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
        }

        public Color FractionalDataTypeButtonColor
        {
            get
            {
                if (!mathTasksOptions.IsIntegerNumbers)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;

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
                OnPropertyChanged("DigitsAfterDotSign");
                OnPropertyChanged("IntDigitsAfterDotSign");
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
                OnPropertyChanged("MinValue");
                RestPVM.MinimumDigitValue = value;
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
                OnPropertyChanged("MaxValue");
                RestPVM.MaximumDigitValue = value;
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
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
        }

        public Color CountVariableOptionButtonColor
        {
            get
            {
                if (mathTasksOptions.TaskType == TaskType.CountVariable)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
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
                OnPropertyChanged("PlusButtonColor");
            }
            else if (button.Text == "-")
            {
                if (mathTasksOptions.Operations.Contains("-"))
                    mathTasksOptions.Operations.Remove("-");
                else
                    mathTasksOptions.Operations.Add("-");
                OnPropertyChanged("MinusButtonColor");
            }
            else if (button.Text == "*")
            {
                if (mathTasksOptions.Operations.Contains("*"))
                    mathTasksOptions.Operations.Remove("*");
                else
                    mathTasksOptions.Operations.Add("*");
                OnPropertyChanged("MultiplyButtonColor");
            }
            else if (button.Text == "/")
            {
                if (mathTasksOptions.Operations.Contains("/"))
                    mathTasksOptions.Operations.Remove("/");
                else
                    mathTasksOptions.Operations.Add("/");
                OnPropertyChanged("DivideButtonColor");
            }
        }

        public Command ChainLengthFixedChangedCommand { get; set; }

        private void ChainLengthFixedChanged()
        {
            if (mathTasksOptions.IsChainLengthFixed)
                mathTasksOptions.IsChainLengthFixed = false;
            else
                mathTasksOptions.IsChainLengthFixed = true;
            OnPropertyChanged("ChainLengthButtonText");
            OnPropertyChanged("ChainLengthImageSrc");
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
            OnPropertyChanged("IntegerDataTypeButtonColor");
            OnPropertyChanged("FractionalDataTypeButtonColor");
            OnPropertyChanged("FractionalNumbersOptionsLabeleVisibility");
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
            OnPropertyChanged("CountResultTaskOptionButtonColor");
            OnPropertyChanged("CountVariableOptionButtonColor");
        }
      
        public Command StartCommand { get; set; }

        private async void Start()
        {
            app.SaveLatestMathTaskOptions(mathTasksOptions);

            ITimeOption timeOption = null;
            if (mathTasksOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.CountdownTimer)
                timeOption = new CountdownTimeOption(mathTasksOptions.TaskTimeOptions);
            else if (mathTasksOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
            {
                timeOption = new LimitedTasksTimeOption(mathTasksOptions.TaskTimeOptions);
            }
            else if(mathTasksOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.LastTask)
            {
                timeOption = new LastTaskTimeOption(mathTasksOptions.TaskTimeOptions);
            }

            await navigation.PushAsync(new MathTasksPage(mathTasksOptions, timeOption));
        }           
    }
}
