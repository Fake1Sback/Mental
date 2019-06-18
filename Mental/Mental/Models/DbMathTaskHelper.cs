using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models.DbModels;
using Mental.Models;

namespace Mental.Models
{
    public static class DbMathTaskHelper
    {
        public static string GetEfficiencyParameterString(this DbMathTask dbMathTask)
        {
            if(dbMathTask.TimeOptions == 0)
            {
                return dbMathTask.AmountOfCorrectAnswers.ToString() + " / " + dbMathTask.AmountOfWrongAnswers.ToString();
            }
            else
            {
                return TimeSpan.FromMilliseconds(dbMathTask.AmountOfMinutes).ToString(@"mm\:ss");
            }
        }

        public static float GetEfficiencyParameterValue(this DbMathTask dbMathTask)
        {
            if(dbMathTask.TimeOptions == 0)
            {
                return dbMathTask.AmountOfCorrectAnswers - dbMathTask.AmountOfWrongAnswers;
            }
            else
            {
                return  -1 * dbMathTask.AmountOfMinutes;
            }
        }
    }
}
