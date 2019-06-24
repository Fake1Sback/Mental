using Mental.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public class CountResultVM : INotifyPropertyChanged
    {
        private AbstractTaskType MathTask;
        private ITimeOption timeOption;
        private StatisticsTimer statisticsTimer;
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;

        private string Answer = string.Empty;

        public CountResultVM(INavigation _navigation, MathTasksOptions _mathTasksOptions,ITimeOption _timeOption)
        {
            mathTasksOptions = _mathTasksOptions;
            timeOption = _timeOption;
            navigation = _navigation;
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
            StartTimerCountdown();
           // statisticsTimer.StartAnswering(MathTask.GetExpressionString());
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
               // statisticsTimer.StartAnswering(MathTask.GetExpressionString());

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


                if (mathTasksOptions.TimeOptions == TimeOptions.FixedAmountOfOperations)
                    mathTasksOptions.AmountOfMinutes = timeOption.GetMillis();

                navigation.PushAsync(new Mental.Views.SimilarTasksStatisticsPage(new Models.DbModels.DbMathTask()
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
