using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models;
using Xamarin.Forms;
using Mental.ViewModels.PartialViewModels;
using Mental.Views;

namespace Mental.ViewModels
{
    public class StroopTaskOptionsVM : BaseVM
    {
        private StroopTaskOptions StroopTaskOptions;
        private INavigation navigation;
        private App app;
        private TimeOptionsPVM _TimeOptionsPVM;

        private double _ButtonsAmountSliderValue;

        public StroopTaskOptionsVM(INavigation _navigation)
        {
            navigation = _navigation;
            app = (App)App.Current;
            StroopTaskOptions = app.GetStoredStroopTaskOptions();
            _TimeOptionsPVM = new TimeOptionsPVM(StroopTaskOptions.TaskTimeOptionsContainer);
            _ButtonsAmountSliderValue = StroopTaskOptions.ButtonsAmount / 2;
            OnPropertyChanged("ButonsAmountSliderValue");
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
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
        }

        public Color TrueOrFalseColor
        {
            get
            {
                if (StroopTaskOptions.StroopTaskType == StroopTaskType.TrueOrFalse)
                    return Color.Aqua;
                else
                    return Color.LightGray;
            }
        }

        public Color FindTextColorColor
        {
            get
            {
                if (StroopTaskOptions.StroopTaskType == StroopTaskType.FindColorByText)
                    return Color.Aqua;
                else
                    return Color.LightGray;
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
                                                
                    await navigation.PushAsync(new StroopTaskPage(StroopTaskOptions,timeOption));
                });
            }
        }
    }
}
