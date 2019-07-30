using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models.DbModels;
using Mental.Models;
using Xamarin.Forms;

namespace Mental.Models.ListItems
{
    public class DbSchulteTableTaskOptionsListItem : BaseVM
    {
        public DbSchulteTableTaskOptions dbSchulteTableTaskOptions;

        private Color ActiveColor = Color.FromHex("#99ffcc");
        private Color DefaultColor = Color.FromHex("#80aaff");
        private Color _FrameBackgroundColor;

        public DbSchulteTableTaskOptionsListItem(DbSchulteTableTaskOptions _dbSchulteTableTaskOptions)
        {
            dbSchulteTableTaskOptions = _dbSchulteTableTaskOptions;
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

        //------------------------------------

        public string GridSize
        {
            get
            {
                return dbSchulteTableTaskOptions.GridSize + " x " + dbSchulteTableTaskOptions.GridSize;
            }
        }

        public string EasyMode
        {
            get
            {
                if (dbSchulteTableTaskOptions.IsEasyModeActivated)
                    return " Easy Mode ";
                else
                    return "Normal Mode";
            }
        }

        public string EasyModeImgSrc
        {
            get
            {
                if (dbSchulteTableTaskOptions.IsEasyModeActivated)
                    return "Easy_Mode_24.png";
                else
                    return "circle_outline_white_24.png";
            }
        }

        public string TimeOption
        {
            get
            {
                if (dbSchulteTableTaskOptions.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return dbSchulteTableTaskOptions.AmountOfMinutes + " min";
                else if (dbSchulteTableTaskOptions.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return dbSchulteTableTaskOptions.AmountOfTasks + " tasks";
                else
                    return dbSchulteTableTaskOptions.AmountOfSecondsForAnswer + " sec";
            }
        }

        public string TimeOptionsImgSrc
        {
            get
            {
                if (dbSchulteTableTaskOptions.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return "access_time_white_24.png";
                else if (dbSchulteTableTaskOptions.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_24.png";
                else
                    return "Stopwatch_24.png";
            }
        }
    }
}
