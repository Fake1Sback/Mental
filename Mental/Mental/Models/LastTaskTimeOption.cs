using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class LastTaskTimeOption : ITimeOption
    {
        private int InitialTime;
        private TimeSpan TimeLeft;

        public LastTaskTimeOption(MathTasksOptions _mathTasksOptions)
        {
            InitialTime = _mathTasksOptions.AmountOfSecondsForAnswer;
            TimeLeft = TimeSpan.FromSeconds(InitialTime);
        }

        public bool CanExecuteOperation(bool IsCorrectAnswer)
        {
            if (CheckTimerEnd() && IsCorrectAnswer)
            {
                TimeLeft = TimeSpan.FromSeconds(InitialTime);
                return true;
            }
            else
                return false;
        }

        public bool CheckTimerEnd()
        {
            if (TimeLeft.TotalSeconds > 0)
                return true;
            else
                return false;
        }

        public int GetMillis()
        {
            return (int)TimeLeft.TotalMilliseconds;
        }

        public string GetTimeString()
        {
            return TimeLeft.ToString();
        }

        public void TimerWork()
        {
            if (CheckTimerEnd())
                TimeLeft = TimeLeft.Subtract(TimeSpan.FromSeconds(1));
        }
    }
}
