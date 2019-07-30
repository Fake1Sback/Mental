using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Mental.Models;
using Mental.Views;
using Mental.Models.DbModels;

namespace Mental.ViewModels
{
    public class SchulteTableVM : BaseVM
    {
        private SchulteTableTaskOptions SchulteTableTaskOptions;
        private int _CurrentNumberToAnswer;
        private ITimeOption timeOption;
        private INavigation navigation;
        private SchulteTableTaskPage schulteTableTaskPage;
        private bool VMTimerBlocker = false;

        private int _AmountOfCorrectAnswers;
        private int _AmountOfWrongAnswers;

        public SchulteTableVM(SchulteTableTaskOptions _schulteTableTaskOptions,ITimeOption _timeOption,INavigation _navigation,SchulteTableTaskPage _schulteTableTaskPage)
        {
            navigation = _navigation;
            SchulteTableTaskOptions = _schulteTableTaskOptions;
            timeOption = _timeOption;
            schulteTableTaskPage = _schulteTableTaskPage;

            StartTimerCountdown();
            _CurrentNumberToAnswer = 1;
            OnPropertyChanged("CurrentNumberString");
        }

        public string TimerValue
        {
            get
            {
                return timeOption.GetTimeString();
            }
        }

        private void StartTimerCountdown()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (timeOption.CheckTimerEnd() && !VMTimerBlocker)
                {
                    timeOption.TimerWork();
                    OnPropertyChanged("TimerValue");
                    return true;
                }
                else
                    return false;
            });
        }

        public string CurrentNumberString
        {
            get
            {
                return _CurrentNumberToAnswer.ToString();
            }
        }

        public Command SchulteTableButtonClicked
        {
            get
            {
                return new Command((obj) =>
                {
                    Button button = obj as Button;
                    int Answer = Convert.ToInt32(button.Text);
                    bool IsCorrectAnswer = _CurrentNumberToAnswer == Answer;
                    if (IsCorrectAnswer)
                    {
                        _AmountOfCorrectAnswers += 1;
                        if (SchulteTableTaskOptions.IsEasyModeActivated)
                        {
                            button.BackgroundColor = Color.FromHex("#ff8566");
                            button.BorderColor = Color.FromHex("#ff8566");
                        }
                    }
                    //else
                    //    _AmountOfWrongAnswers += 1;

                    if (SchulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                    {
                        if (IsCorrectAnswer)
                        {
                            if (timeOption.CanExecuteOperation(IsCorrectAnswer) && _CurrentNumberToAnswer < SchulteTableTaskOptions.GridSize * SchulteTableTaskOptions.GridSize)
                            {
                                if (_CurrentNumberToAnswer == Answer)
                                    _CurrentNumberToAnswer += 1;
                                OnPropertyChanged("CurrentNumberString");
                            }
                            else
                            {
                                schulteTableTaskPage.HideTaskFrame();
                                VMTimerBlocker = true;
                            }
                        }
                    }
                    else
                    {
                        if (timeOption.CanExecuteOperation(IsCorrectAnswer) && _CurrentNumberToAnswer < SchulteTableTaskOptions.GridSize * SchulteTableTaskOptions.GridSize)
                        {
                            if (_CurrentNumberToAnswer == Answer)
                                _CurrentNumberToAnswer += 1;
                            OnPropertyChanged("CurrentNumberString");
                        }
                        else
                        {
                            schulteTableTaskPage.HideTaskFrame();
                            VMTimerBlocker = true;
                        }
                    }
                });
            }
        }

        public Command RestartCommand
        {
            get
            {
                return new Command(() =>
                {
                    schulteTableTaskPage.ShowTaskFrame();
                    schulteTableTaskPage.ButtonsColorToDefault();
                    _AmountOfWrongAnswers = 0;
                    _AmountOfCorrectAnswers = 0;
                    _CurrentNumberToAnswer = 1;
                    OnPropertyChanged("CurrentNumberString");
                    timeOption.TimerRestart();
                    VMTimerBlocker = false;
                    StartTimerCountdown();
                });
            }
        }

        public Command NavigateToStatisticsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    DbSchulteTableTask dbSchulteTableTask = new DbSchulteTableTask()
                    {
                        TimeOption = (byte)SchulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption,
                        AmountOfCorrectAnswers = _AmountOfCorrectAnswers,
                        AmountOfWrongAnswers = _AmountOfWrongAnswers,
                        IsEasyModeActivated = SchulteTableTaskOptions.IsEasyModeActivated,
                        GridSize = SchulteTableTaskOptions.GridSize,                  
                        TaskDateTime = DateTime.Now,
                    };

                    if (SchulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.CountdownTimer)
                    {
                        dbSchulteTableTask.TimeParameter = timeOption.GetMillis();
                        dbSchulteTableTask.TaskComplexityParameter = SchulteTableTaskOptions.TaskTimeOptions.AmountOfMinutes;
                    }
                    else if (SchulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                    {
                        dbSchulteTableTask.TimeParameter = timeOption.GetMillis();
                        dbSchulteTableTask.TaskComplexityParameter = (int)Math.Pow(SchulteTableTaskOptions.GridSize, 2);
                    }
                    else
                    {
                        dbSchulteTableTask.TimeParameter = SchulteTableTaskOptions.TaskTimeOptions.AmountOfSecondsForAnswer;
                        dbSchulteTableTask.TaskComplexityParameter = SchulteTableTaskOptions.TaskTimeOptions.AmountOfSecondsForAnswer;
                    }

                    await navigation.PushAsync(new SimilarSchulteTableTasksStatisticsPage(dbSchulteTableTask, true));
                });
            }
        }

    }
}
