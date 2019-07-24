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
        private int InitialGeneralAmountOfTasks;

        private bool InternalCheck = true;
     
        public LimitedTasksTimeOption(TaskTimeOptionsContainer taskTimeOptions)
        {
            GeneralAmountOfTasks = taskTimeOptions.AmountOfTasks;
            InitialGeneralAmountOfTasks = GeneralAmountOfTasks;
            CurrentAmountOfTasks = 1;
        }

        public void TimerWork()
        {
            if(CheckTimerEnd())
            {
                Time = Time.Add(TimeSpan.FromSeconds(1));
            }
        }

        public string GetTimeString()
        {
            return $"Answering: {CurrentAmountOfTasks} / {GeneralAmountOfTasks}\n" + Time.ToString(@"mm\:ss");
        }

        public int GetMillis()
        {
            return (int)Time.TotalMilliseconds;
        }

        public bool CheckTimerEnd()
        {
            return InternalCheck;
        }

        public bool CanExecuteOperation(bool IsAnswerCorrect)
        {
            if (CurrentAmountOfTasks < GeneralAmountOfTasks)
            {
                CurrentAmountOfTasks += 1;
                return true;
            }
            else
            {
                InternalCheck = false;
                return false;
            }
        }

        public void TimerRestart()
        {
            GeneralAmountOfTasks = InitialGeneralAmountOfTasks;
            CurrentAmountOfTasks = 1;
            InternalCheck = true;
            Time = TimeSpan.FromSeconds(0);
        }
    }
}
