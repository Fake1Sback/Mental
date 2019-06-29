using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class TaskRestrictions
    {
        //public bool IsRestrictionsActivated = false;
        public OperationRestriction[] restrictions;

        public TaskRestrictions()
        {
            restrictions = new OperationRestriction[4]
            {
                new OperationRestriction{OperationName = "+"},
                new OperationRestriction{OperationName = "-"},
                new OperationRestriction{OperationName = "*"},
                new OperationRestriction{OperationName = "/"}
            };
        }

        public static string GetTaskRestrictionsString(OperationRestriction[] _restrictions)
        {
            StringBuilder stringBuilder = new StringBuilder(20);
            for (int i = 0; i < _restrictions.Length; i++)
            {
                if (_restrictions[i].IsBlockActivated)
                    stringBuilder.Append("1");
                else
                    stringBuilder.Append("0");

                stringBuilder.Append(_restrictions[i].Digit1Restriction.ToString());
                if (_restrictions[i].IsDigit1HardRestriction)
                    stringBuilder.Append("1");
                else
                    stringBuilder.Append("0");
                stringBuilder.Append(_restrictions[i].Digit2Restriction.ToString());
                if (_restrictions[i].IsDigit2HardRestriction)
                    stringBuilder.Append("1");
                else
                    stringBuilder.Append("0");
            }
            return stringBuilder.ToString();
        }

        public static OperationRestriction[] GetTaskRestrictionFromString(string Code)
        {
            OperationRestriction[] _restrictions = new OperationRestriction[4];
            string[] EncodedOperationRestrictions = new string[4];
            for (int i = 0; i < 4; i++)
            {
                EncodedOperationRestrictions[i] = Code.Substring(5 * i, 5);
            }
            for (int i = 0; i < 4; i++)
            {
                if (_restrictions[i] == null)
                    _restrictions[i] = new OperationRestriction();

                switch(i)
                {
                    case 0:
                        _restrictions[i].OperationName = "+";
                        break;
                    case 1:
                        _restrictions[i].OperationName = "-";
                        break;
                    case 2:
                        _restrictions[i].OperationName = "*";
                        break;
                    case 3:
                        _restrictions[i].OperationName = "/";
                        break;
                    default:
                        _restrictions[i].OperationName = "+";
                        break;
                }

                if (EncodedOperationRestrictions[i][0] == '1')
                    _restrictions[i].IsBlockActivated = true;
                else
                    _restrictions[i].IsBlockActivated = false;
                _restrictions[i].Digit1Restriction = Int32.Parse(EncodedOperationRestrictions[i][1].ToString());
                if (EncodedOperationRestrictions[i][2] == '1')
                    _restrictions[i].IsDigit1HardRestriction = true;
                else
                    _restrictions[i].IsDigit1HardRestriction = false;
                _restrictions[i].Digit2Restriction = Int32.Parse(EncodedOperationRestrictions[i][3].ToString());
                if (EncodedOperationRestrictions[i][4] == '1')
                    _restrictions[i].IsDigit2HardRestriction = true;
                else
                    _restrictions[i].IsDigit2HardRestriction = false;
            }
            return _restrictions;
        }
    }
}
