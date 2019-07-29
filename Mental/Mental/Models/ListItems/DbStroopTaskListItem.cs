using Mental.Models.DbModels;
using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.Models
{
    public class DbStroopTaskListItem : BaseVM
    {
        public DbStroopTask DbStroopTask;

        private Color ActiveColor = Color.FromHex("#99ffcc");
        private Color DefaultColor = Color.FromHex("#80aaff");

        private Color _FrameBackgroundColor;

        public DbStroopTaskListItem(DbStroopTask _dbStroopTask)
        {
            DbStroopTask = _dbStroopTask;
            SetDefaultColor();
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

        public string TaskTypeString
        {
            get
            {
                if (DbStroopTask.StroopTaskOption == (byte)StroopTaskType.FindOneCorrect)
                    return "Find 1 Correct";
                else if (DbStroopTask.StroopTaskOption == (byte)StroopTaskType.TrueOrFalse)
                    return "True/False";
                else
                    return "Find Color by Text";
            }
        }

        public string TaskTypeImgSrc
        {
            get
            {
                if (DbStroopTask.StroopTaskOption == (byte)StroopTaskType.FindOneCorrect)
                    return "Find_One_24.png";
                else if (DbStroopTask.StroopTaskOption == (byte)StroopTaskType.TrueOrFalse)
                    return "True_False_24.png";
                else
                    return "Color_by_Text_24.png";
            }
        }

        public int AmountOfButtons
        {
            get
            {
                return DbStroopTask.AmountOfButtons;
            }
        }

        public string TimeOptionImgSrc
        {
            get
            {
                if (DbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_24.png";
                else if (DbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return "access_time_white_24.png";
                else
                    return "Stopwatch_24.png";
            }
        }

        public string TimeOptionString
        {
            get
            {

                if (DbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return DbStroopTask.TaskComplexityParameter + " min";
                else if (DbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return DbStroopTask.TaskComplexityParameter.ToString() + " tasks";
                else
                    return DbStroopTask.TaskComplexityParameter.ToString() + " sec";
            }
        }

        public int CorrectAnswers
        {
            get
            {
                return DbStroopTask.AmountOfCorrectAnswers;
            }
        }

        public int WrongAnswers
        {
            get
            {
                return DbStroopTask.AmountOfWrongAnswers;
            }
        }

        public string DateTimeString
        {
            get
            {
                return "Date: " + DbStroopTask.TaskDateTime.ToString(@"dd-MM-yy HH:mm");
            }
        }
    }
}
