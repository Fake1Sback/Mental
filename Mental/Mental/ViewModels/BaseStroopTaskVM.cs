using Mental.Models;
using Mental.Models.DbModels;
using Mental.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public abstract class BaseStroopTaskVM : BaseVM
    {
        protected ITimeOption timeOption;
        protected INavigation navigation;
        protected StroopTaskOptions stroopTaskOptions;

        private bool _QuestionLabelVisibility;
        private bool _YesNoLayoutVisibility;
        private string _QuestionLabelString;
        private Color _QuestionLabelColor;

        private BaseArrayProperty<string> _ColorButtonText;
        private BaseArrayProperty<Color> _ColorButtonColor;
        private BaseArrayProperty<bool> _ColorButtonLayoutsVisibility;

        private int _AmountOfCorrectAnswers;
        private int _AmountOfWrongAnswers;

        private int _ColorButtonFontSize;
        private int _ColorButtonHeight;
        private StackOrientation _FirstColorButtonsStackLayoutOrientation;

        protected bool VMTimerBlocker = false;
        protected StroopTaskPage stroopTaskPage;

        protected Color[] colors = new Color[]
       {
            Color.Blue,
            Color.Red,
            Color.DarkGreen,
            Color.Yellow,
            Color.Orange,
            Color.DarkViolet,
            Color.Gray,
            Color.FromHex("#99ff99"),
            Color.Black,
            Color.Pink,
            Color.Brown
       };
        protected string[] colorsStrings = new string[]
        {
            "Blue",
            "Red",
            "Green",
            "Yellow",
            "Orange",
            "Violet",
            "Gray",
            "Mint",
            "Black",
            "Pink",
            "Brown"
        };

        protected List<Color> colorsList;
        protected List<string> colorsStringsList;

        public BaseStroopTaskVM(INavigation _navigation, StroopTaskOptions _stroopTaskOptions, ITimeOption _timeOption,StroopTaskPage _stroopTaskPage)
        {
            timeOption = _timeOption;
            navigation = _navigation;
            stroopTaskOptions = _stroopTaskOptions;
            stroopTaskPage = _stroopTaskPage;

            Dictionary<int, bool> dic3 = new Dictionary<int, bool>();
            for (int i = 0; i < 5; i++)
            {
                if (i + 1 <= stroopTaskOptions.ButtonsAmount / 2)
                    dic3.Add(i, true);
                else
                    dic3.Add(i, false);
            }
            _ColorButtonLayoutsVisibility = new BaseArrayProperty<bool>(dic3);

            Dictionary<int, string> dic1 = new Dictionary<int, string>();
            dic1.Add(0, "First");
            dic1.Add(1, "Second");
            dic1.Add(2, "Third");
            dic1.Add(3, "Forth");

            Dictionary<int, Color> dic2 = new Dictionary<int, Color>();
            dic2.Add(0, Color.Blue);
            dic2.Add(1, Color.Red);
            dic2.Add(2, Color.Green);
            dic2.Add(3, Color.Violet);

            _ColorButtonText = new BaseArrayProperty<string>(dic1);
            _ColorButtonColor = new BaseArrayProperty<Color>(dic2);

            ColorButtonFontSize = 25 - _stroopTaskOptions.ButtonsAmount;
            ColorButtonHeight = 120 - _stroopTaskOptions.ButtonsAmount * 7;

            StartTimerCountdown();
        }

        //----------------------------------------------------

        protected abstract void GenerateTask();

        //-----------------------------------------------------

        public bool QuestionLabelVisibility
        {
            get
            {
                return _QuestionLabelVisibility;
            }
            set
            {
                _QuestionLabelVisibility = value;
                OnPropertyChanged("QuestionLabelVisibility");
            }
        }

        public string QuestionLabelString
        {
            get
            {
                return _QuestionLabelString;
            }
            set
            {
                _QuestionLabelString = value;
                OnPropertyChanged("QuestionLabelString");
            }
        }

        public Color QuestrionLabelTextColor
        {
            get
            {
                return _QuestionLabelColor;
            }
            set
            {
                _QuestionLabelColor = value;
                OnPropertyChanged("QuestrionLabelTextColor");
            }
        }


        public int ColorButtonFontSize
        {
            get
            {
                return _ColorButtonFontSize;
            }
            set
            {
                _ColorButtonFontSize = value;
                OnPropertyChanged("ColorButtonFontSize");
            }
        }

        public int ColorButtonHeight
        {
            get
            {
                return _ColorButtonHeight;
            }
            set
            {
                _ColorButtonHeight = value;
                OnPropertyChanged("ColorButtonHeight");
            }
        }

        public StackOrientation FirstColorButtonsStackLayoutOrientation
        {
            get
            {
                if (stroopTaskOptions.ButtonsAmount <= 2)
                    return StackOrientation.Vertical;
                else
                    return StackOrientation.Horizontal;
            }
        }

        public int ColorButtonWidth
        {
            get
            {
                if (stroopTaskOptions.ButtonsAmount <= 2)
                    return 290;
                else
                    return 140;
            }
        }


        public bool YesNoLayoutVisibility
        {
            get
            {
                return _YesNoLayoutVisibility;
            }
            set
            {
                _YesNoLayoutVisibility = value;
                OnPropertyChanged("YesNoLayoutVisibility");
            }
        }

        public string TimerValue
        {
            get
            {
                return timeOption.GetTimeString();
            }
        }

        public BaseArrayProperty<bool> ColorButtonLayoutsVisibility
        {
            get
            {
                return _ColorButtonLayoutsVisibility;
            }
            set
            {
                _ColorButtonLayoutsVisibility = value;
            //    OnPropertyChanged("ColorButtonLayoutsVisibility");
            }
        }

        public BaseArrayProperty<string> ColorButtonText
        {
            get
            {
                return _ColorButtonText;
            }
            set
            {
                _ColorButtonText = value;
             //   OnPropertyChanged("ColorButtonText");
            }
        }

        public BaseArrayProperty<Color> ColorButtonColor
        {
            get
            {
                return _ColorButtonColor;
            }
            set
            {
                _ColorButtonColor = value;
             //   OnPropertyChanged("ColorButtonColor");
            }
        }

        public Command ColorButtonClickedCommand { get; set; }
        public Command YesButtonClickedCommand { get; set; }
        public Command NoButtonClickedCommand { get; set; }

        public int AmountOfCorrectAnswers
        {
            get
            {
                return _AmountOfCorrectAnswers;
            }
            set
            {
                _AmountOfCorrectAnswers = value;
                OnPropertyChanged("AmountOfCorrectAnswers");
            }
        }

        public int AmountOfWrongAnswers
        {
            get
            {
                return _AmountOfWrongAnswers;
            }
            set
            {
                _AmountOfWrongAnswers = value;
                OnPropertyChanged("AmountOfWrongAnswers");
            }
        }

        protected void StartTimerCountdown()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (timeOption.CheckTimerEnd() && !VMTimerBlocker)
                {
                    timeOption.TimerWork();
                    OnPropertyChanged("TimerValue");
                    return true;
                }
                else
                    return false;
            });
        }

        public Command RestartCommand
        {
            get
            {
                return new Command(() =>
                {
                    stroopTaskPage.ShowTaskFrame();
                    AmountOfCorrectAnswers = 0;
                    AmountOfWrongAnswers = 0;
                    GenerateTask();
                    timeOption.TimerRestart();
                    VMTimerBlocker = false;
                    StartTimerCountdown();
                });
            }
        }

        public Command NavigateToStatisticsCommand
        {
            get
            {
                return new Command(async () =>
                {
                    DbStroopTask dbStroopTask = new DbStroopTask
                    {
                        AmountOfButtons = stroopTaskOptions.ButtonsAmount,
                        AmountOfCorrectAnswers = AmountOfCorrectAnswers,
                        AmountOfWrongAnswers = AmountOfWrongAnswers,
                        StroopTaskOption = (byte)stroopTaskOptions.StroopTaskType,
                        TimeOption = (byte)stroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption,
                        TaskDateTime = DateTime.Now
                    };

                    if (stroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.CountdownTimer)
                    {
                        dbStroopTask.TimeParameter = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfMinutes;
                        dbStroopTask.TaskComplexityParameter = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfMinutes;
                    }
                    else if (stroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption == TimeOptions.FixedAmountOfOperations)
                    {
                        dbStroopTask.TimeParameter = timeOption.GetMillis();
                        dbStroopTask.TaskComplexityParameter = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfTasks;
                    }
                    else
                    {
                        dbStroopTask.TimeParameter = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfSecondsForAnswer;
                        dbStroopTask.TaskComplexityParameter = stroopTaskOptions.TaskTimeOptionsContainer.AmountOfSecondsForAnswer;
                    }

                    await navigation.PushAsync(new StroopTaskSimilarStatisticsPage(dbStroopTask, true));
                });
            }
        }
    }
}
