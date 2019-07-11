using Mental.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public static class DbTaskHelpers
    {
        public static string GetEfficiencyParameterString(this DbMathTask dbMathTask)
        {
            if (dbMathTask.TimeOptions == 0)
            {
                return dbMathTask.AmountOfCorrectAnswers.ToString() + " / " + dbMathTask.AmountOfWrongAnswers.ToString();
            }
            else if (dbMathTask.TimeOptions == 1)
            {
                return TimeSpan.FromMilliseconds(dbMathTask.TimeParameter).ToString(@"mm\:ss");
            }
            else
            {
                return dbMathTask.AmountOfCorrectAnswers.ToString();
            }
        }

        public static double GetEfficiencyParameterValue(this DbMathTask dbMathTask)
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

        public static string GetEfficiencyParameterString(this DbSchulteTableTask dbSchulteTableTask)
        {
            if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
            {
                double val1 = dbSchulteTableTask.AmountOfCorrectAnswers / Math.Pow(dbSchulteTableTask.GridSize, 2);
                double val2 = val1 * 100;
                double val3 = dbSchulteTableTask.TaskComplexityParameter * 1000 * 60 + 1;
                double val4 = (dbSchulteTableTask.TimeParameter / val3) * 100;
                return ((int)val2 + (int)val4).ToString() + "%";               
            }
            else if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
            {
                return TimeSpan.FromMilliseconds(dbSchulteTableTask.TimeParameter).ToString(@"mm\:ss");
            }
            else
            {
                return ((dbSchulteTableTask.AmountOfCorrectAnswers / Math.Pow(dbSchulteTableTask.GridSize, 2)) * 100).ToString() + "%";
            }
        }

        public static double GetEfficiencyParameterValue(this DbSchulteTableTask dbSchulteTableTask)
        {
            if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
            {
                double val1 = dbSchulteTableTask.AmountOfCorrectAnswers / Math.Pow(dbSchulteTableTask.GridSize, 2);
                double val2 = val1 * 100;
                double val3 = dbSchulteTableTask.TaskComplexityParameter * 1000 * 60 + 1;
                double val4 = (dbSchulteTableTask.TimeParameter / val3) * 100;
                return (int)val2 + (int)val4;       
            }
            else if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
            {
                return -1 * dbSchulteTableTask.TimeParameter;
            }
            else
                return (int)((dbSchulteTableTask.AmountOfCorrectAnswers / Math.Pow(dbSchulteTableTask.GridSize, 2)) * 100);
        }

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

        public static double GetEfficiencyParameterValue(this DbStroopTask dbStroopTask)
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
