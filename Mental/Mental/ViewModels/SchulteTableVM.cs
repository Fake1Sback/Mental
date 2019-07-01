﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Mental.Models;
using Mental.Views;
using Mental.Models.DbModels;

namespace Mental.ViewModels
{
    public class SchulteTableVM : INotifyPropertyChanged
    {
        private SchulteTableTaskOptions SchulteTableTaskOptions;
        private int _CurrentNumberToAnswer;
        private ITimeOption timeOption;
        private INavigation navigation;

        private int _AmountOfCorrectAnswers;
        private int _AmountOfWrongAnswers;

        public SchulteTableVM(SchulteTableTaskOptions _schulteTableTaskOptions,ITimeOption _timeOption,INavigation _navigation)
        {
            navigation = _navigation;
            SchulteTableTaskOptions = _schulteTableTaskOptions;
            timeOption = _timeOption;
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
                if (timeOption.CheckTimerEnd())
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
                return new Command(async (obj) =>
                {
                    Button button = obj as Button;
                    bool IsCorrectAnswer = _CurrentNumberToAnswer == Convert.ToInt32(button.Text);
                    if (IsCorrectAnswer)
                    {
                        _AmountOfCorrectAnswers += 1;
                        if (SchulteTableTaskOptions.IsEasyModeActivated)
                            button.BackgroundColor = Color.Red;
                    }
                    else
                        _AmountOfWrongAnswers += 1;

                    if (timeOption.CanExecuteOperation(IsCorrectAnswer) && _CurrentNumberToAnswer < SchulteTableTaskOptions.GridSize * SchulteTableTaskOptions.GridSize)
                    {
                        if (_CurrentNumberToAnswer == Convert.ToInt32(button.Text))
                            _CurrentNumberToAnswer += 1;
                        OnPropertyChanged("CurrentNumberString");
                    }
                    else
                    {
                        DbSchulteTableTask dbSchulteTableTask = new DbSchulteTableTask()
                        {
                            TimeOption = (byte)SchulteTableTaskOptions.TimeOptions,
                            AmountOfCorrectAnswers = _AmountOfCorrectAnswers,
                            AmountOfWrongAnswers = _AmountOfWrongAnswers,
                            IsEasyModeActivated = SchulteTableTaskOptions.IsEasyModeActivated,
                            GridSize = SchulteTableTaskOptions.GridSize,
                            LongestTimeNumberString = "-",
                            LongestTimeSpentForFindingNumber = 1,
                            ShortestTimeNumberString = "+",
                            ShortestTimeSpentForFindingNumber = 2,
                            TaskDateTime = DateTime.Now,
                        };

                        if(SchulteTableTaskOptions.TimeOptions == TimeOptions.CountdownTimer)
                        {
                            dbSchulteTableTask.TimeParameter = SchulteTableTaskOptions.AmountOfMinutes;
                            dbSchulteTableTask.TaskComplexityParameter = SchulteTableTaskOptions.AmountOfMinutes;
                        }
                        else if(SchulteTableTaskOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                        {
                            dbSchulteTableTask.TimeParameter = timeOption.GetMillis();
                            dbSchulteTableTask.TaskComplexityParameter = (int)Math.Pow(SchulteTableTaskOptions.GridSize, 2);
                        }
                        else
                        {
                            dbSchulteTableTask.TimeParameter = SchulteTableTaskOptions.AmountOfSecondsForAnswer;
                            dbSchulteTableTask.TaskComplexityParameter = SchulteTableTaskOptions.AmountOfSecondsForAnswer;
                        }

                        await navigation.PushAsync(new SimilarSchulteTableTasksStatisticsPage(dbSchulteTableTask,true));
                    }
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
