using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models;
using Xamarin.Forms;

namespace Mental.ViewModels.PartialViewModels
{
    public class RestrictionsPVM : INotifyPropertyChanged
    {
        private TaskRestrictions taskRestrictions;
        private MathTasksOptions mathTaskOptions;

        public RestrictionsPVM(MathTasksOptions _mathTasksOptions)
        {
            mathTaskOptions = _mathTasksOptions;
            taskRestrictions = mathTaskOptions.restrictions;
        }

        private double _PlusDigit1Restriction;
        private double _PlusDigit2Restriction;
        private double _MinusDigit1Restriction;
        private double _MinusDigit2Restriction;
        private double _MultiplyDigit1Restriction;
        private double _MultiplyDigit2Restriction;
        private double _DivideDigit1Restriction;
        private double _DivideDigit2Restriction;

        private double _PlusDigit1SliderPercentage;
        private double _PlusDigit2SliderPercentage;
        private double _MinusDigit1SliderPercentage;
        private double _MinusDigit2SliderPercentage;
        private double _MultiplyDigit1SliderPercentage;
        private double _MultiplyDigit2SliderPercentage;
        private double _DivideDigit1SliderPercentage;
        private double _DivideDigit2SliderPercentage;

        public bool RestrictionLayoutVisibility
        {
            get
            {
                return mathTaskOptions.IsRestrictionsActivated;
            }
            private set { }
        }

        public string RestrictionsVisibilityText
        {
            get
            {
                if (mathTaskOptions.IsRestrictionsActivated)
                    return "+";
                else
                    return "-";
            }
            private set { }
        }

        public Command ChangeRestrictionVisibilityCommand
        {
            get
            {
                return new Command(() =>
                         {
                             mathTaskOptions.IsRestrictionsActivated = !mathTaskOptions.IsRestrictionsActivated;
                             OnPropertyChanged("RestrictionLayoutVisibility");
                             OnPropertyChanged("RestrictionsVisibilityText");
                         });
            }
            set { }
        }

        public double PlusDigit1Restriction
        {
            get
            {
                return _PlusDigit1Restriction;
            }
            set
            {
                _PlusDigit1Restriction = value;
                taskRestrictions.restrictions[0].Digit1Restriction = (int)Math.Round(value,0);
                _PlusDigit1SliderPercentage = GetSliderPercentage(PlusDigit1Restriction);
                OnPropertyChanged("PlusDigit1Restriction");
                OnPropertyChanged("IntPlusDigit1Restriction");
            }
        }

        public double PlusDigit2Restriction
        {
            get
            {
                return _PlusDigit2Restriction;
            }
            set
            {
                _PlusDigit2Restriction = value;
                taskRestrictions.restrictions[0].Digit2Restriction = (int)Math.Round(value, 0);
                _PlusDigit2SliderPercentage = GetSliderPercentage(PlusDigit2Restriction);
                OnPropertyChanged("PlusDigit2Restriction");
                OnPropertyChanged("IntPlusDigit2Restriction");
            }
        }

        public double MinusDigit1Restriction
        {
            get
            {
                return _MinusDigit1Restriction;
            }
            set
            {
                _MinusDigit1Restriction = value;
                taskRestrictions.restrictions[1].Digit1Restriction = (int)Math.Round(value, 0);
                _MinusDigit1SliderPercentage = GetSliderPercentage(MinusDigit1Restriction);
                OnPropertyChanged("MinusDigit1Restriction");
                OnPropertyChanged("IntMinusDigit1Restriction");
            }
        }
        
        public double MinusDigit2Restriction
        {
            get
            {
                return _MinusDigit2Restriction;
            }
            set
            {
                _MinusDigit2Restriction = value;
                taskRestrictions.restrictions[1].Digit2Restriction = (int)Math.Round(value, 0);
                _MinusDigit2SliderPercentage = GetSliderPercentage(MinusDigit2Restriction);
                OnPropertyChanged("MinusDigit2Restriction");
                OnPropertyChanged("IntMinusDigit2Restriction");
            }
        }

