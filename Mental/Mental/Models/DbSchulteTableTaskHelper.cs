using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models.DbModels;

namespace Mental.Models
{
    public static class DbSchulteTableTaskHelper
    {
        public static string GetEfficiencyParameterString(this DbSchulteTableTask dbSchulteTableTask)
        {
            if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
            {
                return (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100).ToString() + "%";
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
                return (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100);
            }
            else if (dbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
            {
                return -1 * dbSchulteTableTask.TimeParameter;
            }
            else
                return (dbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(dbSchulteTableTask.GridSize, 2) * 100);
        }
    }
}
