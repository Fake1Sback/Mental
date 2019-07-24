using Mental.Models;
using Mental.Models.DbModels;
using Mental.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public class CountResultVM : BaseVM
    {
        private AbstractTaskType MathTask;
        private ITimeOption timeOption;
        private StatisticsTimer statisticsTimer;
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;
        private MathTasksPage mathTasksPage;
        private bool VMTimerBlocker = false;

        private string Answer = string.Empty;

        public CountResultVM(INavigation _navigation, MathTasksOptions _mathTasksOptions,ITimeOption _timeOption,MathTasksPage _mathTasksPage)
        {
            mathTasksOptions = _mathTasksOptions;
            timeOption = _timeOption;
            navigation = _navigation;
            mathTasksPage = _mathTasksPage;
            statisticsTimer = new StatisticsTimer();

            if (mathTasksOptions.IsIntegerNumbers)
                MathTask = new CountResultTaskOption(_mathTasksOptions);
            else
                MathTask = new CountDoubleResultTaskOption(_mathTasksOptions);

            DigitButtonPressedCommand = new Command(DigitsButtonPressed);
            DelButtonPressedCommand = new Command(DelButtonPressed);
            MinusButtonPressedCommand = new Command(MinusButtonPressed);
            DotButtonPressedCommand = new Command(DotButtonPressed);
            OkButtonPressedCommand = new Command(OkButtonPressed);

            MathTask.GenerateExpression();
            OnPropertyChanged("LabelFontSize");

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
                return MathTask.GetExpressionString();
            }
            private set { }
        }

        public string AnswerValue
        {
            get
            {
                if (Answer != string.Empty)
                    return Answer.ToString();
                else
                    return "?";
            }
            set
            {
                if (value == "-")
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
                }
                else if (value == ".")
                {
                    if (Answer.Length != 0 && !Answer.Contains("."))
                        Answer += ".";
                }
                else
                    Answer += value;
                OnPropertyChanged("AnswerValue");
            }
        }

        public int LabelFontSize
        {
            get
            {
                return GetFontSize(MathTask.GetExpressionString());
            }
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

        public int GetFontSize(string ExpressionString)
        {
            int ExpressionStringLengthFactor = ExpressionString.Length / 5;
            return 36 - 1 * ExpressionStringLengthFactor;
        }

        public Command DigitButtonPressedCommand { get; set; }

        private void DigitsButtonPressed(object obj)
        {
            Button button = obj as Button;
            AnswerValue = button.Text;
        }

        public Command DelButtonPressedCommand { get; set; }

        private void DelButtonPressed()
        {
            if (Answer.Length != 0)
                Answer = Answer.Remove(Answer.Length - 1);
            OnPropertyChanged("AnswerValue");
        }

        public Command MinusButtonPressedCommand { get; set; }

        private void MinusButtonPressed()
        {
            AnswerValue = "-";
        }

        public Command DotButtonPressedCommand { get; set; }

        private void DotButtonPressed()
        {
            AnswerValue = ".";
        }

        public Command OkButtonPressedCommand { get; set; }

        private void OkButtonPressed()
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

                OnPropertyChanged("LabelFontSize");
                OnPropertyChanged("AnswerValue");
                OnPropertyChanged("OperationValue");
            }
            else
            {
                statisticsTimer.TurnOffTimer();
                mathTasksPage.HideTaskFrame();
                VMTimerBlocker = true;
            }
        }

        public Command NavigateToStatisticsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    string Op = string.Empty;
                    foreach (var a in mathTasksOptions.Operations)
                    {
                        Op += a;
                    }

                    DbMathTask dbMathTask = new DbMathTask()
                    {
                        Operations = Op,
                        TaskType = (byte)mathTasksOptions.TaskType,
                        TimeOptions = (byte)mathTasksOptions.TaskTimeOptions.CurrentTimeOption,
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

                    if (mathTasksOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                    {
                        dbMathTask.TimeParameter = timeOption.GetMillis();
                        dbMathTask.TaskComplexityParameter = mathTasksOptions.TaskTimeOptions.AmountOfTasks;
                    }
                    else if (mathTasksOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.CountdownTimer)
                    {
                        dbMathTask.TimeParameter = mathTasksOptions.TaskTimeOptions.AmountOfMinutes;
                        dbMathTask.TaskComplexityParameter = mathTasksOptions.TaskTimeOptions.AmountOfMinutes;
                    }
                    else if (mathTasksOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.LastTask)
                    {
                        dbMathTask.TimeParameter = mathTasksOptions.TaskTimeOptions.AmountOfSecondsForAnswer;
                        dbMathTask.TaskComplexityParameter = mathTasksOptions.TaskTimeOptions.AmountOfSecondsForAnswer;
                    }

                    await navigation.PushAsync(new SimilarTasksStatisticsPage(dbMathTask, true));
                });
            }
        }
        public Command RestartCommand
        {
            get
            {
                return new Command(() =>
                {
                    mathTasksPage.ShowTaskFrame();
                    MathTask.AmountOfCorrectAnswers = 0;
                    MathTask.AmountOfWrongAnswers = 0;
                    Answer = string.Empty;
                    MathTask.GenerateExpression();
                    timeOption.TimerRestart();
                    OnPropertyChanged("AnswerValue");
                    OnPropertyChanged("OperationValue");
                    OnPropertyChanged("AmountOfCorrectAnswers");
                    OnPropertyChanged("AmountOfWrongAnswers");
                    VMTimerBlocker = false;
                    StartTimerCountdown();
                });
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
    }
}
