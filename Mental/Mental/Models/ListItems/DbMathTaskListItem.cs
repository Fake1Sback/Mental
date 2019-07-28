using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models.DbModels;
using Mental.ViewModels;
using Xamarin.Forms;

namespace Mental.Models
{
    public class DbMathTaskListItem : BaseVM
    {
        public DbMathTask dbMathTask;

        private Color ActiveColor = Color.FromHex("#99ffcc");
        private Color DefaultColor = Color.FromHex("#80aaff");

        private Color _FrameBackgroundColor;

        public DbMathTaskListItem(DbMathTask _dbMathTask)
        {
            dbMathTask = _dbMathTask;
            _FrameBackgroundColor = DefaultColor;
        }

        public Color FrameBackgroundColor
        {
            get
            {
                return _FrameBackgroundColor;
            }
            set
            {
                _FrameBackgroundColor = value;
                OnPropertyChanged("FrameBackgroundColor");
            }
        }

        public void SetActiveColor()
        {
            FrameBackgroundColor = ActiveColor;
        }

        public void SetDefaultColor()
        {
            FrameBackgroundColor = DefaultColor;
        }

        public string Operations
        {
            get
            {
                return dbMathTask.Operations;
            }
            set
            {
                dbMathTask.Operations = value;
                OnPropertyChanged("Operations");
            }
        }     

        private bool OperationStringChecker(string value)
        {
            if (dbMathTask.Operations.Contains(value))
                return true;
            else
                return false;
        }

        public int MinValue
        {
            get
            {
                return dbMathTask.MinValue;
            }
            private set { }
        }

        public int MaxValue
        {
            get
            {
                return dbMathTask.MaxValue;
            }
            private set { }
        }

        public string TimeOption
        {
            get
            {
                if (dbMathTask.TimeOptions == 0)
                    return dbMathTask.TaskComplexityParameter + " min";
                else if (dbMathTask.TimeOptions == 1)
                    return dbMathTask.TaskComplexityParameter.ToString() + " tasks";
                else
                    return dbMathTask.TaskComplexityParameter.ToString() + " sec";
            }
            private set { }
        }

        public string TimeOptionImgSrc
        {
            get
            {
                if (dbMathTask.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_18.png";
                else
                    return "access_time_white_18.png";
            }
        }

        public string TimeOptionsImageSrcUrl
        {
            get
            {
                if (dbMathTask.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_18.png";
                else if (dbMathTask.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return "access_time_white_18.png";
                else
                    return "Stopwatch_18.png";
            }
        }

        public string TaskType
        {
            get
            {
                if (dbMathTask.TaskType == 0)
                    return "Result";
                else
                    return " X ";
            }
        }

        public int CorrentAnswers
        {
            get
            {
                return dbMathTask.AmountOfCorrectAnswers;
            }
            private set { }
        }

        public int WrongAnswers
        {
            get
            {
                return dbMathTask.AmountOfWrongAnswers;
            }
            private set { }
        }

        public string DataType
        {
            get
            {
                if (dbMathTask.IsInteger)
                    return "INTEGER";
                else
                {
                    string str = ".";
                    for(int i = 0;i < dbMathTask.DigitsAfterDotSing;i++)
                    {
                        str += "X";
                    }
                    return "FRACTIONAL " + str;
                }
                    
            }
            private set { }
        }

        public string ChainLengthImgSrc
        {
            get
            {
                if (dbMathTask.IsChainLengthFixed)
                    return "Chain_18.png";
                else
                    return "Broken_chain_18.png";
            }
        }

        public int MaxChainLength
        {
            get
            {
                return dbMathTask.MaxChainLength;
            }
            private set { }
        }

        public bool RestrictionsVisibility
        {
            get
            {
                return dbMathTask.IsRestrictionActivated;
            }
        }

        public string DateTimeString
        {
            get
            {
                return dbMathTask.TaskDateTime.ToString(@"dd-MM-yy HH:mm");
            }
        }
    }
}
