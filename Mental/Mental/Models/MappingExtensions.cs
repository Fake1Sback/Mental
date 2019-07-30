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

        public static LatestDbMathTaskOptions ToLatestDbMathTaskOptions(this MathTasksOptions mathTasksOptions)
        {
            LatestDbMathTaskOptions latestDbMathTaskOptions = new LatestDbMathTaskOptions();
            string operations = string.Empty;
            for (int i = 0; i < mathTasksOptions.Operations.Count; i++)
            {
                operations += mathTasksOptions.Operations[i];
            }

            latestDbMathTaskOptions.Operations = operations;

            latestDbMathTaskOptions.IsRestrictionActivated = mathTasksOptions.IsRestrictionsActivated;
            latestDbMathTaskOptions.RestrictionsString = TaskRestrictions.GetTaskRestrictionsString(mathTasksOptions.restrictions.restrictions);

            latestDbMathTaskOptions.IsIntegerNumbers = mathTasksOptions.IsIntegerNumbers;
            latestDbMathTaskOptions.DigitsAfterDotSign = mathTasksOptions.DigitsAfterDotSign;

            latestDbMathTaskOptions.MinValue = mathTasksOptions.MinValue;
            latestDbMathTaskOptions.MaxValue = mathTasksOptions.MaxValue;

            latestDbMathTaskOptions.IsChainLengthFixed = mathTasksOptions.IsChainLengthFixed;
            latestDbMathTaskOptions.MaxChainLength = mathTasksOptions.MaxChainLength;

            latestDbMathTaskOptions.AmountOfTasks = mathTasksOptions.TaskTimeOptions.AmountOfTasks;
            latestDbMathTaskOptions.AmountOfMinutes = mathTasksOptions.TaskTimeOptions.AmountOfMinutes;
            latestDbMathTaskOptions.AmountOfSecondsForAnswer = mathTasksOptions.TaskTimeOptions.AmountOfSecondsForAnswer;

            latestDbMathTaskOptions.TaskType = (byte)mathTasksOptions.TaskType;
            latestDbMathTaskOptions.TimeOptions = (byte)mathTasksOptions.TaskTimeOptions.CurrentTimeOption;

            return latestDbMathTaskOptions;
        }

        public static MathTasksOptions ToMathTaskOptions(this LatestDbMathTaskOptions latestDbMathTaskOptions)
        {
            MathTasksOptions mathTasksOptions = new MathTasksOptions();
            if (latestDbMathTaskOptions.Operations.Contains("+"))
                mathTasksOptions.Operations.Add("+");
            if (latestDbMathTaskOptions.Operations.Contains("-"))
                mathTasksOptions.Operations.Add("-");
            if (latestDbMathTaskOptions.Operations.Contains("*"))
                mathTasksOptions.Operations.Add("*");
            if (latestDbMathTaskOptions.Operations.Contains("/"))
                mathTasksOptions.Operations.Add("/");

            mathTasksOptions.TaskType = (TaskType)latestDbMathTaskOptions.TaskType;

            mathTasksOptions.TaskTimeOptions = new TaskTimeOptionsContainer
            {
                CurrentTimeOption = (TimeOptions)latestDbMathTaskOptions.TimeOptions,
                AmountOfMinutes = latestDbMathTaskOptions.AmountOfMinutes,
                AmountOfTasks = latestDbMathTaskOptions.AmountOfTasks,
                AmountOfSecondsForAnswer = latestDbMathTaskOptions.AmountOfSecondsForAnswer
            };

            mathTasksOptions.IsRestrictionsActivated = latestDbMathTaskOptions.IsRestrictionActivated;
            mathTasksOptions.restrictions.restrictions = TaskRestrictions.GetTaskRestrictionFromString(latestDbMathTaskOptions.RestrictionsString);

            mathTasksOptions.IsIntegerNumbers = latestDbMathTaskOptions.IsIntegerNumbers;
            mathTasksOptions.DigitsAfterDotSign = latestDbMathTaskOptions.DigitsAfterDotSign;

            mathTasksOptions.MaxValue = latestDbMathTaskOptions.MaxValue;
            mathTasksOptions.MinValue = latestDbMathTaskOptions.MinValue;

            mathTasksOptions.IsChainLengthFixed = latestDbMathTaskOptions.IsChainLengthFixed;
            mathTasksOptions.MaxChainLength = latestDbMathTaskOptions.MaxChainLength;

            return mathTasksOptions;
        }

        //------------------------------------------------------------------------------------------------------------------

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

        public static LatestDbSchulteTableTaskOptions ToLatestDbSchulteTaskOptions(this SchulteTableTaskOptions schulteTableTaskOptions)
        {
            LatestDbSchulteTableTaskOptions latestDbSchulteTableTaskOptions = new LatestDbSchulteTableTaskOptions();

            latestDbSchulteTableTaskOptions.GridSize = schulteTableTaskOptions.GridSize;
            latestDbSchulteTableTaskOptions.IsEasyModeActivated = schulteTableTaskOptions.IsEasyModeActivated;
            latestDbSchulteTableTaskOptions.TimeOptions = (byte)schulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption;
            latestDbSchulteTableTaskOptions.AmountOfMinutes = schulteTableTaskOptions.TaskTimeOptions.AmountOfMinutes;
            latestDbSchulteTableTaskOptions.AmountOfTasks = schulteTableTaskOptions.TaskTimeOptions.AmountOfTasks;
            latestDbSchulteTableTaskOptions.AmountOfSecondsForAnswer = schulteTableTaskOptions.TaskTimeOptions.AmountOfSecondsForAnswer;

            return latestDbSchulteTableTaskOptions;
        }

        public static SchulteTableTaskOptions ToSchulteTableTaskOptions(this LatestDbSchulteTableTaskOptions latestDbSchulteTableTaskOptions)
        {
            SchulteTableTaskOptions schulteTableTaskOptions = new SchulteTableTaskOptions()
            {
                GridSize = latestDbSchulteTableTaskOptions.GridSize,
                IsEasyModeActivated = latestDbSchulteTableTaskOptions.IsEasyModeActivated,
                TaskTimeOptions = new TaskTimeOptionsContainer
                {
                    CurrentTimeOption = (TimeOptions)latestDbSchulteTableTaskOptions.TimeOptions,
                    AmountOfMinutes = latestDbSchulteTableTaskOptions.AmountOfMinutes,
                    AmountOfTasks = latestDbSchulteTableTaskOptions.AmountOfTasks,
                    AmountOfSecondsForAnswer = latestDbSchulteTableTaskOptions.AmountOfSecondsForAnswer
                }
            };
            return schulteTableTaskOptions;
        }

        //-----------------------------------------------------------------------------------------------

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

        public static LatestDbStroopTaskOptions ToLatestDbStroopTaskOptions(this StroopTaskOptions stroopTaskOptions)
        {
            LatestDbStroopTaskOptions latestDbStroopTaskOptions = new LatestDbStroopTaskOptions();

            latestDbStroopTaskOptions.TimeOptions = (byte)stroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption;
            latestDbStroopTaskOptions.AmountOfMinutes = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfMinutes;
            latestDbStroopTaskOptions.AmountOfTasks = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfTasks;
            latestDbStroopTaskOptions.AmountOfSecondsForAnswer = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfSecondsForAnswer;
            latestDbStroopTaskOptions.ButtonsAmount = stroopTaskOptions.ButtonsAmount;
            latestDbStroopTaskOptions.StroopTaskType = (byte)stroopTaskOptions.StroopTaskType;

            return latestDbStroopTaskOptions;
        }

        public static StroopTaskOptions ToStroopTaskOptions(this LatestDbStroopTaskOptions latestDbStroopTaskOptions)
        {
            StroopTaskOptions stroopTaskOptions = new StroopTaskOptions();

            stroopTaskOptions.StroopTaskType = (StroopTaskType)latestDbStroopTaskOptions.StroopTaskType;
            stroopTaskOptions.ButtonsAmount = latestDbStroopTaskOptions.ButtonsAmount;
            stroopTaskOptions.TaskTimeOptionsContainer = new TaskTimeOptionsContainer
            {
                AmountOfMinutes = latestDbStroopTaskOptions.AmountOfMinutes,
                AmountOfSecondsForAnswer = latestDbStroopTaskOptions.AmountOfSecondsForAnswer,
                AmountOfTasks = latestDbStroopTaskOptions.AmountOfTasks,
                CurrentTimeOption = (TimeOptions)latestDbStroopTaskOptions.TimeOptions
            };

            return stroopTaskOptions;
        }
    }
}