        public double MultiplyDigit1Restriction
        {
            get
            {
                return _MultiplyDigit1Restriction;
            }
            set
            {
                _MultiplyDigit1Restriction = value;
                taskRestrictions.restrictions[2].Digit1Restriction = (int)Math.Round(value, 0);
                _MultiplyDigit1SliderPercentage = GetSliderPercentage(MultiplyDigit1Restriction);
                OnPropertyChanged("MultiplyDigit1Restriction");
                OnPropertyChanged("IntMultiplyDigit1Restriction");
            }
        }

        public double MultiplyDigit2Restriction
        {
            get
            {
                return _MultiplyDigit2Restriction;
            }
            set
            {
                _MultiplyDigit2Restriction = value;
                taskRestrictions.restrictions[2].Digit2Restriction = (int)Math.Round(value, 0);
                _MultiplyDigit2SliderPercentage = GetSliderPercentage(MultiplyDigit2Restriction);
                OnPropertyChanged("MultiplyDigit2Restriction");
                OnPropertyChanged("IntMultiplyDigit2Restriction");
            }
        }

        public double DivideDigit1Restriction
        {
            get
            {
                return _DivideDigit1Restriction;
            }
            set
            {
                _DivideDigit1Restriction = value;
                taskRestrictions.restrictions[3].Digit1Restriction = (int)Math.Round(value, 0);
                _DivideDigit1SliderPercentage = GetSliderPercentage(DivideDigit1Restriction);
                OnPropertyChanged("DivideDigit1Restriction");
                OnPropertyChanged("IntDivideDigit1Restriction");
            }
        }

        public double DivideDigit2Restriction
        {
            get
            {
                return _DivideDigit2Restriction;
            }
            set
            {
                _DivideDigit2Restriction = value;
                taskRestrictions.restrictions[3].Digit2Restriction = (int)Math.Round(value, 0);
                _DivideDigit2SliderPercentage = GetSliderPercentage(DivideDigit2Restriction);
                OnPropertyChanged("DivideDigit2Restriction");
                OnPropertyChanged("IntDivideDigit2Restriction");
            }
        }

        //--------------------------------------------

