using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Mental.Models.DbModels;
using Mental.ViewModels;
using Xamarin.Forms;

namespace Mental.Models
{
    public class DbSchulteTableTaskListItem : BaseVM
    {
        public DbSchulteTableTask DbSchulteTableTask;

        private Color ActiveColor = Color.FromHex("#99ffcc");
        private Color DefaultColor = Color.FromHex("#80aaff");

        private Color _FrameBackgroundColor;

        public DbSchulteTableTaskListItem(DbSchulteTableTask _dbSchulteTableTask)
        {
            DbSchulteTableTask = _dbSchulteTableTask;
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

        public string GridSizeString
        {
            get
            {
                return DbSchulteTableTask.GridSize + " x " + DbSchulteTableTask.GridSize;
            }
        }

        public string EasyModeString
        {
            get
            {
                if (DbSchulteTableTask.IsEasyModeActivated)
                    return "Easy Mode";
                else
                    return "Normal Mode";

            }
        }

        public string EasyModeSrc
        {
            get
            {
                if (DbSchulteTableTask.IsEasyModeActivated)
                    return "Easy_Mode_18.png";
                else
                    return "circle_outline_white_18.png";
            }
        }

        public string TimeOptionImgSrc
        {
            get
            {
                if (DbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_18.png";
                else
                    return "access_time_white_18.png";
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
                return DbSchulteTableTask.GetEfficiencyParameterString();
            }
        }

        public string DateTimeString
        {
            get
            {
                return "Date: " + DbSchulteTableTask.TaskDateTime.ToString(@"dd-MM-yy HH:mm");
            }
        }
    }
}
