using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models;
using Xamarin.Forms;
using Mental.ViewModels.PartialViewModels;
using Mental.Views;
using Mental.Services;

namespace Mental.ViewModels
{
    public class StroopTaskOptionsVM : BaseVM
    {
        private StroopTaskOptions StroopTaskOptions;
        private INavigation navigation;
        private App app;
        private TimeOptionsPVM _TimeOptionsPVM;

        private double _ButtonsAmountSliderValue;

        private Color DefaultOptionButtonBackgroundColor = Color.FromHex("#80aaff");
        private Color ActiveOptionButtonBackgroundColor = Color.FromHex("#99ffcc");

        private bool _InfoVisibility = false;
        private string _InfoCaption = string.Empty;
        private string _InfoText = string.Empty;

        public StroopTaskOptionsVM(INavigation _navigation)
        {
            navigation = _navigation;
            app = (App)App.Current;
            StroopTaskOptions = app.GetStoredStroopTaskOptions();
            _TimeOptionsPVM = new TimeOptionsPVM(StroopTaskOptions.TaskTimeOptionsContainer);
            _ButtonsAmountSliderValue = StroopTaskOptions.ButtonsAmount / 2;
            OnPropertyChanged("ButonsAmountSliderValue");
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

        public double ButtonsAmountSliderValue
        {
            get
            {
                return _ButtonsAmountSliderValue;
            }
            set
            {
                _ButtonsAmountSliderValue = value;
                IntButtonsAmount = ((int)Math.Round(value, 0)) * 2;
                OnPropertyChanged("ButtonsAmountSliderValue");                
            }
        }

        public int IntButtonsAmount
        {
            get
            {
                return StroopTaskOptions.ButtonsAmount;
            }
            set
            {
                StroopTaskOptions.ButtonsAmount = value;
                OnPropertyChanged("IntButtonsAmount");
            }
        }      

        public Command ColorShowingOptionsClickedCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    Button button = obj as Button;
                    if (button.Text == "Background")
                    {
                        StroopTaskOptions.ColorShowingOption = ColorShowingOptions.BackgroundColor;
                    }
                    else if (button.Text == "Border")
                    {
                        StroopTaskOptions.ColorShowingOption = ColorShowingOptions.BorderColor;
                    }
                    OnPropertyChanged("BackgroundColorShowingOptionsColor");
                    OnPropertyChanged("BorderColorShowingOptionsColor");
                });
            }
        }

        public Color BackgroundColorShowingOptionsColor
        {
            get
            {
                if (StroopTaskOptions.ColorShowingOption == ColorShowingOptions.BackgroundColor)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
        }

        public Color BorderColorShowingOptionsColor
        {
            get
            {
                if (StroopTaskOptions.ColorShowingOption == ColorShowingOptions.BorderColor)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
        }

        public Command StroopTaskTypeChangedCommand
        {
            get
            {
                return new Command((obj) =>
                {
                    Button button = obj as Button;
                    if (button.Text == "Find Correct")
                        StroopTaskOptions.StroopTaskType = StroopTaskType.FindOneCorrect;
                    else if (button.Text == "True or False")
                        StroopTaskOptions.StroopTaskType = StroopTaskType.TrueOrFalse;
                    else if (button.Text == "Find Text By Color")
                        StroopTaskOptions.StroopTaskType = StroopTaskType.FindColorByText;

                    OnPropertyChanged("FindOneCorrectColor");
                    OnPropertyChanged("TrueOrFalseColor");
                    OnPropertyChanged("FindTextColorColor");
                });
            }
        }

        public Color FindOneCorrectColor
        {
            get
            {
                if (StroopTaskOptions.StroopTaskType == StroopTaskType.FindOneCorrect)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
        }

        public Color TrueOrFalseColor
        {
            get
            {
                if (StroopTaskOptions.StroopTaskType == StroopTaskType.TrueOrFalse)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
        }

        public Color FindTextColorColor
        {
            get
            {
                if (StroopTaskOptions.StroopTaskType == StroopTaskType.FindColorByText)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
        }

        public Command StartCommand
        {
            get
            {
                return new Command(async () =>
                {
                    app.SaveLatestStroopTaskOptions(StroopTaskOptions);
                    ITimeOption timeOption = null;
                    if (StroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.CountdownTimer)
                        timeOption = new CountdownTimeOption(StroopTaskOptions.TaskTimeOptionsContainer);
                    else if (StroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                        timeOption = new LimitedTasksTimeOption(StroopTaskOptions.TaskTimeOptionsContainer);
                    else
                        timeOption = new LastTaskTimeOption(StroopTaskOptions.TaskTimeOptionsContainer);
                                                
                    await navigation.PushAsync(new StroopTaskPage(StroopTaskOptions, timeOption));
                });
            }
        }
    }
}
