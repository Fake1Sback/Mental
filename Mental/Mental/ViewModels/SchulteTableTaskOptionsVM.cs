using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Mental.Models;
using Mental.Views;
using Mental.ViewModels.PartialViewModels;
using Mental.Services;

namespace Mental.ViewModels
{
    public class SchulteTableTaskOptionsVM : BaseVM
    {
        private INavigation navigation;
        private SchulteTableTaskOptions SchulteTableTaskOptions;
        private App app;
        private TimeOptionsPVM _TimeOptionsPVM;

        private bool _InfoVisibility = false;
        private string _InfoCaption = string.Empty;
        private string _InfoText = string.Empty;

        public SchulteTableTaskOptionsVM(INavigation _navigation)
        {
            app = (App)App.Current;
            navigation = _navigation;
            SchulteTableTaskOptions = app.GetStoredSchulteTableTaskOptions();
            _TimeOptionsPVM = new TimeOptionsPVM(SchulteTableTaskOptions.TaskTimeOptions);
            GridSizeSliderValue = SchulteTableTaskOptions.GridSize;
        }

        //--------------------------------------------------------
        public bool InfoVisibility
        {
            get
            {
                return _InfoVisibility;
            }
            set
            {
                _InfoVisibility = value;
                OnPropertyChanged("InfoVisibility");
            }
        }

        public string InfoCaption
        {
            get
            {
                return _InfoCaption;
            }
            set
            {
                _InfoCaption = value;
                OnPropertyChanged("InfoCaption");
            }
        }

        public string InfoText
        {
            get
            {
                return _InfoText;
            }
            set
            {
                _InfoText = value;
                OnPropertyChanged("InfoText");
            }
        }

        public Command<string> ShowInfoCommand
        {
            get
            {
                return new Command<string>((str) =>
                {
                    InfoVisibility = true;
                    InfoCaption = OptionsInfoDictionary.GetCaption(str);
                    InfoText = OptionsInfoDictionary.GetInfoText(str);
                });
            }
        }

        public Command HideInfoCommand
        {
            get
            {
                return new Command(() =>
                {
                    InfoVisibility = false;
                });
            }
        }

        //---------------------------------------------------

        private double _GridSizeSliderValue;

        public TimeOptionsPVM TimeOptionsPVM
        {
            get
            {
                return _TimeOptionsPVM;
            }
            set
            {
                _TimeOptionsPVM = value;
                OnPropertyChanged("TimeOptionsPVM");
            }
        }

        public double GridSizeSliderValue
        {
            get
            {
                return _GridSizeSliderValue;
            }
            set
            {
                _GridSizeSliderValue = value;
                SchulteTableTaskOptions.GridSize = (int)Math.Round(_GridSizeSliderValue,0);
                if (SchulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                    TimeOptionsPVM.AmountOfTasks = (int)Math.Pow(IntGridSizeSliderValue, 2);
                OnPropertyChanged("GridSizeSliderValue");
                OnPropertyChanged("IntGridSizeSliderValue");
            }
        }

        public int IntGridSizeSliderValue
        {
            get
            {
                return SchulteTableTaskOptions.GridSize;
            }
        }

        public bool EasyModeActivated
        {
            get
            {
                return SchulteTableTaskOptions.IsEasyModeActivated;
            }
        }

        public string EasyModeActivatedImgSrc
        {
            get
            {
                if(SchulteTableTaskOptions.IsEasyModeActivated)
                    return "done_black_18.png";
                else
                    return "";
            }
        }

        public Command EasyModeActivatedSwitchedCommand
        {
            get
            {
                return new Command(() =>
                {
                    SchulteTableTaskOptions.IsEasyModeActivated = !SchulteTableTaskOptions.IsEasyModeActivated;
                    OnPropertyChanged("EasyModeActivated");
                    OnPropertyChanged("EasyModeActivatedImgSrc");
                });
            }
        }
     
        public Command StartCommand
        {
            get
            {
                return new Command(async ()=> {
                   // app.SaveLatestSchulteTableTaskOptions(SchulteTableTaskOptions);
                    ITimeOption timeOption;
                    if (SchulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.CountdownTimer)
                    {
                        timeOption = new CountdownTimeOption(SchulteTableTaskOptions.TaskTimeOptions);
                    }
                    else if (SchulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                    {
                        timeOption = new LimitedTasksTimeOption(SchulteTableTaskOptions.TaskTimeOptions);
                    }
                    else
                    {
                        timeOption = new LastTaskTimeOption(SchulteTableTaskOptions.TaskTimeOptions);
                    }

                    await navigation.PushAsync(new SchulteTableTaskPage(SchulteTableTaskOptions,timeOption));
                });
            }
        }
    }
}
