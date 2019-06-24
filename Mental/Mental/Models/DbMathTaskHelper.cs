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
            else if(dbMathTask.TimeOptions == 1)
            {
                return TimeSpan.FromMilliseconds(dbMathTask.TimeParameter).ToString(@"mm\:ss");
            }
            else
            {
                return dbMathTask.AmountOfCorrectAnswers.ToString();
            }
        }

        public static float GetEfficiencyParameterValue(this DbMathTask dbMathTask)
        {
            if (dbMathTask.TimeOptions == 0)
            {
                return dbMathTask.AmountOfCorrectAnswers - dbMathTask.AmountOfWrongAnswers;
            }
            else if (dbMathTask.TimeOptions == 1)
            {
                return -1 * dbMathTask.TimeParameter;
            }
            else
                return dbMathTask.AmountOfCorrectAnswers;
        }
    }
}
