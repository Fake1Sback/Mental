using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;

namespace Mental.Models
{
    public static class MappingExtensions
    {
        public static MathTasksOptions ToMathTaskOptions(this DbMathTaskOptions dbMathTaskOptions)
        {
            MathTasksOptions mathTasksOptions = new MathTasksOptions();
            if (dbMathTaskOptions.Operations.Contains("+"))
                mathTasksOptions.Operations.Add("+");
            if (dbMathTaskOptions.Operations.Contains("-"))
                mathTasksOptions.Operations.Add("-");
            if (dbMathTaskOptions.Operations.Contains("*"))
                mathTasksOptions.Operations.Add("*");
            if (dbMathTaskOptions.Operations.Contains("/"))
                mathTasksOptions.Operations.Add("/");

            mathTasksOptions.TaskType = (TaskType)dbMathTaskOptions.TaskType;

            mathTasksOptions.TaskTimeOptions = new TaskTimeOptionsContainer
            {
                CurrentTimeOption = (TimeOptions)dbMathTaskOptions.TimeOptions,
                AmountOfMinutes = dbMathTaskOptions.AmountOfMinutes,
                AmountOfTasks = dbMathTaskOptions.AmountOfTasks,
                AmountOfSecondsForAnswer = dbMathTaskOptions.AmountOfSecondsForAnswer
            };

            mathTasksOptions.IsRestrictionsActivated = dbMathTaskOptions.IsRestrictionActivated;
            mathTasksOptions.restrictions.restrictions = TaskRestrictions.GetTaskRestrictionFromString(dbMathTaskOptions.RestrictionsString);

            mathTasksOptions.IsIntegerNumbers = dbMathTaskOptions.IsIntegerNumbers;
            mathTasksOptions.DigitsAfterDotSign = dbMathTaskOptions.DigitsAfterDotSign;

            mathTasksOptions.MaxValue = dbMathTaskOptions.MaxValue;
            mathTasksOptions.MinValue = dbMathTaskOptions.MinValue;

            mathTasksOptions.IsChainLengthFixed = dbMathTaskOptions.IsChainLengthFixed;
            mathTasksOptions.MaxChainLength = dbMathTaskOptions.MaxChainLength;

            return mathTasksOptions;
        }

        public static DbMathTaskOptions ToDbMathTaskOptions(this MathTasksOptions mathTasksOptions)
        {
            DbMathTaskOptions dbMathTaskOptions = new DbMathTaskOptions();
            string operations = string.Empty;
            for (int i = 0; i < mathTasksOptions.Operations.Count; i++)
            {
                operations += mathTasksOptions.Operations[i];
            }

            dbMathTaskOptions.Operations = operations;

            dbMathTaskOptions.IsRestrictionActivated = mathTasksOptions.IsRestrictionsActivated;
            dbMathTaskOptions.RestrictionsString = TaskRestrictions.GetTaskRestrictionsString(mathTasksOptions.restrictions.restrictions);

            dbMathTaskOptions.IsIntegerNumbers = mathTasksOptions.IsIntegerNumbers;
            dbMathTaskOptions.DigitsAfterDotSign = mathTasksOptions.DigitsAfterDotSign;

            dbMathTaskOptions.MinValue = mathTasksOptions.MinValue;
            dbMathTaskOptions.MaxValue = mathTasksOptions.MaxValue;

            dbMathTaskOptions.IsChainLengthFixed = mathTasksOptions.IsChainLengthFixed;
            dbMathTaskOptions.MaxChainLength = mathTasksOptions.MaxChainLength;

            dbMathTaskOptions.AmountOfTasks = mathTasksOptions.TaskTimeOptions.AmountOfTasks;
            dbMathTaskOptions.AmountOfMinutes = mathTasksOptions.TaskTimeOptions.AmountOfMinutes;
            dbMathTaskOptions.AmountOfSecondsForAnswer = mathTasksOptions.TaskTimeOptions.AmountOfSecondsForAnswer;

            dbMathTaskOptions.TaskType = (byte)mathTasksOptions.TaskType;
            dbMathTaskOptions.TimeOptions = (byte)mathTasksOptions.TaskTimeOptions.CurrentTimeOption;

            return dbMathTaskOptions;
        }

