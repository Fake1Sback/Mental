using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class OperationRestriction
    {
        public string OperationName;
        public bool IsBlockActivated = false;
        public int Digit1Restriction = 1, Digit2Restriction = 1;
        public bool IsDigit1HardRestriction = true, IsDigit2HardRestriction = true;
    }
}
