using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Mental.Models;
using Mental.Views;

namespace Mental.ViewModels
{
    public class SchulteTableTaskOptionsVM : INotifyPropertyChanged
    {
        private INavigation navigation;
        private SchulteTableTaskOptions SchulteTableTaskOptions;
        private App app;

        public SchulteTableTaskOptionsVM(INavigation _navigation)
        {
            app = (App)App.Current;
            navigation = _navigation;
            SchulteTableTaskOptions = app.GetStoredSchulteTableTaskOptions();
            TimeOptionsChangedCommand = new Command(TimeOptionsChanged);
            GridSizeSliderValue = SchulteTableTaskOptions.GridSize;
        }

        private double _GridSizeSliderValue;
        private double _TimerCountdownSliderValue;
        private double _LastAnswerSliderValue;

        public double GridSizeSliderValue
        {
            get
            {
                return _GridSizeSliderValue;
            }
            set
            {
                _GridSizeSliderValue = value;
                SchulteTableTaskOptions.GridSize = (int)Math.Round(_GridSizeSliderValue,0);
                if (SchulteTableTaskOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                    AmountOfTasks = (int)Math.Pow(IntGridSizeSliderValue, 2);
                OnPropertyChanged("GridSizeSliderValue");
                OnPropertyChanged("IntGridSizeSliderValue");
            }
        }

        public int IntGridSizeSliderValue
        {
            get
            {
                return SchulteTableTaskOptions.GridSize;
            }
        }

        public bool EasyModeActivated
        {
            get
            {
                return SchulteTableTaskOptions.IsEasyModeActivated;
            }
        }

        public string EasyModeActivatedString
        {
            get
            {
                if (SchulteTableTaskOptions.IsEasyModeActivated)
                    return "+";
                else
                    return "-";
            }
        }

        public Command EasyModeActivatedSwitchedCommand
        {
            get
            {
                return new Command(() =>
                {
                    SchulteTableTaskOptions.IsEasyModeActivated = !SchulteTableTaskOptions.IsEasyModeActivated;
                    OnPropertyChanged("EasyModeActivated");
                    OnPropertyChanged("EasyModeActivatedString");
                });
            }
        }

        public Color CountdownTimeOptionButtonColor
        {
            get
            {
                if (SchulteTableTaskOptions.TimeOptions == TimeOptions.CountdownTimer)
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
                if (SchulteTableTaskOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
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
                if (SchulteTableTaskOptions.TimeOptions == TimeOptions.LastTask)
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
                if (SchulteTableTaskOptions.TimeOptions == TimeOptions.CountdownTimer)
                    return true;
                else
                    return false;
            }
            private set { }
        }

        public bool FixedAmountOfOperationsLayoutVisibility
        {
            get
            {
                if (SchulteTableTaskOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                    return true;
                else
                    return false;
            }
            private set { }
        }

        public bool LastTaskLayoutVisibility
        {
            get
            {
                if (SchulteTableTaskOptions.TimeOptions == TimeOptions.LastTask)
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
                return SchulteTableTaskOptions.AmountOfMinutes;
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
                SchulteTableTaskOptions.AmountOfMinutes = (int)value;
                OnPropertyChanged("AmountOfMinutes");
                OnPropertyChanged("IntAmountOfMinutes");
            }
        }
      
        public int AmountOfTasks
        {
            get
            {
                return SchulteTableTaskOptions.AmountOfTasks;
            }
            set
            {
                SchulteTableTaskOptions.AmountOfTasks = value;
                OnPropertyChanged("AmountOfTasks");
            }
        }
     
        public int IntAmountOfSecondsForAnswer
        {
            get
            {
                return SchulteTableTaskOptions.AmountOfSecondsForAnswer;
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
                SchulteTableTaskOptions.AmountOfSecondsForAnswer = (int)value;
                OnPropertyChanged("AmountOfSecondsForAnswer");
                OnPropertyChanged("IntAmountOfSecondsForAnswer");
            }
        }

        public Command TimeOptionsChangedCommand { get; set; }

        private void TimeOptionsChanged(object obj)
        {
            Button button = obj as Button;
            if (button.Text == "Countdown")
            {
                SchulteTableTaskOptions.TimeOptions = TimeOptions.CountdownTimer;
            }
            else if (button.Text == "Limited Tasks")
            {
                SchulteTableTaskOptions.TimeOptions = TimeOptions.FixedAmountOfOperations;
            }
            else if (button.Text == "Last Task")
            {
                SchulteTableTaskOptions.TimeOptions = TimeOptions.LastTask;
            }
            OnPropertyChanged("CountdownTimeOptionButtonColor");
            OnPropertyChanged("FixedAmountOfOperationsTimeOptionButtonColor");
            OnPropertyChanged("LastTaskTimeOptionButtonColor");
            OnPropertyChanged("CountdownTimeOptionsLayoutVisibility");
            OnPropertyChanged("FixedAmountOfOperationsLayoutVisibility");
            OnPropertyChanged("LastTaskLayoutVisibility");

            OnPropertyChanged("IntAmountOfSecondsForAnswer");
            OnPropertyChanged(" AmountOfTasks");
            OnPropertyChanged("IntAmountOfMinutes");
        }

        public Command StartCommand
        {
            get
            {
                return new Command(async ()=> {
                    app.SaveLatestSchulteTableTaskOptions(SchulteTableTaskOptions);
                    ITimeOption timeOption;
                    if (SchulteTableTaskOptions.TimeOptions == TimeOptions.CountdownTimer)
                    {
                        timeOption = new CountdownTimeOption(SchulteTableTaskOptions);
                    }
                    else if (SchulteTableTaskOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                    {
                        timeOption = new LimitedTasksTimeOption(SchulteTableTaskOptions);
                    }
                    else
                    {
                        timeOption = new LastTaskTimeOption(SchulteTableTaskOptions);
                    }

                    await navigation.PushAsync(new SchulteTableTaskPage(SchulteTableTaskOptions,timeOption));
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
