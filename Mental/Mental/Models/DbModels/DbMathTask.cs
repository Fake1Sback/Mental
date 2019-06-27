using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models.DbModels
{
    public class DbMathTask
    {   
        public int Id { get; set; }
        public string Operations { get; set; }
        public byte TaskType { get; set; }
        public byte TimeOptions { get; set; }

        public int TimeParameter { get; set; }
        public int TaskComplexityParameter { get; set; }

        public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public bool IsChainLengthFixed { get; set; }
        public int MaxChainLength { get; set; }

        public bool IsInteger { get; set; }
        public int DigitsAfterDotSing { get; set; }

        public bool IsRestrictionActivated { get; set; }
        public string RestrictionsString { get; set; }

        public int AmountOfCorrectAnswers { get; set; }
        public int AmountOfWrongAnswers { get; set; }

        public int LongestTimeSpentForExpression { get; set; }
        public string LongestTimeExpressionString { get; set; }

        public int ShortestTimeSpentForExpression { get; set; }
        public string ShortestTimeExpressionString { get; set; }

        public DateTime TaskDateTime { get; set; }
    }
}
