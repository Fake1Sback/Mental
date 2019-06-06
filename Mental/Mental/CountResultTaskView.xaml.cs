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

        private LimitedTasksTimeOption LimitedTasksTimeOption;

        private string Answer = string.Empty;

        public CountResultTaskView(MathTasksOptions _mathTasksOptions, ITimeOption _timeOption)
        {
            MathTask = new CountResultTaskOption(_mathTasksOptions);
            timeOption = _timeOption;

            InitializeComponent();
            timeOption.StartTimer();
            MathTask.GenerateExpression();
            StartTimerCountdown();
            OperationLabel.Text = MathTask.GetExpressionString();
        }

        private void StartTimerCountdown()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Timer_Label.Text = timeOption.GetTime();
                return true;
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
            if (timeOption.CanExecuteOperation())
            {
                if (MathTask.CheckAnswer(Convert.ToInt32(Answer)))
                {
                    MathTask.AmountOfCorrectAnswers += 1;
                    AmountOfCorrectAnswersLabel.Text = MathTask.AmountOfCorrectAnswers.ToString();
                }
                else
                {
                    MathTask.AmountOfWrongAnswers += 1;
                    AmountOfWrongAnswersLabel.Text = MathTask.AmountOfWrongAnswers.ToString();
                }
                Answer = string.Empty;
                AnswerLabel.Text = "?";
                MathTask.GenerateExpression();
                OperationLabel.Text = MathTask.GetExpressionString();
            }
            else
                Navigation.PopToRootAsync();
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