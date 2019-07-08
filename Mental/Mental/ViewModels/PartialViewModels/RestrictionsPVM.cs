using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models;
using Xamarin.Forms;

namespace Mental.ViewModels.PartialViewModels
{
    public class RestrictionsPVM : BaseVM
    {
        private TaskRestrictions taskRestrictions;
        private MathTasksOptions mathTaskOptions;

        public RestrictionsPVM(MathTasksOptions _mathTasksOptions)
        {
            mathTaskOptions = _mathTasksOptions;
            taskRestrictions = mathTaskOptions.restrictions;
            IsRestrictionsOriginallyActivated = _mathTasksOptions.IsRestrictionsActivated;

            if(FindAmountOfDigits(mathTaskOptions.MinValue) != FindAmountOfDigits(mathTaskOptions.MaxValue))
            {
                _MinimumSliderValue = FindAmountOfDigits(mathTaskOptions.MinValue);
                _MaximumSliderValue = FindAmountOfDigits(mathTaskOptions.MaxValue);
            }
            else
            {
                _MinimumSliderValue = FindAmountOfDigits(mathTaskOptions.MinValue);
                _MaximumSliderValue = FindAmountOfDigits(mathTaskOptions.MaxValue) + 1;
                _mathTasksOptions.IsRestrictionsActivated = false;
            }

            PlusDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[0].Digit1Restriction) / 100;
            PlusDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[0].Digit2Restriction) / 100;
            MinusDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[1].Digit1Restriction) / 100;
            MinusDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[1].Digit2Restriction) / 100;
            MultiplyDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[2].Digit1Restriction) / 100;
            MultiplyDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[2].Digit2Restriction) / 100;
            DivideDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[3].Digit1Restriction) / 100;
            DivideDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[3].Digit2Restriction) / 100;
        }

        private bool IsRestrictionsOriginallyActivated = false;

        private double _PlusDigit1Restriction;
        private double _PlusDigit2Restriction;
        private double _MinusDigit1Restriction;
        private double _MinusDigit2Restriction;
        private double _MultiplyDigit1Restriction;
        private double _MultiplyDigit2Restriction;
        private double _DivideDigit1Restriction;
        private double _DivideDigit2Restriction;

        private int _MinimumSliderValue;
        private int _MaximumSliderValue;

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
                             if (FindAmountOfDigits(mathTaskOptions.MinValue) != FindAmountOfDigits(mathTaskOptions.MaxValue))
                             {
                                 mathTaskOptions.IsRestrictionsActivated = !mathTaskOptions.IsRestrictionsActivated;
                                 IsRestrictionsOriginallyActivated = !IsRestrictionsOriginallyActivated;
                                 OnPropertyChanged("RestrictionLayoutVisibility");
                                 OnPropertyChanged("RestrictionsVisibilityText");
                             }
                         });
            }
            set { }
        }

        //----------------------------------------------

        public double PlusDigit1Restriction
        {
            get
            {
                return _PlusDigit1Restriction;
            }
            set
            {
                _PlusDigit1Restriction = value;
                IntPlusDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100),0);
                OnPropertyChanged("PlusDigit1Restriction");
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
                IntPlusDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100), 0);
                OnPropertyChanged("PlusDigit2Restriction");
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
                IntMinusDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100), 0);
                OnPropertyChanged("MinusDigit1Restriction");
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
                IntMinusDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100), 0);
                OnPropertyChanged("MinusDigit2Restriction");
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
                IntMultiplyDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100), 0);
                OnPropertyChanged("MultiplyDigit1Restriction");
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
                IntMultiplyDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100), 0);
                OnPropertyChanged("MultiplyDigit2Restriction");
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
                IntDivideDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100), 0);
                OnPropertyChanged("DivideDigit1Restriction");
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
                IntDivideDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(value * 100), 0);
                OnPropertyChanged("DivideDigit2Restriction");
            }
        }

        //--------------------------------------------

        public int IntPlusDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[0].Digit1Restriction;
            }
            set
            {
                taskRestrictions.restrictions[0].Digit1Restriction = value;
                OnPropertyChanged("IntPlusDigit1Restriction");
            }
        }

        public int IntPlusDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[0].Digit2Restriction;
            }
            set
            {
                taskRestrictions.restrictions[0].Digit2Restriction = value;
                OnPropertyChanged("IntPlusDigit2Restriction");
            }
        }

        public int IntMinusDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[1].Digit1Restriction;
            }
            set
            {
                taskRestrictions.restrictions[1].Digit1Restriction = value;
                OnPropertyChanged("IntMinusDigit1Restriction");
            }
        }

        public int IntMinusDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[1].Digit2Restriction;
            }
            set
            {
                taskRestrictions.restrictions[1].Digit2Restriction = value;
                OnPropertyChanged("IntMinusDigit2Restriction");
            }
        }

        public int IntMultiplyDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[2].Digit1Restriction;
            }
            set
            {
                taskRestrictions.restrictions[2].Digit1Restriction = value;
                OnPropertyChanged("IntMultiplyDigit1Restriction");
            }
        }

        public int IntMultiplyDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[2].Digit2Restriction;
            }
            set
            {
                taskRestrictions.restrictions[2].Digit2Restriction = value;
                OnPropertyChanged("IntMultiplyDigit2Restriction");
            }
        }

        public int IntDivideDigit1Restriction
        {
            get
            {
                return taskRestrictions.restrictions[3].Digit1Restriction;
            }
            set
            {
                taskRestrictions.restrictions[3].Digit1Restriction = value;
                OnPropertyChanged("IntDivideDigit1Restriction");
            }
        }

        public int IntDivideDigit2Restriction
        {
            get
            {
                return taskRestrictions.restrictions[3].Digit2Restriction;
            }
            set
            {
                taskRestrictions.restrictions[3].Digit2Restriction = value;
                OnPropertyChanged("IntDivideDigit2Restriction");
            }
        }

        //---------------------------------------------------------------

        public int MinimumDigitValue
        {
            get
            {
                return _MinimumSliderValue;
            }
            set
            {
                if (FindAmountOfDigits(value) == FindAmountOfDigits(MaximumDigitValue))
                {
                    mathTaskOptions.IsRestrictionsActivated = false;
                    OnPropertyChanged("RestrictionLayoutVisibility");
                    OnPropertyChanged("RestrictionsVisibilityText");
                }
                else
                {
                    if(IsRestrictionsOriginallyActivated)
                    {
                        if(!mathTaskOptions.IsRestrictionsActivated)
                        {
                            mathTaskOptions.IsRestrictionsActivated = true;
                            OnPropertyChanged("RestrictionLayoutVisibility");
                            OnPropertyChanged("RestrictionsVisibilityText");
                        }
                    }
                    _MinimumSliderValue = FindAmountOfDigits(value);
                    OnPropertyChanged("MinimumSliderValue");
                    RenewSliderValues();
                }
            }
        }

        public int MaximumDigitValue
        {
            get
            {
                return _MaximumSliderValue;
            }
            set
            {
                if (FindAmountOfDigits(value) == FindAmountOfDigits(MinimumDigitValue))
                {
                    mathTaskOptions.IsRestrictionsActivated = false;
                    OnPropertyChanged("RestrictionLayoutVisibility");
                    OnPropertyChanged("RestrictionsVisibilityText");
                }
                else
                {
                    if (IsRestrictionsOriginallyActivated)
                    {
                        if (!mathTaskOptions.IsRestrictionsActivated)
                        {
                            mathTaskOptions.IsRestrictionsActivated = true;
                            OnPropertyChanged("RestrictionLayoutVisibility");
                            OnPropertyChanged("RestrictionsVisibilityText");
                        }
                        _MaximumSliderValue = FindAmountOfDigits(value);
                        OnPropertyChanged("_MaximumSliderValue");
                        RenewSliderValues();
                    }
                }
            }
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
            CurrentSliderValue = CurrentSliderValue - _MinimumSliderValue;
            int MaximumPercentageValue = _MaximumSliderValue - _MinimumSliderValue;

            return CurrentSliderValue * 100 / MaximumPercentageValue;
        }

        private double GetValueFromSliderPercentage(double CurrentSliderPercentage)
        {
            int MaximumPercentageValue = _MaximumSliderValue - _MinimumSliderValue;
            return CurrentSliderPercentage * MaximumPercentageValue / 100 + _MinimumSliderValue;
        }

        public void RenewSliderValues()
        {
            IntPlusDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(_PlusDigit1Restriction * 100), 0);
            IntPlusDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(_PlusDigit2Restriction * 100), 0);
            IntMinusDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(_MinusDigit1Restriction * 100), 0);
            IntMinusDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(_MinusDigit2Restriction * 100), 0);
            IntMultiplyDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(_MultiplyDigit1Restriction * 100), 0);
            IntMultiplyDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(_MultiplyDigit2Restriction * 100), 0);
            IntDivideDigit1Restriction = (int)Math.Round(GetValueFromSliderPercentage(_DivideDigit1Restriction * 100), 0);
            IntDivideDigit2Restriction = (int)Math.Round(GetValueFromSliderPercentage(_DivideDigit2Restriction* 100), 0);

            OnPropertyChanged("PlusDigit1Restriction");
            OnPropertyChanged("PlusDigit2Restriction");
            OnPropertyChanged("MinusDigit1Restriction");
            OnPropertyChanged("MinusDigit2Restriction");
            OnPropertyChanged("MultiplyDigit1Restriction");
            OnPropertyChanged("MultiplyDigit2Restriction");
            OnPropertyChanged("DivideDigit1Restriction");
            OnPropertyChanged("DivideDigit2Restriction");
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
    }
}
