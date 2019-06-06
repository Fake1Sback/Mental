using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.Models
{
    public class LimitedTasksTimeOption : ITimeOption
    {
        private TimeSpan Time;
        private int GeneralAmountOfTasks;
        private int CurrentAmountOfTasks;

        public LimitedTasksTimeOption(MathTasksOptions mathTasksOptions)
        {
            GeneralAmountOfTasks = mathTasksOptions.AmountOfTasks;
            CurrentAmountOfTasks = 1;
        }


        public void StartTimer()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                Time = Time.Add(TimeSpan.FromSeconds(1));
                return true;
            });
        }

        public string GetTime()
        {
            return $"Answered: {CurrentAmountOfTasks} / {GeneralAmountOfTasks}\n {Time.ToString()}";
        }

        public bool CanExecuteOperation()
        {
            if (CurrentAmountOfTasks < GeneralAmountOfTasks)
            {
                CurrentAmountOfTasks += 1;
                return true;
            }
            else
                return false;
        }
    }
}
