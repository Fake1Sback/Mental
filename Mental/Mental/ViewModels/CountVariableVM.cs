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
    public class CountVariableVM : BaseVM
    {
        private AbstractTaskType MathTask;
        private ITimeOption timeOption;
        private MathTasksOptions mathTasksOptions;
        private INavigation navigation;
        private MathTasksPage mathTasksPage;
        private bool VMTimerBlocker = false;

        private string Answer = string.Empty;

        public CountVariableVM(INavigation _navigation, MathTasksOptions _mathTasksOptions, ITimeOption _timeOption,MathTasksPage _mathTasksPage)
        {
            mathTasksOptions = _mathTasksOptions;
            timeOption = _timeOption;
            navigation = _navigation;
            mathTasksPage = _mathTasksPage;

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

        public int LabelFontSize
        {
            get
            {
                return GetFontSize(OperationValue);
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
            Answer += button.Text;
            OnPropertyChanged("OperationValue");
            OnPropertyChanged("LabelFontSize");
        }

        public Command DelButtonPressedCommand { get; set; }

        private void DelButtonPressed()
        {
            if (Answer.Length != 0)
                Answer = Answer.Remove(Answer.Length - 1);
            OnPropertyChanged("OperationValue");
            OnPropertyChanged("LabelFontSize");
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
            OnPropertyChanged("LabelFontSize");
        }

        public Command DotButtonPressedCommand { get; set; }

        private void DotButtonPressed()
        {
            if (Answer.Length != 0 && !Answer.Contains("."))
                Answer += ".";
            OnPropertyChanged("OperationValue");
            OnPropertyChanged("LabelFontSize");
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
                MathTask.GenerateExpression();
              
                OnPropertyChanged("AnswerValue");
                OnPropertyChanged("OperationValue");
                OnPropertyChanged("LabelFontSize");
            }
            else
            {
                mathTasksPage.HideTaskFrame();
                VMTimerBlocker = true;
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
