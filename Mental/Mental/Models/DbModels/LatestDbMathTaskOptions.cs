using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models.DbModels
{
    public class LatestDbMathTaskOptions
    {
        public int Id { get; set; }
        public string Operations { get; set; }

        public bool IsRestrictionActivated { get; set; }
        public string RestrictionsString { get; set; }

        public bool IsIntegerNumbers { get; set; }
        public int DigitsAfterDotSign { get; set; }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public bool IsChainLengthFixed { get; set; }
        public int MaxChainLength { get; set; }

        public int AmountOfTasks { get; set; }
        public int AmountOfMinutes { get; set; }
        public int AmountOfSecondsForAnswer { get; set; }

        public byte TaskType { get; set; }
        public byte TimeOptions { get; set; }

        //---------------------------------------
        public string CrutchField { get; set; }
    }
}