        public static SchulteTableTaskOptions ToSchulteTableTaskOptions(this DbSchulteTableTaskOptions dbSchulteTableTaskOptions)
        {
            SchulteTableTaskOptions schulteTableTaskOptions = new SchulteTableTaskOptions()
            {
                GridSize = dbSchulteTableTaskOptions.GridSize,
                IsEasyModeActivated = dbSchulteTableTaskOptions.IsEasyModeActivated,
                TaskTimeOptions = new TaskTimeOptionsContainer
                {
                    CurrentTimeOption = (TimeOptions)dbSchulteTableTaskOptions.TimeOptions,
                    AmountOfMinutes = dbSchulteTableTaskOptions.AmountOfMinutes,
                    AmountOfTasks = dbSchulteTableTaskOptions.AmountOfTasks,
                    AmountOfSecondsForAnswer = dbSchulteTableTaskOptions.AmountOfSecondsForAnswer
                }
            };
            return schulteTableTaskOptions;
        }

        public static DbSchulteTableTaskOptions ToDbSchulteTableTaskOptions(this SchulteTableTaskOptions schulteTableTaskOptions)
        {
            DbSchulteTableTaskOptions dbSchulteTableTaskOptions = new DbSchulteTableTaskOptions();

            dbSchulteTableTaskOptions.GridSize = schulteTableTaskOptions.GridSize;
            dbSchulteTableTaskOptions.IsEasyModeActivated = schulteTableTaskOptions.IsEasyModeActivated;
            dbSchulteTableTaskOptions.TimeOptions = (byte)schulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption;
            dbSchulteTableTaskOptions.AmountOfMinutes = schulteTableTaskOptions.TaskTimeOptions.AmountOfMinutes;
            dbSchulteTableTaskOptions.AmountOfTasks = schulteTableTaskOptions.TaskTimeOptions.AmountOfTasks;
            dbSchulteTableTaskOptions.AmountOfSecondsForAnswer = schulteTableTaskOptions.TaskTimeOptions.AmountOfSecondsForAnswer;

            return dbSchulteTableTaskOptions;

        }

        public static StroopTaskOptions ToStroopTaskOptions(this DbStroopTaskOptions dbStroopTaskOptions)
        {
            StroopTaskOptions stroopTaskOptions = new StroopTaskOptions();

            stroopTaskOptions.StroopTaskType = (StroopTaskType)dbStroopTaskOptions.StroopTaskType;
            stroopTaskOptions.ButtonsAmount = dbStroopTaskOptions.ButtonsAmount;
            stroopTaskOptions.TaskTimeOptionsContainer = new TaskTimeOptionsContainer
            {
                AmountOfMinutes = dbStroopTaskOptions.AmountOfMinutes,
                AmountOfSecondsForAnswer = dbStroopTaskOptions.AmountOfSecondsForAnswer,
                AmountOfTasks = dbStroopTaskOptions.AmountOfTasks,
                CurrentTimeOption = (TimeOptions)dbStroopTaskOptions.TimeOptions
            };

            return stroopTaskOptions;
        }

        public static DbStroopTaskOptions ToDbStroopTaskOptions(this StroopTaskOptions stroopTaskOptions)
        {
            DbStroopTaskOptions dbStroopTaskOptions = new DbStroopTaskOptions();

            dbStroopTaskOptions.TimeOptions = (byte)stroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption;
            dbStroopTaskOptions.AmountOfMinutes = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfMinutes;
            dbStroopTaskOptions.AmountOfTasks = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfTasks;
            dbStroopTaskOptions.AmountOfSecondsForAnswer = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfSecondsForAnswer;
            dbStroopTaskOptions.ButtonsAmount = stroopTaskOptions.ButtonsAmount;
            dbStroopTaskOptions.StroopTaskType = (byte)stroopTaskOptions.StroopTaskType;

            return dbStroopTaskOptions;
        }
    }
}
