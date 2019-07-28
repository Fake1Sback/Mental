using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;
using Xamarin.Forms;

namespace Mental.Models.ListItems
{
    public class DbStroopTaskOptionsListItem : BaseVM
    {
        public DbStroopTaskOptions dbStroopTaskOptions;

        private Color ActiveColor = Color.FromHex("#99ffcc");
        private Color DefaultColor = Color.FromHex("#80aaff");
        private Color _FrameBackgroundColor;

        public DbStroopTaskOptionsListItem(DbStroopTaskOptions _dbStroopTaskOptions)
        {
            dbStroopTaskOptions = _dbStroopTaskOptions;
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

        //------------------------------------------

        public int AmountOfButtons
        {
            get
            {
                return dbStroopTaskOptions.ButtonsAmount;
            }
        }

        public string StroopTaskTypeImgSrc
        {
            get
            {
                if (dbStroopTaskOptions.StroopTaskType == (byte)StroopTaskType.FindOneCorrect)
                    return "Find_One_24.png";
                else if (dbStroopTaskOptions.StroopTaskType == (byte)StroopTaskType.TrueOrFalse)
                    return "True_False_24.png";
                else
                    return "Color_By_Text_24.png";
            }
        }

        public string StroopTaskTypeString
        {
            get
            {
                if (dbStroopTaskOptions.StroopTaskType == (byte)StroopTaskType.FindOneCorrect)
                    return "Find 1 Correct";
                else if (dbStroopTaskOptions.StroopTaskType == (byte)StroopTaskType.TrueOrFalse)
                    return "True / False";
                else
                    return "Find Color by Text";
            }
        }

        public string TimeOption
        {
            get
            {
                if (dbStroopTaskOptions.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return dbStroopTaskOptions.AmountOfMinutes + " min";
                else if (dbStroopTaskOptions.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return dbStroopTaskOptions.AmountOfTasks + " tasks";
                else
                    return dbStroopTaskOptions.AmountOfSecondsForAnswer + " sec";
            }
        }

        public string TimeOptionsImgSrc
        {
            get
            {
                if (dbStroopTaskOptions.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return "access_time_white_18.png";
                else if (dbStroopTaskOptions.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_18.png";
                else
                    return "Stopwatch_18.png";
            }
        }
    }
}