        public int IntPlusDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[0].Digit1Restriction;
            }
            private set { }
        }

        public int IntPlusDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[0].Digit2Restriction;
            }
            private set { }
        }

        public int IntMinusDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[1].Digit1Restriction;
            }
            private set { }
        }

        public int IntMinusDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[1].Digit2Restriction;
            }
            private set { }
        }

        public int IntMultiplyDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[2].Digit1Restriction;
            }
            private set { }
        }

        public int IntMultiplyDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[2].Digit2Restriction;
            }
            private set { }
        }

        public int IntDivideDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[3].Digit1Restriction;
            }
            private set { }
        }

        public int IntDivideDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[3].Digit2Restriction;
            }
            private set { }
        }

        //---------------------------------------------------------------

        public int MinimumDigitValue
        {
            get
            {
                if (FindAmountOfDigits(mathTaskOptions.MinValue) == FindAmountOfDigits(mathTaskOptions.MaxValue))
                {
                    mathTaskOptions.IsRestrictionsActivated = false;
                    OnPropertyChanged("RestrictionLayoutVisibility");
                    OnPropertyChanged("RestrictionsVisibilityText");
                    return mathTaskOptions.MinValue;
                }
                return FindAmountOfDigits(mathTaskOptions.MinValue);
            }
            private set { }
        }

        public int MaximumDigitValue
        {
            get
            {
                if (FindAmountOfDigits(mathTaskOptions.MaxValue) == FindAmountOfDigits(mathTaskOptions.MinValue))
                {
                    mathTaskOptions.IsRestrictionsActivated = false;
                    OnPropertyChanged("RestrictionLayoutVisibility");
                    OnPropertyChanged("RestrictionsVisibilityText");
                    return mathTaskOptions.MinValue + 1;                   
                }
                else
                    return FindAmountOfDigits(mathTaskOptions.MaxValue);
            }
            private set { }
        }

        private int FindAmountOfDigits(int Value)
        {
            int AmountOfDigits = 0;
            if (Value < 0)
                Value = Value * -1;
            while(Value >= 0)
            {
                Value = Value / 10;
                AmountOfDigits += 1;
                if (Value == 0)
                    break;
            }
            return AmountOfDigits;
        }

        //---------------------------------------------------------------

        private double GetSliderPercentage(double CurrentSliderValue)
        {
            CurrentSliderValue = CurrentSliderValue - FindAmountOfDigits(mathTaskOptions.MinValue);
            int MaximumPercentageValue = FindAmountOfDigits(mathTaskOptions.MaxValue) - FindAmountOfDigits(mathTaskOptions.MinValue);

            return CurrentSliderValue * 100 / MaximumPercentageValue;
        }

        private double GetValueFromSliderPercentage(double CurrentSliderPercentage)
        {
            int MaximumPercentageValue = FindAmountOfDigits(mathTaskOptions.MaxValue) - FindAmountOfDigits(mathTaskOptions.MinValue);
            return CurrentSliderPercentage * MaximumPercentageValue / 100 + FindAmountOfDigits(mathTaskOptions.MinValue);
        }

        public void RenewSliderValues()
        {
            _PlusDigit1Restriction = GetValueFromSliderPercentage(_PlusDigit1SliderPercentage);
            _PlusDigit2Restriction = GetValueFromSliderPercentage(_PlusDigit2SliderPercentage);
            _MinusDigit1Restriction = GetValueFromSliderPercentage(_MinusDigit1SliderPercentage);
            _MinusDigit2Restriction = GetValueFromSliderPercentage(_MinusDigit2SliderPercentage);
            _MultiplyDigit1Restriction = GetValueFromSliderPercentage(_MultiplyDigit1SliderPercentage);
            _MultiplyDigit2Restriction = GetValueFromSliderPercentage(_MultiplyDigit2SliderPercentage);
            _DivideDigit1Restriction = GetValueFromSliderPercentage(_DivideDigit1SliderPercentage);
            _DivideDigit2Restriction = GetValueFromSliderPercentage(_DivideDigit2SliderPercentage);

            OnPropertyChanged("PlusDigit1Restriction");
            OnPropertyChanged("PlusDigit2Restriction");
            OnPropertyChanged("MinusDigit1Restriction");
            OnPropertyChanged("MinusDigit2Restriction");
            OnPropertyChanged("MultiplyDigit1Restriction");
            OnPropertyChanged("MultiplyDigit2Restriction");
            OnPropertyChanged("DivideDigit1Restriction");
            OnPropertyChanged("DivideDigit2Restriction");

            OnPropertyChanged("IntPlusDigit1Restriction");
            OnPropertyChanged("IntPlusDigit2Restriction");
            OnPropertyChanged("IntMinusDigit1Restriction");
            OnPropertyChanged("IntMinusDigit2Restriction");
            OnPropertyChanged("IntMultiplyDigit1Restriction");
            OnPropertyChanged("IntMultiplyDigit2Restriction");
            OnPropertyChanged("IntDivideDigit1Restriction");
            OnPropertyChanged("IntDivideDigit2Restriction");
        }

        //---------------------------------------------------------------

        public string PlusDigit1RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(0, 0);
            }
            private set { }
        }

        public string PlusDigit2RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(0, 1);
            }
            private set { }
        }

        public string MinusDigit1RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(1, 0);
            }
            private set { }
        }

        public string MinusDigit2RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(1, 1);
            }
            private set { }
        }

        public string MultiplyDigit1RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(2, 0);
            }
            private set { }
        }

        public string MultiplyDigit2RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(2, 1);
            }
            private set { }
        }

        public string DivideDigit1RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(3, 0);
            }
            private set { }
        }

        public string DivideDigit2RestrictionString
        {
            get
            {
                return CheckHardRestrictionText(3, 1);
            }
            private set { }
        }

        //----------------------------------------------------------------

        private string CheckHardRestrictionText(int OperationIndex,int HardRestricionDigitNumber)
        {
            if(HardRestricionDigitNumber == 0)
            {
                if (taskRestrictions.restrictions[OperationIndex].IsDigit1HardRestriction)
                    return "+";
                else
                    return "-";
            }
            else
            {
                if (taskRestrictions.restrictions[OperationIndex].IsDigit2HardRestriction)
                    return "+";
                else
                    return "-";
            }
        }

        public Command PlusDigit1HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[0].IsDigit1HardRestriction = !taskRestrictions.restrictions[0].IsDigit1HardRestriction; OnPropertyChanged("PlusDigit1RestrictionString"); }); } set { } }
        public Command PlusDigit2HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[0].IsDigit2HardRestriction = !taskRestrictions.restrictions[0].IsDigit2HardRestriction; OnPropertyChanged("PlusDigit2RestrictionString"); }); } set { } }
        public Command MinusDigit1HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[1].IsDigit1HardRestriction = !taskRestrictions.restrictions[1].IsDigit1HardRestriction; OnPropertyChanged("MinusDigit1RestrictionString"); }); } set { } }
        public Command MinusDigit2HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[1].IsDigit2HardRestriction = !taskRestrictions.restrictions[1].IsDigit2HardRestriction; OnPropertyChanged("MinusDigit2RestrictionString"); }); } set { } }
        public Command MultiplyDigit1HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[2].IsDigit1HardRestriction = !taskRestrictions.restrictions[2].IsDigit1HardRestriction; OnPropertyChanged("MultiplyDigit1RestrictionString"); }); } set { } }
        public Command MultiplyDigit2HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[2].IsDigit2HardRestriction = !taskRestrictions.restrictions[2].IsDigit2HardRestriction; OnPropertyChanged("MultiplyDigit2RestrictionString"); }); } set { } }
        public Command DivideDigit1HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[3].IsDigit1HardRestriction = !taskRestrictions.restrictions[3].IsDigit1HardRestriction; OnPropertyChanged("DivideDigit1RestrictionString"); }); } set { } }
        public Command DivideDigit2HardRestrictionCommand { get { return new Command(() => { taskRestrictions.restrictions[3].IsDigit2HardRestriction = !taskRestrictions.restrictions[3].IsDigit2HardRestriction; OnPropertyChanged("DivideDigit2RestrictionString"); }); } set { } }

        //----------------------------------------------------------------

        private string CheckBlockRestrictionActivatedText(int OperationIndex)
        {
            if (taskRestrictions.restrictions[OperationIndex].IsBlockActivated)
                return "+";
            else
                return "-";
        }

        public string PlusBlockActivatedString
        {
            get
            {
                return CheckBlockRestrictionActivatedText(0);
            }
            private set { }
        }
        public string MinusBlockActivatedString
        {
            get
            {
                return CheckBlockRestrictionActivatedText(1);
            }
            private set { }
        }
        public string MultiplyBlockActivatedString
        {
            get
            {
                return CheckBlockRestrictionActivatedText(2);
            }
            private set { 
}
        }
        public string DivideBlockActivatedString
        {
            get
            {
                return CheckBlockRestrictionActivatedText(3);
            }
            private set { }
        }

        public Command PlusBlockActivatedCommand { get { return new Command(() => { taskRestrictions.restrictions[0].IsBlockActivated = !taskRestrictions.restrictions[0].IsBlockActivated; OnPropertyChanged("PlusBlockActivatedString"); }); } set { } }
        public Command MinusBlockActivatedCommand { get { return new Command(() => { taskRestrictions.restrictions[1].IsBlockActivated = !taskRestrictions.restrictions[1].IsBlockActivated; OnPropertyChanged("MinusBlockActivatedString"); }); } set { } }
        public Command MultiplyBlockActivatedCommand { get { return new Command(() => { taskRestrictions.restrictions[2].IsBlockActivated = !taskRestrictions.restrictions[2].IsBlockActivated; OnPropertyChanged("MultiplyBlockActivatedString"); }); } set { } }
        public Command DivideBlockActivatedCommand { get { return new Command(() => { taskRestrictions.restrictions[3].IsBlockActivated = !taskRestrictions.restrictions[3].IsBlockActivated; OnPropertyChanged("DivideBlockActivatedString"); }); } set { } }

        //----------------------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
