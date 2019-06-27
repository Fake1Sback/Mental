using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models.DbModels;

namespace Mental.Models
{
    public class MathTasksOptions
    {
        public List<string> Operations = new List<string>();

        public bool IsRestrictionsActivated = false;
        public TaskRestrictions restrictions = new TaskRestrictions();

        public bool IsIntegerNumbers;
        public int DigitsAfterDotSign;

        public int MinValue;
        public int MaxValue;

        public bool IsChainLengthFixed;
        public int MaxChainLength;
        public const int MinAvailableChainLength = 2;
        public const int MaxAvailableChainLength = 8;

        public int AmountOfTasks;
        public int AmountOfMinutes;
        public int AmountOfSecondsForAnswer;

        public TaskType TaskType;
        public TimeOptions TimeOptions;
    }

    public enum TimeOptions
    {
        CountdownTimer,
        FixedAmountOfOperations,
        LastTask
    }

    public enum TaskType
    {
        CountResult,
        CountVariable
    }
}
