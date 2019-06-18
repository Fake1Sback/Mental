using Mental.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mental
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CountResultTaskView : ContentPage
	{
        private AbstractTaskType MathTask;
        private ITimeOption timeOption;
        private StatisticsTimer statisticsTimer;
        private MathTasksOptions mathTasksOptions;

        private string Answer = string.Empty;
      //  private bool TimerWorking = true;

        public CountResultTaskView(MathTasksOptions _mathTasksOptions, ITimeOption _timeOption)
        {
            mathTasksOptions = _mathTasksOptions;
            if (_mathTasksOptions.IsIntegerNumbers)
                MathTask = new CountResultTaskOption(_mathTasksOptions);
            else
                MathTask = new CountDoubleResultTaskOption(_mathTasksOptions);
            timeOption = _timeOption;
            statisticsTimer = new StatisticsTimer();

            InitializeComponent();
            MathTask.GenerateExpression();
            StartTimerCountdown();
            statisticsTimer.StartAnswering(MathTask.GetExpressionString());
            OperationLabel.Text = MathTask.GetExpressionString();
        }

        private void StartTimerCountdown()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (timeOption.CheckTimerEnd())
                {
                    timeOption.TimerWork();
                    Timer_Label.Text = timeOption.GetTimeString();
                    return true;
                }
                else
                    return false;
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            Answer += button.Text;
            AnswerLabel.Text = Answer;
        }

        private void Ok_Button_Clicked(object sender, EventArgs e)
        {
            statisticsTimer.StopAnswering();
            if (MathTask.CheckAnswer(Answer))
            {
                MathTask.AmountOfCorrectAnswers += 1;
                AmountOfCorrectAnswersLabel.Text = MathTask.AmountOfCorrectAnswers.ToString();
            }
            else
            {
                MathTask.AmountOfWrongAnswers += 1;
                AmountOfWrongAnswersLabel.Text = MathTask.AmountOfWrongAnswers.ToString();
            }

            if (timeOption.CanExecuteOperation())
            {
                Answer = string.Empty;
                AnswerLabel.Text = "?";
                MathTask.GenerateExpression();
                statisticsTimer.StartAnswering(MathTask.GetExpressionString());
                OperationLabel.Text = MathTask.GetExpressionString();
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

                Navigation.PushAsync(new SimilarTasksStatisticsPage(new Models.DbModels.DbMathTask()
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
                },true));
            }
        }

        private void Del_Button_Clicked(object sender, EventArgs e)
        {
            if (Answer.Length != 0)
            {
                Answer = Answer.Remove(Answer.Length - 1);
                AnswerLabel.Text = Answer;
            }
        }

        private void Minus_Button_Clicked(object sender, EventArgs e)
        {
            if (Answer.Length == 0)
            {
                Answer = "-";
                AnswerLabel.Text = Answer;
            }
        }
    }
}