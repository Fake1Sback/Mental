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

        public void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                TimeLeft = TimeLeft.Subtract(TimeSpan.FromSeconds(1));
                if (TimeLeft.TotalSeconds <= 0)
                    return false;
                else
                    return true;
            });
        }

        public string GetTime()
        {
            return TimeLeft.ToString();
        }

        public bool CanExecuteOperation()
        {
            if (TimeLeft.TotalSeconds > 0)
                return true;
            else
                return false;
        }
    }
}
