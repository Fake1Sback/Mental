using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.Models
{
    public class StatisticsTimer
    {
        public TimeSpan LongestTimeSpentForExpression;
        public string LongestTimeExpressionString;

        public TimeSpan ShortestTimeSpentForExpression;
        public string ShortestTimeExpressionString;

        private TimeSpan CurrentTime;
        private TimeSpan LastAnswerTime;
        private string CurrentExpression;

        private bool IsWorking = true;

        public StatisticsTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (IsWorking)
                {
                    CurrentTime = CurrentTime.Add(TimeSpan.FromSeconds(1));
                    return true;
                }
                else
                    return false;
            });
        }

        public void RegisterTime(string expression)
        {
            CurrentExpression = expression;
            if(CheckIfInternalDataEmpty())
            {
                LongestTimeSpentForExpression = CurrentTime;
                ShortestTimeSpentForExpression = CurrentTime;
                LongestTimeExpressionString = CurrentExpression;
                ShortestTimeExpressionString = CurrentExpression;
                LastAnswerTime = CurrentTime;
            }
            else
            {
                TimeSpan RegisterTime = CurrentTime.Subtract(LastAnswerTime);
                if (RegisterTime > LongestTimeSpentForExpression)
                {
                    LongestTimeSpentForExpression = RegisterTime;
                    LongestTimeExpressionString = CurrentExpression;
                }
                else if (RegisterTime < ShortestTimeSpentForExpression)
                {
                    ShortestTimeSpentForExpression = RegisterTime;
                    ShortestTimeExpressionString = CurrentExpression;
                }
                LastAnswerTime = CurrentTime;
            }
        }

        public void TurnOffTimer()
        {
            IsWorking = false;
        }

        private bool CheckIfInternalDataEmpty()
        {
            if (string.IsNullOrEmpty(LongestTimeExpressionString) || string.IsNullOrEmpty(ShortestTimeExpressionString))
                return true;
            else
                return false;
        }
    }
}
