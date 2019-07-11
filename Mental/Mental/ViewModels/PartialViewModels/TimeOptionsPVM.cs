using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Mental.Models;

namespace Mental.ViewModels.PartialViewModels
{
    public class TimeOptionsPVM : BaseVM
    {
        private TaskTimeOptionsContainer TaskTimeOptionsContainer;
        private double _TimerCountdownSliderValue;
        private double _LastAnswerSliderValue;

        private Color DefaultOptionButtonBackgroundColor = Color.FromHex("#80aaff");
        private Color ActiveOptionButtonBackgroundColor = Color.FromHex("#99ffcc");

        public TimeOptionsPVM(TaskTimeOptionsContainer _taskTimeOptionsContainer)
        {
            TaskTimeOptionsContainer = _taskTimeOptionsContainer;

            _TimerCountdownSliderValue = TaskTimeOptionsContainer.AmountOfMinutes;
            _LastAnswerSliderValue = TaskTimeOptionsContainer.AmountOfSecondsForAnswer;

            TimeOptionsChangedCommand = new Command(TimeOptionsChanged);
        }

        public Color CountdownTimeOptionButtonColor
        {
            get
            {
                if (TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.CountdownTimer)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
        }

        public Color FixedAmountOfOperationsTimeOptionButtonColor
        {
            get
            {
                if (TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
        }

        public Color LastTaskTimeOptionButtonColor
        {
            get
            {
                if (TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.LastTask)
                    return ActiveOptionButtonBackgroundColor;
                else
                    return DefaultOptionButtonBackgroundColor;
            }
            private set { }
        }

        public bool CountdownTimeOptionsLayoutVisibility
        {
            get
            {
                if (TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.CountdownTimer)
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
                return TaskTimeOptionsContainer.AmountOfMinutes;
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
                TaskTimeOptionsContainer.AmountOfMinutes = (int)value;
                OnPropertyChanged("AmountOfMinutes");
                OnPropertyChanged("IntAmountOfMinutes");
            }
        }

        public bool FixedAmountOfOperationsLayoutVisibility
        {
            get
            {
                if (TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
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
                return TaskTimeOptionsContainer.AmountOfTasks;
            }
            set
            {
                TaskTimeOptionsContainer.AmountOfTasks = value;
                OnPropertyChanged("AmountOfTasks");
            }
        }

        public bool LastTaskLayoutVisibility
        {
            get
            {
                if (TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.LastTask)
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
                return TaskTimeOptionsContainer.AmountOfSecondsForAnswer;
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
                TaskTimeOptionsContainer.AmountOfSecondsForAnswer = (int)value;
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
                TaskTimeOptionsContainer.CurrentTimeOption = TimeOptions.CountdownTimer;
            }
            else if (button.Text == "Limited Tasks")
            {
                TaskTimeOptionsContainer.CurrentTimeOption = TimeOptions.FixedAmountOfOperations;
            }
            else if (button.Text == "Last Task")
            {
                TaskTimeOptionsContainer.CurrentTimeOption = TimeOptions.LastTask;
            }
            OnPropertyChanged("CountdownTimeOptionButtonColor");
            OnPropertyChanged("FixedAmountOfOperationsTimeOptionButtonColor");
            OnPropertyChanged("LastTaskTimeOptionButtonColor");
            OnPropertyChanged("CountdownTimeOptionsLayoutVisibility");
            OnPropertyChanged("FixedAmountOfOperationsLayoutVisibility");
            OnPropertyChanged("LastTaskLayoutVisibility");

            OnPropertyChanged("AmountOfMinutes");
            OnPropertyChanged("IntAmountOfMinutes");
            OnPropertyChanged("AmountOfTasks");
            OnPropertyChanged("AmountOfSecondsForAnswer");
            OnPropertyChanged("IntAmountOfSecondsForAnswer");           
        }
    }
}
