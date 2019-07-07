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

        public static string GetEfficiencyParameterString(this DbSchulteTableTask dbSchulteTableTask)
        {
            if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
            {
                float val1 = (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100);
                float val2 = dbSchulteTableTask.TaskComplexityParameter * 1000 * 60 + 1;
                float val3 = dbSchulteTableTask.TimeParameter / val2 * 100;
                return ((int)val1 + (int)val3).ToString() + "%";

                //  return (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100 + (dbSchulteTableTask.TimeParameter / (dbSchulteTableTask.TaskComplexityParameter * 1000 * 60 + 1) * 100)).ToString() + "%";
            }
            else if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
            {
                return TimeSpan.FromMilliseconds(dbSchulteTableTask.TimeParameter).ToString(@"mm\:ss");
            }
            else
            {
                return (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100).ToString() + "%";
            }
        }

        public static float GetEfficiencyParameterValue(this DbSchulteTableTask dbSchulteTableTask)
        {
            if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
            {
                float val1 = (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100);
                float val2 = dbSchulteTableTask.TaskComplexityParameter * 1000 * 60 + 1;
                float val3 = dbSchulteTableTask.TimeParameter / val2 * 100;
                return (int)val1 + (int)val3;

              //  return (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100) + (dbSchulteTableTask.TimeParameter / (dbSchulteTableTask.TaskComplexityParameter * 1000 * 60 + 1) * 100);
            }
            else if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
            {
                return -1 * dbSchulteTableTask.TimeParameter;
            }
            else
                return (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100);
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
