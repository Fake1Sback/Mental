using Mental.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;
using Xamarin.Forms;

namespace Mental.Models
{
    public class DbMathTaskOptionsListItem : BaseVM
    {
        public DbMathTaskOptions dbMathTaskOptions;

        private Color ActiveColor = Color.FromHex("#99ffcc");
        private Color DefaultColor = Color.FromHex("#80aaff");
        private Color _FrameBackgroundColor;

        public DbMathTaskOptionsListItem(DbMathTaskOptions _dbMathTaskOptions)
        {
            dbMathTaskOptions = _dbMathTaskOptions;
            SetDefaultColor();
        }

        //----------------------------------------
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

        //---------------------------------------

        public string Operations
        {
            get
            {
                return dbMathTaskOptions.Operations;
            }
        }

        public string MaxChainLength
        {
            get
            {
                return "Max: " + dbMathTaskOptions.MaxChainLength;
            }
        }

        public string ChainLengthImgSrc
        {
            get
            {
                if(dbMathTaskOptions.IsChainLengthFixed)
                    return "Chain_24.png";
                else
                    return "Broken_Chain_2_24.png";
            }
        }

        public int MinValue
        {
            get
            {
                return dbMathTaskOptions.MinValue;
            }
        }

        public int MaxValue
        {
            get
            {
                return dbMathTaskOptions.MaxValue;
            }
        }

        public string DataType
        {
            get
            {
                if (dbMathTaskOptions.IsIntegerNumbers)
                    return "INTEGER";
                else
                {
                    string str = ".";
                    for (int i = 0; i < dbMathTaskOptions.DigitsAfterDotSign; i++)
                    {
                        str += "X";
                    }
                    return "FRACTIONAL " + str;
                }

            }
        }

        public bool RestrictionsVisibility
        {
            get
            {
                return dbMathTaskOptions.IsRestrictionActivated;
            }
        }
       
        public string TaskType
        {
            get
            {
                if (dbMathTaskOptions.TaskType == 0)
                    return "Result";
                else
                    return " X ";
            }
        }

        public string TimeOption
        {
            get
            {
                if (dbMathTaskOptions.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return dbMathTaskOptions.AmountOfMinutes + " min";
                else if (dbMathTaskOptions.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return dbMathTaskOptions.AmountOfTasks + " tasks";
                else
                    return dbMathTaskOptions.AmountOfSecondsForAnswer + " sec";
            }
        }

        public string TimeOptionsImgSrc
        {
            get
            {
                if (dbMathTaskOptions.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return "access_time_white_24.png";
                else if (dbMathTaskOptions.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_24.png";
                else
                    return "Stopwatch_24.png";
            }
        }
    }

    public enum Favourites
    {
        MathOptionsFavourite,
        SchulteTableOptionsFavourite,
        StroopOptionsFavourite
    }
}
