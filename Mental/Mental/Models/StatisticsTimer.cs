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

        private bool IsAnswering;
        private TimeSpan CurrentTime;
        private string CurrentExpression;

        public void StartAnswering(string expression)
        {
            IsAnswering = true;
            CurrentExpression = expression;
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (IsAnswering)
                {
                    CurrentTime = CurrentTime.Add(TimeSpan.FromSeconds(1));
                    return true;
                }
                else
                    return false;
            });
        }

        public void StopAnswering()
        {
            IsAnswering = false;
            if(CheckIfInternalDataEmpty())
            {
                LongestTimeSpentForExpression = CurrentTime;
                ShortestTimeSpentForExpression = CurrentTime;
                LongestTimeExpressionString = CurrentExpression;
                ShortestTimeExpressionString = CurrentExpression;
            }
            else
            {
                if(CurrentTime > LongestTimeSpentForExpression)
                {
                    LongestTimeSpentForExpression = CurrentTime;
                    LongestTimeExpressionString = CurrentExpression;
                }
                else if(CurrentTime < ShortestTimeSpentForExpression)
                {
                    ShortestTimeSpentForExpression = CurrentTime;
                    ShortestTimeExpressionString = CurrentExpression;
                }
            }
            CurrentTime = TimeSpan.FromSeconds(0);
        }

        private bool CheckIfInternalDataEmpty()
        {
            if (string.IsNullOrEmpty(LongestTimeExpressionString) || string.IsNullOrEmpty(ShortestTimeExpressionString))
            {
                return true;
            }
            else
                return false;
        }
    }
}
