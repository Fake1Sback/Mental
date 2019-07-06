using Mental.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public static class DbStroopTaskHelper
    {
        public static string GetEfficiencyParameterString(this DbStroopTask dbStroopTask)
        {
            if (dbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
            {
                return dbStroopTask.AmountOfCorrectAnswers + " / " + dbStroopTask.AmountOfWrongAnswers;
            }
            else if (dbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
            {
                return TimeSpan.FromMilliseconds(dbStroopTask.TimeParameter).ToString(@"mm\:ss");
            }
            else
            {
                return dbStroopTask.AmountOfCorrectAnswers.ToString();
            }
        }

        public static float GetEfficiencyParameterValue(this DbStroopTask dbStroopTask)
        {
            if (dbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
            {
                return dbStroopTask.AmountOfCorrectAnswers - dbStroopTask.AmountOfWrongAnswers;
            }
            else if (dbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
            {
                return -1 * dbStroopTask.TimeParameter;
            }
            else
                return dbStroopTask.AmountOfCorrectAnswers;
        }
    }
}
