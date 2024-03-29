﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.Models
{
    public class CountdownTimeOption : ITimeOption
    {
        private TimeSpan TimeLeft;
        private TimeSpan InitialTimeLeft;
   
        public CountdownTimeOption(TaskTimeOptionsContainer taskTimeOptions)
        {    
            TimeLeft = TimeSpan.FromMinutes(taskTimeOptions.AmountOfMinutes);
            InitialTimeLeft = TimeLeft;
        }

        public void TimerWork()
        {
            if (CheckTimerEnd())
                TimeLeft = TimeLeft.Subtract(TimeSpan.FromSeconds(1));
        }

        public string GetTimeString()
        {
            return TimeLeft.ToString(@"mm\:ss");
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

        public void TimerRestart()
        {
            TimeLeft = InitialTimeLeft;
        }
    }
}
