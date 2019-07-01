using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.Models
{
    public class CountdownTimeOption : ITimeOption
    {
        private TimeSpan TimeLeft;
   
        public CountdownTimeOption(MathTasksOptions _mathTasksOptions)
        {    
            TimeLeft = TimeSpan.FromMinutes(_mathTasksOptions.AmountOfMinutes);
        }

        public CountdownTimeOption(SchulteTableTaskOptions _schulteTableTaskOptions)
        {
            TimeLeft = TimeSpan.FromMinutes(_schulteTableTaskOptions.AmountOfMinutes);
        }

        public void TimerWork()
        {
            if (CheckTimerEnd())
                TimeLeft = TimeLeft.Subtract(TimeSpan.FromSeconds(1));
        }

        public string GetTimeString()
        {
            return TimeLeft.ToString();
        }

        public int GetMillis()
        {
            return (int)TimeLeft.TotalMilliseconds;
        }

        public bool CheckTimerEnd()
        {
            if (TimeLeft.TotalSeconds > 0)
                return true;
            else
                return false;
        }

        public bool CanExecuteOperation(bool IsAnswerCorrect)
        {
            return CheckTimerEnd();
        }
    }
}
