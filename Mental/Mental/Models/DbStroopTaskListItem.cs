using Mental.Models.DbModels;
using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class DbStroopTaskListItem : BaseVM
    {
        public DbStroopTask DbStroopTask;

        public DbStroopTaskListItem(DbStroopTask _dbStroopTask)
        {
            DbStroopTask = _dbStroopTask;
        }

        public string TaskTypeString
        {
            get
            {
                if (DbStroopTask.StroopTaskOption == (byte)StroopTaskType.FindOneCorrect)
                    return "Find 1";
                else if (DbStroopTask.StroopTaskOption == (byte)StroopTaskType.TrueOrFalse)
                    return "True/False";
                else
                    return "Find Color";
            }
        }

        public string AmountOfButtonsString
        {
            get
            {
                return "Buttons: " + DbStroopTask.AmountOfButtons;
            }
        }

        public string TimeOptionString
        {
            get
            {

                if (DbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return DbStroopTask.TaskComplexityParameter + " min";
                else if (DbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return DbStroopTask.TaskComplexityParameter.ToString() + " tasks";
                else
                    return DbStroopTask.TaskComplexityParameter.ToString() + " sec";
            }
        }

        public int CorrectAnswers
        {
            get
            {
                return DbStroopTask.AmountOfCorrectAnswers;
            }
        }

        public int WrongAnswers
        {
            get
            {
                return DbStroopTask.AmountOfWrongAnswers;
            }
        }

        public string DateTimeString
        {
            get
            {
                return "Date: " + DbStroopTask.TaskDateTime.ToString(@"dd-MM-yy HH:mm");
            }
        }
    }
}
