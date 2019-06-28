﻿using Mental.Models;
using Mental.Models.DbModels;
using Mental.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public class CountVariableVM : INotifyPropertyChanged
    {
        private AbstractTaskType MathTask;
        private ITimeOption timeOption;
        private StatisticsTimer statisticsTimer;
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;

        private string Answer = string.Empty;

        public CountVariableVM(INavigation _navigation, MathTasksOptions _mathTasksOptions, ITimeOption _timeOption)
        {
            mathTasksOptions = _mathTasksOptions;
            timeOption = _timeOption;
            navigation = _navigation;
            statisticsTimer = new StatisticsTimer();

            if (_mathTasksOptions.IsIntegerNumbers)
                MathTask = new CountVariableTaskOption(mathTasksOptions);
            else
                MathTask = new CountDoubleVariableTaskOption(mathTasksOptions);

            DigitButtonPressedCommand = new Command(DigitsButtonPressed);
            DelButtonPressedCommand = new Command(DelButtonPressed);
            MinusButtonPressedCommand = new Command(MinusButtonPressed);
            DotButtonPressedCommand = new Command(DotButtonPressed);
            OkButtonPressedCommand = new Command(OkButtonPressed);

            MathTask.GenerateExpression();
            StartTimerCountdown();
        }

        public string TimerValue
        {
            get
            {
                return timeOption.GetTimeString();
            }
            private set { }
        }

        public string OperationValue
        {
            get
            {
                if (Answer != string.Empty)
                    return MathTask.GetExpressionString().Replace("x", Answer);
                else
                    return MathTask.GetExpressionString();
            }
            private set { }
        }

        public string AnswerValue
        {
            get
            {
                return MathTask.GetResult();
            }
            private set { }
        }

        public int AmountOfCorrectAnswers
        {
            get
            {
                return MathTask.AmountOfCorrectAnswers;
            }
            private set { }
        }

        public int AmountOfWrongAnswers
        {
            get
            {
                return MathTask.AmountOfWrongAnswers;
            }
            private set { }
        }


        public Command DigitButtonPressedCommand { get; set; }

        private void DigitsButtonPressed(object obj)
        {
            Button button = obj as Button;
            Answer += button.Text;
            OnPropertyChanged("OperationValue");
        }

        public Command DelButtonPressedCommand { get; set; }

        private void DelButtonPressed()
        {
            if (Answer.Length != 0)
                Answer = Answer.Remove(Answer.Length - 1);
            OnPropertyChanged("OperationValue");
        }

        public Command MinusButtonPressedCommand { get; set; }

        private void MinusButtonPressed()
        {
            if (Answer.Length == 0)
                Answer = "-";
            else
            {
                if (Answer.Contains("-"))
                    Answer = Answer.Replace("-", "");
                else
                    Answer = Answer.Insert(0, "-");
            }              
            OnPropertyChanged("OperationValue");
        }

        public Command DotButtonPressedCommand { get; set; }

        private void DotButtonPressed()
        {
            if (Answer.Length != 0 && !Answer.Contains("."))
                Answer += ".";
            OnPropertyChanged("OperationValue");
        }

        public Command OkButtonPressedCommand { get; set; }

        private async void OkButtonPressed()
        {
            bool IsAnswerCorrect = MathTask.CheckAnswer(Answer);
            if (IsAnswerCorrect)
            {
                MathTask.AmountOfCorrectAnswers += 1;
                OnPropertyChanged("AmountOfCorrectAnswers");
            }
            else
            {
                MathTask.AmountOfWrongAnswers += 1;
                OnPropertyChanged("AmountOfWrongAnswers");
            }

            if (timeOption.CanExecuteOperation(IsAnswerCorrect))
            {
                Answer = string.Empty;
                statisticsTimer.RegisterTime(MathTask.GetExpressionString());
                MathTask.GenerateExpression();

                OnPropertyChanged("AnswerValue");
                OnPropertyChanged("OperationValue");
            }
            else
            {
                statisticsTimer.TurnOffTimer();
                string Op = string.Empty;
                foreach (var a in mathTasksOptions.Operations)
                {
                    Op += a;
                }

                DbMathTask dbMathTask = new DbMathTask()
                {
                    Operations = Op,
                    TaskType = (byte)mathTasksOptions.TaskType,
                    TimeOptions = (byte)mathTasksOptions.TimeOptions,
                    MinValue = mathTasksOptions.MinValue,
                    MaxValue = mathTasksOptions.MaxValue,
                    IsChainLengthFixed = mathTasksOptions.IsChainLengthFixed,
                    MaxChainLength = mathTasksOptions.MaxChainLength,
                    IsInteger = mathTasksOptions.IsIntegerNumbers,
                    DigitsAfterDotSing = mathTasksOptions.DigitsAfterDotSign,
                    IsRestrictionActivated = mathTasksOptions.IsRestrictionsActivated,
                    RestrictionsString = TaskRestrictions.GetTaskRestrictionsString(mathTasksOptions.restrictions.restrictions),
                    AmountOfCorrectAnswers = MathTask.AmountOfCorrectAnswers,
                    AmountOfWrongAnswers = MathTask.AmountOfWrongAnswers,
                    LongestTimeSpentForExpression = (int)statisticsTimer.LongestTimeSpentForExpression.TotalSeconds,
                    LongestTimeExpressionString = statisticsTimer.LongestTimeExpressionString,
                    ShortestTimeSpentForExpression = (int)statisticsTimer.ShortestTimeSpentForExpression.TotalSeconds,
                    ShortestTimeExpressionString = statisticsTimer.ShortestTimeExpressionString,
                    TaskDateTime = DateTime.Now
                };

                if (mathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                {
                    dbMathTask.TimeParameter = timeOption.GetMillis();
                    dbMathTask.TaskComplexityParameter = mathTasksOptions.AmountOfTasks;
                }
                else if (mathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
                {
                    dbMathTask.TimeParameter = mathTasksOptions.AmountOfMinutes;
                    dbMathTask.TaskComplexityParameter = mathTasksOptions.AmountOfMinutes;
                }
                else if (mathTasksOptions.TimeOptions == TimeOptions.LastTask)
                {
                    dbMathTask.TimeParameter = mathTasksOptions.AmountOfSecondsForAnswer;
                    dbMathTask.TaskComplexityParameter = mathTasksOptions.AmountOfSecondsForAnswer;
                }

                await navigation.PushAsync(new SimilarTasksStatisticsPage(dbMathTask, true));
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


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}