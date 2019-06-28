using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models.DbModels;

namespace Mental.Models
{
    public class DbMathTaskListItem : INotifyPropertyChanged
    {
        public DbMathTask dbMathTask;
        private string srcUrl1 = "someurl";
        private string srcUrl2 = "someurl2";

        public DbMathTaskListItem(DbMathTask _dbMathTask)
        {
            dbMathTask = _dbMathTask;
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

        public bool PlusOperation
        {
            get
            {
                return OperationStringChecker("+");
            }
            private set { }
        }

        public bool MinusOperation
        {
            get
            {
                return OperationStringChecker("-");
            }
            private set { }
        }

        public bool MultiplyOperation
        {
            get
            {
                return OperationStringChecker("*");
            }
            private set { }
        }

        public bool DivideOperation
        {
            get
            {
                return OperationStringChecker("/");
            }
            private set { }
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

        public string TimeOptionsImageSrcUrl
        {
            get
            {
                if (dbMathTask.TimeOptions == 0)
                    return srcUrl1;
                else
                    return srcUrl2;
            }
            private set { }
        }

        public string TaskType
        {
            get
            {
                if (dbMathTask.TaskType == 0)
                    return "Find Result";
                else
                    return "Find X";
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
                    return "INT";
                else
                {
                    return "FRACTIONAL ." + dbMathTask.DigitsAfterDotSing;
                }
                    
            }
            private set { }
        }

        public string ChainLengthImageSrcUrl
        {
            get
            {
                if (dbMathTask.IsChainLengthFixed)
                    return srcUrl1;
                else
                    return srcUrl2;
            }
            private set { }
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

        public string RestrictionsString
        {
            get
            {
                return "R";
            }
            private set { }
        }

        public string DateTimeString
        {
            get
            {
                return dbMathTask.TaskDateTime.ToString(@"dd-MM-yy HH:mm");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
