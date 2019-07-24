using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models.DbModels
{
    public class DbStroopTask
    {
        public int Id { get; set; }
        public byte TimeOption { get; set; }

        public int TimeParameter { get; set; }
        public int TaskComplexityParameter { get; set; }

        public byte StroopTaskOption { get; set; }
        public int AmountOfButtons { get; set; }

        public int AmountOfCorrectAnswers { get; set; }
        public int AmountOfWrongAnswers { get; set; }

        public DateTime TaskDateTime { get; set; }
    }
}
