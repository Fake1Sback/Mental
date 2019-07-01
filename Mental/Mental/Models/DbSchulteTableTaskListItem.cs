using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models.DbModels;

namespace Mental.Models
{
    public class DbSchulteTableTaskListItem : INotifyPropertyChanged
    {
        public DbSchulteTableTask DbSchulteTableTask;

        public DbSchulteTableTaskListItem(DbSchulteTableTask _dbSchulteTableTask)
        {
            DbSchulteTableTask = _dbSchulteTableTask;
        }

        public string GridSizeString
        {
            get
            {
                return "#" + DbSchulteTableTask.GridSize;
            }
        }

        public string EasyModeString
        {
            get
            {
                if (DbSchulteTableTask.IsEasyModeActivated)
                    return "Easy Mode: +";
                else
                    return "Easy Mode: -";

            }
        }

        public string TimeOptionString
        {
            get
            {
                if (DbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return DbSchulteTableTask.TaskComplexityParameter + " min";
                else if (DbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return DbSchulteTableTask.TaskComplexityParameter.ToString() + " tasks";
                else

                    return DbSchulteTableTask.TaskComplexityParameter.ToString() + " sec";
            }
        }

        public string EfficiencyString
        {
            get
            {
                if (DbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return (DbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(DbSchulteTableTask.GridSize,2) * 100).ToString() + "%";
                else if (DbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return TimeSpan.FromMilliseconds(DbSchulteTableTask.TimeParameter).ToString(@"mm\:ss");
                else
                    return (DbSchulteTableTask.AmountOfCorrectAnswers / (int)Math.Pow(DbSchulteTableTask.GridSize, 2) * 100).ToString() + "%";
            }
        }

        public string DateTimeString
        {
            get
            {
                return "Date: " + DbSchulteTableTask.TaskDateTime.ToString(@"dd-MM-yy HH:mm");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
