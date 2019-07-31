using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models;
using Mental.Views;
using Xamarin.Forms;
using Mental.ViewModels.PartialViewModels;
using Mental.Services;
using Mental.Models.DbModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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


        private bool _InfoVisibility = false;
        private string _InfoCaption = string.Empty;
        private string _InfoText = string.Empty;

        private Color ActiveColor = Color.FromHex("#6699ff");

        private Color DefaultOptionButtonBackgroundColor = Color.FromHex("#80aaff");
        private Color ActiveOptionButtonBackgroundColor = Color.FromHex("#99ffcc");

        public MathTasksOptionsVM(INavigation _navigation)
        {
            navigation = _navigation;
            app = (App)App.Current;
            mathTasksOptions = app.GetStoredMathTaskOptions();
            Initialize();
        }

        public MathTasksOptionsVM(INavigation _navigation, MathTasksOptions FavouriteMathTaskOptions)
        {
            navigation = _navigation;
            app = (App)App.Current;
            mathTasksOptions = FavouriteMathTaskOptions;
            Initialize();
        }

        private void Initialize()
        {
            _restPVM = new RestrictionsPVM(mathTasksOptions);
            _TimeOptionsPVM = new TimeOptionsPVM(mathTasksOptions.TaskTimeOptions);

            _SliderMaxChainLengthValue = mathTasksOptions.MaxChainLength;
            _DigitsAfterDotSignSliderValue = mathTasksOptions.DigitsAfterDotSign;

            ChainLengthFixedChangedCommand = new Command(ChainLengthFixedChanged);
            NumbersTypeChangedCommand = new Command(NumberTypeChanged);
            TypeOfTaskChangedCommand = new Command(TypeOfTaskChanged);
            StartCommand = new Command(Start);
        }


        //--------------------------------------------------------
        public bool InfoVisibility
        {
            get
            {
                return _InfoVisibility;
            }
            set
            {
                _InfoVisibility = value;
                OnPropertyChanged("InfoVisibility");
            }
        }

        public string InfoCaption
        {
            get
            {
                return _InfoCaption;
            }
            set
            {
                _InfoCaption = value;
                OnPropertyChanged("InfoCaption");
            }
        }

        public string InfoText
        {
            get
            {
                return _InfoText;
            }
            set
            {
                _InfoText = value;
                OnPropertyChanged("InfoText");
            }
        }

        public Command<string> ShowInfoCommand
        {
            get
            {
                return new Command<string>((str) =>
                {
                    InfoVisibility = true;
                    InfoCaption = OptionsInfoDictionary.GetCaption(str);
                    InfoText = OptionsInfoDictionary.GetInfoText(str);
                });
            }
        }

        public Command HideInfoCommand
        {
            get
            {
                return new Command(() =>
                {
                    InfoVisibility = false;
                });
            }
        }

        //---------------------------------------------------

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
   
        public Command ListOfOperationsChangedCommand
        {
            get
            {
                return new Command<string>((str) =>
                {
                    if (mathTasksOptions.Operations.Contains(str))
                    {
                        if (mathTasksOptions.Operations.Count > 1)
                            mathTasksOptions.Operations.Remove(str);
                    }
                    else
                        mathTasksOptions.Operations.Add(str);

                    switch (str)
                    {
                        case "+":
                            OnPropertyChanged("PlusButtonColor");
                            break;
                        case "-":
                            OnPropertyChanged("MinusButtonColor");
                            break;
                        case "*":
                            OnPropertyChanged("MultiplyButtonColor");
                            break;
                        case "/":
                            OnPropertyChanged("DivideButtonColor");
                            break;
                    }

                    RestPVM.RenewRestrictionsBlockVisibility();
                });
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

        public Command AddToFavouriteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    using(var db = new ApplicationContext("mental.db"))
                    {
                        int FavouritesCount = await db.FavouriteMathTaskOptions.Where(o => o.IsLatestTaskOption == false).CountAsync();
                        if (FavouritesCount >= 10)
                        {
                            InfoVisibility = true;
                            InfoCaption = OptionsInfoDictionary.GetCaption("FavouriteRecordsLimitation");
                            InfoText = OptionsInfoDictionary.GetInfoText("FavouriteRecordsLimitation");
                        }
                        else
                        {
                            DbMathTaskOptions options = mathTasksOptions.ToDbMathTaskOptions();
                            options.IsLatestTaskOption = false;
                            await db.FavouriteMathTaskOptions.AddAsync(options);
                            await db.SaveChangesAsync();
                        }
                    }
                    MessagingCenter.Send<BaseVM>(this, "UpdateMathTaskOptions");
                });
            }
        }
    }
}
