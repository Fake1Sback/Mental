using Mental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public class CountResultIntVM : INotifyPropertyChanged
    {
        private AbstractTaskType MathTask;
        private ITimeOption timeOption;
        private StatisticsTimer statisticsTimer;
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;

        private string Result = string.Empty;

        public CountResultIntVM(INavigation _navigation, MathTasksOptions _mathTasksOptions,ITimeOption _timeOption)
        {
            mathTasksOptions = _mathTasksOptions;
            timeOption = _timeOption;
            navigation = _navigation;
            statisticsTimer = new StatisticsTimer();
            MathTask = new CountResultTaskOption(_mathTasksOptions);

            DigitButtonPressedCommand = new Command(DigitsButtonPressed);
            DelButtonPressedCommand = new Command(DelButtonPressed);
            MinusButtonPressedCommand = new Command(MinusButtonPressed);
            DotButtonPressedCommand = new Command(DotButtonPressed);
            OkButtonPressedCommand = new Command(OkButtonPressed);

            MathTask.GenerateExpression();
            StartTimerCountdown();
            statisticsTimer.StartAnswering(MathTask.GetExpressionString());
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
                if (Result != string.Empty)
                    return Result.ToString();
                else
                    return "?";
            }
            set
            {
                if (Result.Length == 0 && value == "-")
                    Result = value;
                else if (Result.Length != 0 && value == "." && !Result.Contains("."))
                    Result += ".";
                else
                    Result += value;
                OnPropertyChanged("AnswerValue");
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


        public Command DigitButtonPressedCommand { get; set; }

        private void DigitsButtonPressed(object obj)
        {
            Button button = obj as Button;
            AnswerValue = button.Text;
        }

        public Command DelButtonPressedCommand { get; set; }

        private void DelButtonPressed()
        {
            if (Result.Length != 0)
                Result = Result.Remove(Result.Length - 1);
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
            statisticsTimer.StopAnswering();
            if (MathTask.CheckAnswer(Result))
            {
                MathTask.AmountOfCorrectAnswers += 1;
                OnPropertyChanged("AmountOfCorrectAnswers");
            }
            else
            {
                MathTask.AmountOfWrongAnswers += 1;
                OnPropertyChanged("AmountOfWrongAnswers");
            }

            if (timeOption.CanExecuteOperation())
            {
                Result = string.Empty;
                MathTask.GenerateExpression();
                statisticsTimer.StartAnswering(MathTask.GetExpressionString());

                OnPropertyChanged("AnswerValue");
                OnPropertyChanged("OperationValue");
            }
            else
            {
                string Op = string.Empty;
                foreach (var a in mathTasksOptions.Operations)
                {
                    Op += a;
                }


                if (mathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                    mathTasksOptions.AmountOfMinutes = timeOption.GetMillis();

                navigation.PushAsync(new SimilarTasksStatisticsPage(new Models.DbModels.DbMathTask()
                {
                    Operations = Op,
                    TaskType = (byte)mathTasksOptions.TaskType,
                    TimeOptions = (byte)mathTasksOptions.TimeOptions,
                    MinValue = mathTasksOptions.MinValue,
                    MaxValue = mathTasksOptions.MaxValue,
                    AmountOfTasks = mathTasksOptions.AmountOfTasks,
                    AmountOfMinutes = mathTasksOptions.AmountOfMinutes,
                    IsChainLengthFixed = mathTasksOptions.IsChainLengthFixed,
                    MaxChainLength = mathTasksOptions.MaxChainLength,
                    IsInteger = mathTasksOptions.IsIntegerNumbers,
                    DigitsAfterDotSing = mathTasksOptions.DigitsAfterDotSign,
                    IsSpecialModeActivated = mathTasksOptions.IsSpecialModeActivated,
                    AmountOfXDigits = mathTasksOptions.AmountOfXDigits,
                    AmountOfCorrectAnswers = MathTask.AmountOfCorrectAnswers,
                    AmountOfWrongAnswers = MathTask.AmountOfWrongAnswers,
                    LongestTimeSpentForExpression = (int)statisticsTimer.LongestTimeSpentForExpression.TotalSeconds,
                    LongestTimeExpressionString = statisticsTimer.LongestTimeExpressionString,
                    ShortestTimeSpentForExpression = (int)statisticsTimer.ShortestTimeSpentForExpression.TotalSeconds,
                    ShortestTimeExpressionString = statisticsTimer.ShortestTimeExpressionString
                }, true));
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
