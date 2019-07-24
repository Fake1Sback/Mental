using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models.DbModels
{
    public class DbSchulteTableTask
    {
        public int Id { get; set; }
        public byte TimeOption { get; set; }

        public int TimeParameter { get; set; }
        public int TaskComplexityParameter { get; set; }

        public bool IsEasyModeActivated { get; set; }
        public int GridSize { get; set; }

        public int AmountOfCorrectAnswers { get; set; }
        public int AmountOfWrongAnswers { get; set; }

        public DateTime TaskDateTime { get; set; }
    }
}
