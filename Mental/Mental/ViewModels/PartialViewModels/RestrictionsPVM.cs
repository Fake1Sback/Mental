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

            _MinimumSliderValue = FindAmountOfDigits(mathTaskOptions.MinValue);
            _MaximumSliderValue = FindAmountOfDigits(mathTaskOptions.MaxValue);

            OnPropertyChanged("MinimumDigitValue");
            OnPropertyChanged("MaximumDigitValue");

            mathTaskOptions.IsRestrictionsActivated = CheckIfRestrictionsActivated();

            PlusDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[0].Digit1Restriction) / 100;
            PlusDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[0].Digit2Restriction) / 100;
            MinusDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[1].Digit1Restriction) / 100;
            MinusDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[1].Digit2Restriction) / 100;
            MultiplyDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[2].Digit1Restriction) / 100;
            MultiplyDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[2].Digit2Restriction) / 100;
            DivideDigit1Restriction = GetSliderPercentage(taskRestrictions.restrictions[3].Digit1Restriction) / 100;
            DivideDigit2Restriction = GetSliderPercentage(taskRestrictions.restrictions[3].Digit2Restriction) / 100;

           // RenewSliderValues();
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

        public bool IsPlusDidit1HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[0].IsDigit1HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[0].IsDigit1HardRestriction = value;
                OnPropertyChanged("IsPlusDidit1HardRestriction");
                OnPropertyChanged("HardRestrictionPlusDigit1String");
            }
        }

        public string HardRestrictionPlusDigit1String
        {
            get
            {
                if (taskRestrictions.restrictions[0].IsDigit1HardRestriction)
                    return "Only";
                else
                    return "Max";
            }
        }

        public bool IsPlusDigit2HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[0].IsDigit2HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[0].IsDigit2HardRestriction = value;
                OnPropertyChanged("IsPlusDidit2HardRestriction");
                OnPropertyChanged("HardRestrictionPlusDigit2String");
            }
        }

        public string HardRestrictionPlusDigit2String
        {
            get
            {
                if (taskRestrictions.restrictions[0].IsDigit2HardRestriction)
                    return "Only";
                else
                    return "Max";
            }
        }

        public bool IsMinusDigit1HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[1].IsDigit1HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[1].IsDigit1HardRestriction = value;
                OnPropertyChanged("IsMinusDigit1HardRestriction");
                OnPropertyChanged("HardRestrictionMinusDigit1String");
            }
        }

        public string HardRestrictionMinusDigit1String
        {
            get
            {
                if (taskRestrictions.restrictions[1].IsDigit1HardRestriction)
                    return "Only";
                else
                    return "Max";
            }
        }

        public bool IsMinusDigit2HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[1].IsDigit2HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[1].IsDigit2HardRestriction = value;
                OnPropertyChanged("IsMinusDigit2HardRestriction");
                OnPropertyChanged("HardRestrictionMinusDigit2String");
            }
        }

        public string HardRestrictionMinusDigit2String
        {
            get
            {
                if (taskRestrictions.restrictions[1].IsDigit2HardRestriction)
                    return "Only";
                else
                    return "Max";
            }
        }

        public bool IsMultiplyDigit1HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[2].IsDigit1HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[2].IsDigit1HardRestriction = value;
                OnPropertyChanged("IsMultiplyDigit1HardRestriction");
                OnPropertyChanged("HardRestrictionMultiplyDigit1String");
            }
        }

        public string HardRestrictionMultiplyDigit1String
        {
            get
            {
                if (taskRestrictions.restrictions[2].IsDigit1HardRestriction)
                    return "Only";
                else
                    return "Max";
            }
        }

        public bool IsMultiplyDigit2HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[2].IsDigit2HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[2].IsDigit2HardRestriction = value;
                OnPropertyChanged("IsMultiplyDigit2HardRestriction");
                OnPropertyChanged("HardRestrictionMultiplyDigit2String");
            }
        }

        public string HardRestrictionMultiplyDigit2String
        {
            get
            {
                if (taskRestrictions.restrictions[2].IsDigit2HardRestriction)
                    return "Only";
                else
                    return "Max";
            }
        }

        public bool IsDivideDigit1HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[3].IsDigit1HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[3].IsDigit1HardRestriction = value;
                OnPropertyChanged("IsDivideDigit1HardRestriction");
                OnPropertyChanged("HardRestrictionDivideDigit1String");
            }
        }

        public string HardRestrictionDivideDigit1String
        {
            get
            {
                if (taskRestrictions.restrictions[3].IsDigit1HardRestriction)
                    return "Only";
                else
                    return "Max";
            }
        }

        public bool IsDivideDigit2HardRestriction
        {
            get
            {
                return taskRestrictions.restrictions[3].IsDigit2HardRestriction;
            }
            set
            {
                taskRestrictions.restrictions[3].IsDigit2HardRestriction = value;
                OnPropertyChanged("IsDivideDigit2HardRestriction");
                OnPropertyChanged("HardRestrictionDivideDigit2String");
            }
        }

        public string HardRestrictionDivideDigit2String
        {
            get
            {
                if (taskRestrictions.restrictions[3].IsDigit2HardRestriction)
                    return "Only";
                else
                    return "Max";
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
                _MinimumSliderValue = FindAmountOfDigits(value);
                OnPropertyChanged("MinimumDigitValue");
                mathTaskOptions.IsRestrictionsActivated = CheckIfRestrictionsActivated();
                RenewSliderValues();
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
                _MaximumSliderValue = FindAmountOfDigits(value);
                OnPropertyChanged("MaximumDigitValue");
                mathTaskOptions.IsRestrictionsActivated = CheckIfRestrictionsActivated();
                RenewSliderValues();
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
      
        //----------------------------------------------------------------

        private string CheckBlockRestrictionImgSrc(int OperationIndex)
        {
            if (taskRestrictions.restrictions[OperationIndex].IsBlockActivated)
                return "done_black_18.png";
            else
                return "";

        }

        public string PlusBlockActivatedImgSrc
        {
            get
            {
                return CheckBlockRestrictionImgSrc(0);
            }
        }

        public string MinusBlockActivatedImgSrc
        {
            get
            {
                return CheckBlockRestrictionImgSrc(1);
            }
        }

        public string MultiplyBlockActivatedImgSrc
        {
            get
            {
                return CheckBlockRestrictionImgSrc(2);
            }
        }

        public string DivideBlockActivatedImgSrc
        {
            get
            {
                return CheckBlockRestrictionImgSrc(3);
            }
        }

        private bool CheckIfRestrictionsActivated()
        {
            if (_MaximumSliderValue == _MinimumSliderValue)
                return false;

            for(int i = 0;i < taskRestrictions.restrictions.Length;i++)
            {
                if(taskRestrictions.restrictions[i].IsBlockActivated)
                    return true;
            }
            return false;
        }

        //---------------------------------------------------------------

        public void RenewRestrictionsBlockVisibility()
        {
            OnPropertyChanged("PlusBlockVivibility");
            OnPropertyChanged("MinusBlockVisibility");
            OnPropertyChanged("MultiplyBlockVisibility");
            OnPropertyChanged("DivideBlockVisibility");
        }

        public bool PlusBlockVivibility
        {
            get
            {
                return mathTaskOptions.Operations.Contains("+");
            }
        }

        public bool MinusBlockVisibility
        {
            get
            {
                return mathTaskOptions.Operations.Contains("-");
            }
        }

        public bool MultiplyBlockVisibility
        {
            get
            {
                return mathTaskOptions.Operations.Contains("*");
            }
        }

        public bool DivideBlockVisibility
        {
            get
            {
                return mathTaskOptions.Operations.Contains("/");
            }
        }

        public Command PlusBlockActivatedCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (taskRestrictions.restrictions[0].IsBlockActivated)
                        taskRestrictions.restrictions[0].IsBlockActivated = false;
                    else
                    {
                        if (mathTaskOptions.Operations.Contains("+"))
                            taskRestrictions.restrictions[0].IsBlockActivated = true;
                    }
                    mathTaskOptions.IsRestrictionsActivated = CheckIfRestrictionsActivated();
                    OnPropertyChanged("PlusBlockActivatedImgSrc");
                });
            }
            set
            { }
        }
        public Command MinusBlockActivatedCommand
        {
            get
            {
                return new Command(() =>
                {                 
                    if (taskRestrictions.restrictions[1].IsBlockActivated)
                        taskRestrictions.restrictions[1].IsBlockActivated = false;
                    else
                    {
                        if (mathTaskOptions.Operations.Contains("-"))
                            taskRestrictions.restrictions[1].IsBlockActivated = true;
                    }
                    mathTaskOptions.IsRestrictionsActivated = CheckIfRestrictionsActivated();
                    OnPropertyChanged("MinusBlockActivatedImgSrc");
                });
            }
            set
            { }
        }
        public Command MultiplyBlockActivatedCommand
        {
            get
            {
                return new Command(() =>
                {                  
                    if (taskRestrictions.restrictions[2].IsBlockActivated)
                        taskRestrictions.restrictions[2].IsBlockActivated = false;
                    else
                    {
                        if (mathTaskOptions.Operations.Contains("*"))
                            taskRestrictions.restrictions[2].IsBlockActivated = true;
                    }
                    mathTaskOptions.IsRestrictionsActivated = CheckIfRestrictionsActivated();
                    OnPropertyChanged("MultiplyBlockActivatedImgSrc");
                });
            }
            set { }
        }
        public Command DivideBlockActivatedCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (taskRestrictions.restrictions[3].IsBlockActivated)
                        taskRestrictions.restrictions[3].IsBlockActivated = false;
                    else
                    {
                        if (mathTaskOptions.Operations.Contains("/"))
                            taskRestrictions.restrictions[3].IsBlockActivated = true;
                    }
                    mathTaskOptions.IsRestrictionsActivated = CheckIfRestrictionsActivated();
                    OnPropertyChanged("DivideBlockActivatedImgSrc");
                });
            }
            set { }
        }

        //----------------------------------------------------------------
    }
}
