using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;
using Microcharts;
using Entry = Microcharts.Entry;
using Mental.Models;
using Mental.Models.DbModels;
using System.Linq;
using Mental.Views;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Mental.ViewModels
{
    public class GeneralStatisticsVM : BaseVM
    {
        private int GeneralAmountOfRecords;
        private int AmountOfDataInListView = 5;

        private int _StartPaginationIndex;
        private int _CurrentPaginationIndex;
        private int _LastPaginationIndex;

        private string Color1 = "#0040ff"; //DarkBlue        
        private string Color2 = "#F55B70"; //Red
        private string Color3 = "#009933"; //Green
        private string Color4 = "#ff9900"; //Orange
        private string Color5 = "#8000ff"; //Violet
        private string Color6 = "#2eb8b8"; //Aqua
        private string Color7 = "#cc4400"; //Brown

        private List<DbMathTaskListItem> _mathTaskListItems;
        private DbMathTaskListItem _SelectedMathTaskListItem;

        private INavigation navigation;

        private List<Entry> _TimeOptionsChart;
        private List<Entry> _TaskTypeChart;
        private List<Entry> _DataTypeChart;
        private List<Entry> _RestrictionsChart;
        private List<Entry> _OperationsChart;
        private List<Entry> _ChainLengthChart;
        private List<Entry> _MaxChainLengthChart;

        private int _ListViewHeightRequest;

        public GeneralStatisticsVM(INavigation _navigation)
        {
            navigation = _navigation;
            using (var db = new ApplicationContext("mental.db"))
            {
                GeneralAmountOfRecords = db.mathTasks.Count();
                InitializeTimeOptionsChart(db);
                InitializeTaskTypeOptionsChart(db);
                InitializeDataTypeOptionsChart(db);
                InitializeRestrictionsOptionsChart(db);
                InitializeOperationsModeOptionsChart(db);
                InitializeFixedChainLengthOptionsChart(db);
                InitializeMaxChainLengthOptionsChart(db);
            }

            int AmountOfPages = GeneralAmountOfRecords / AmountOfDataInListView;
            if (GeneralAmountOfRecords % AmountOfDataInListView == 0)
                AmountOfPages -= 1;

            StartPaginationIndex = 0;
            CurrentPaginationIndex = 0;
            LastPaginationIndex = AmountOfPages;

            LoadMoreDbMathTaskInfo();
            LoadMoreDbMathTasksCommand = new Command(LoadMoreDbMathTaskInfo);
            LoadSimilarCommand = new Command(LoadSimilar);
        }

        public List<DbMathTaskListItem> mathTaskListItems
        {
            get
            {
                return new List<DbMathTaskListItem>(_mathTaskListItems);
            }
        }

        public DbMathTaskListItem SelectedMathTaskListItem
        {
            set
            {
                if (value != null)
                {
                    foreach (DbMathTaskListItem item in _mathTaskListItems)
                        item.SetDefaultColor();

                    _SelectedMathTaskListItem = value;
                    _SelectedMathTaskListItem.SetActiveColor();
                }               
            }
        }

        public DonutChart TimeOptionsChart
        {
            get
            {
                return new DonutChart() { Entries = _TimeOptionsChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = 20 };
            }
            set
            {
                _TimeOptionsChart = value.Entries.ToList();
                OnPropertyChanged("TimeOptionsChart");
            }
        }

        public DonutChart TaskTypeChart
        {
            get
            {
                return new DonutChart() { Entries = _TaskTypeChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = 20};
            }
            set
            {
                _TaskTypeChart = value.Entries.ToList();
                OnPropertyChanged("TaskTypeChart");
            }
        }

        public DonutChart DataTypeChart
        {
            get
            {
                return new DonutChart() { Entries = _DataTypeChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = 20};
            }
            set
            {
                _DataTypeChart = value.Entries.ToList();
                OnPropertyChanged("DataTypeChart");
            }
        }

        public DonutChart RestrictionsChart
        {
            get
            {
                return new DonutChart() { Entries = _RestrictionsChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = 20 };
            }
            set
            {
                _RestrictionsChart = value.Entries.ToList();
                OnPropertyChanged("RestrictionsChart");
            }
        }

        public PointChart OperationsChart
        {
            get
            {
                return new PointChart() { Entries = _OperationsChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), PointAreaAlpha = 200, LabelTextSize = 20 };
            }
            set
            {
                _OperationsChart = value.Entries.ToList();
                OnPropertyChanged("OperationsChart");
            }
        }

        public DonutChart ChainLengthChart
        {
            get
            {
                return new DonutChart() { Entries = _ChainLengthChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = 20 };
            }
            set
            {
                _ChainLengthChart = value.Entries.ToList();
                OnPropertyChanged("ChainLengthChart");
            }
        }

        public PointChart MaxChainLengthChart
        {
            get
            {
                return new PointChart() { Entries = _MaxChainLengthChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), PointAreaAlpha = 200 };
            }
            set
            {
                _MaxChainLengthChart = value.Entries.ToList();
                OnPropertyChanged("MaxChainLengthChart");
            }
        }


        private async void InitializeTimeOptionsChart(ApplicationContext db)
        {           
            int AmountOfCountdownOptionRecords = await db.mathTasks.Where(t => t.TimeOptions == 0).CountAsync();
            int AmountOfLimitedTasksOptionsRecords = await db.mathTasks.Where(t => t.TimeOptions == 1).CountAsync();
            int AmountOfLastTaskOptionsRecords = GeneralAmountOfRecords - (AmountOfCountdownOptionRecords + AmountOfLimitedTasksOptionsRecords);

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfCountdownOptionRecords)
                {
                    ValueLabel = "Countdown",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new Entry(AmountOfLimitedTasksOptionsRecords)
                {
                    ValueLabel = "Limited Tasks",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                },
                new Entry(AmountOfLastTaskOptionsRecords)
                {
                    ValueLabel = "Last Task",
                    Color = SkiaSharp.SKColor.Parse(Color3)
                }
            };

            _TimeOptionsChart = entries;
            OnPropertyChanged("TimeOptionsChart");
        }

        private async void InitializeTaskTypeOptionsChart(ApplicationContext db)
        {
            int AmountOfFindResultOptionsRecords = await db.mathTasks.Where(t => t.TaskType == 0).CountAsync();       
            int AmountOfFindXOptionsRecords = GeneralAmountOfRecords - AmountOfFindResultOptionsRecords;

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfFindResultOptionsRecords)
                {
                    ValueLabel = "Find Result",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new Entry(AmountOfFindXOptionsRecords)
                {
                    ValueLabel = "Find X",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                }
            };

            _TaskTypeChart = entries;
            OnPropertyChanged("TaskTypeChart");
        }

        private async void InitializeDataTypeOptionsChart(ApplicationContext db)
        {
            int AmountOfIntOptionsRecords = await db.mathTasks.Where(t => t.IsInteger == true).CountAsync();
            int AmountOfFractionalOptionsRecords = GeneralAmountOfRecords - AmountOfIntOptionsRecords;

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfIntOptionsRecords)
                {
                    ValueLabel = "Integer",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new Entry(AmountOfFractionalOptionsRecords)
                {
                    ValueLabel = "Fractional",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                }
            };

            _DataTypeChart = entries;
            OnPropertyChanged("DataTypeChart");
        }

        private async void InitializeRestrictionsOptionsChart(ApplicationContext db)
        {
            int AmountOfRestrictedRecords = await db.mathTasks.Where(t => t.IsRestrictionActivated == true).CountAsync();
            int AmountOfNoSpecialModeOptionsRecords = GeneralAmountOfRecords - AmountOfRestrictedRecords;

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfRestrictedRecords)
                {
                    ValueLabel = "Restricted",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new Entry(AmountOfNoSpecialModeOptionsRecords)
                {
                    ValueLabel = "No Restrictions",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                }
            };

            _RestrictionsChart = entries;
            OnPropertyChanged("RestrictionsChart");
        }

        private async void InitializeOperationsModeOptionsChart(ApplicationContext db)
        {
            int AmountOfPlusOperations = await db.mathTasks.Where(t => t.Operations.Contains("+")).CountAsync();
            int AmountOfMinusOperations = await db.mathTasks.Where(t => t.Operations.Contains("-")).CountAsync();
            int AmountOfMultiplyOperations = await db.mathTasks.Where(t => t.Operations.Contains("*")).CountAsync();
            int AmountOfDivideOperations = await db.mathTasks.Where(t => t.Operations.Contains("/")).CountAsync();

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfPlusOperations)
                {
                    ValueLabel = AmountOfPlusOperations.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color1),
                    Label = "+",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMinusOperations)
                {
                    ValueLabel = AmountOfMinusOperations.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color2),
                    Label = "-",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMultiplyOperations)
                {
                    ValueLabel = AmountOfMultiplyOperations.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color3),
                    Label = "*",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                 new Entry(AmountOfDivideOperations)
                {
                    ValueLabel = AmountOfDivideOperations.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color4),
                    Label = "/",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                }
            };

            _OperationsChart = entries;
            OnPropertyChanged("OperationsChart");
        }

        private async void InitializeFixedChainLengthOptionsChart(ApplicationContext db)
        {
            int AmountOfFixedChainLengthOptionRecords = await db.mathTasks.Where(t => t.IsChainLengthFixed == true).CountAsync();
            int AmountOfNotFixedChainLengthOptionsRecords = GeneralAmountOfRecords - AmountOfFixedChainLengthOptionRecords;

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfFixedChainLengthOptionRecords)
                {
                    ValueLabel = "Fixed",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new Entry(AmountOfNotFixedChainLengthOptionsRecords)
                {
                    ValueLabel = "Not Fixed",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                }
            };

            _ChainLengthChart = entries;
            OnPropertyChanged("ChainLengthChart");
        }

        private async void InitializeMaxChainLengthOptionsChart(ApplicationContext db)
        {
            int AmountOfMax2ChainLength = await db.mathTasks.Where(t => t.MaxChainLength == 2).CountAsync();
            int AmountOfMax3ChainLength = await db.mathTasks.Where(t => t.MaxChainLength == 3).CountAsync();
            int AmountOfMax4ChainLength = await db.mathTasks.Where(t => t.MaxChainLength == 4).CountAsync();
            int AmountOfMax5ChainLength = await db.mathTasks.Where(t => t.MaxChainLength == 5).CountAsync();
            int AmountOfMax6ChainLength = await db.mathTasks.Where(t => t.MaxChainLength == 6).CountAsync();
            int AmountOfMax7ChainLength = await db.mathTasks.Where(t => t.MaxChainLength == 7).CountAsync();
            int AmountOfMax8ChainLength = GeneralAmountOfRecords - (AmountOfMax2ChainLength + AmountOfMax3ChainLength + AmountOfMax4ChainLength + AmountOfMax5ChainLength + AmountOfMax6ChainLength + AmountOfMax7ChainLength);

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfMax2ChainLength)
                {
                    ValueLabel = AmountOfMax2ChainLength.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color1),
                    Label = "2 OP",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMax3ChainLength)
                {
                    ValueLabel = AmountOfMax3ChainLength.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color2),
                    Label = "3 OP",
                     TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMax4ChainLength)
                {
                    ValueLabel = AmountOfMax4ChainLength.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color3),
                    Label = "4 OP",
                     TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMax5ChainLength)
                {
                    ValueLabel = AmountOfMax5ChainLength.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color4),
                    Label = "5 OP",
                     TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMax6ChainLength)
                {
                    ValueLabel = AmountOfMax6ChainLength.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color5),
                    Label = "6 OP",
                     TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMax7ChainLength)
                {
                    ValueLabel = AmountOfMax7ChainLength.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color6),
                    Label = "7 OP",
                     TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new Entry(AmountOfMax8ChainLength)
                {
                    ValueLabel = AmountOfMax8ChainLength.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color7),
                    Label = "8 OP",
                     TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
            };

            _MaxChainLengthChart = entries;
            OnPropertyChanged("MaxChainLengthChart");
        }

        //-------------------- Pagination -------------------------------------------

        public int StartPaginationIndex
        {
            get
            {
                return _StartPaginationIndex;
            }
            set
            {
                _StartPaginationIndex = value;
                OnPropertyChanged("StartPaginationIndex");
            }
        }

        public int CurrentPaginationIndex
        {
            get
            {
                return _CurrentPaginationIndex;
            }
            set
            {
                _CurrentPaginationIndex = value;
                OnPropertyChanged("CurrentPaginationIndex");
            }
        }

        public int LastPaginationIndex
        {
            get
            {
                return _LastPaginationIndex;
            }
            set
            {
                _LastPaginationIndex = value;
                OnPropertyChanged("LastPaginationIndex");
            }
        }


        public Command StartPaginationButtonCommand
        {
            get
            {
                return new Command(() => {
                    CurrentPaginationIndex = StartPaginationIndex;
                    LoadMoreDbMathTaskInfo();
                });
            }
        }

        public Command LastPaginationButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    CurrentPaginationIndex = LastPaginationIndex;
                    LoadMoreDbMathTaskInfo();
                });
            }
        }

        public Command LeftPaginationButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (CurrentPaginationIndex != StartPaginationIndex)
                    {
                        CurrentPaginationIndex -= 1;
                        LoadMoreDbMathTaskInfo();
                    }
                });
            }
        }

        public Command RightPaginationButtonCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (CurrentPaginationIndex != LastPaginationIndex)
                    {
                        CurrentPaginationIndex += 1;
                        LoadMoreDbMathTaskInfo();
                    }
                });
            }
        }

        //-----------------------------------------------------------------------

        public Command LoadMoreDbMathTasksCommand { get; set; }

        private void LoadMoreDbMathTaskInfo()
        {
            DbMathTask[] dbMathTasks;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTasks = db.mathTasks.OrderByDescending(t => t.Id).Skip(AmountOfDataInListView * CurrentPaginationIndex).Take(AmountOfDataInListView).ToArray();
            }

             _mathTaskListItems = new List<DbMathTaskListItem>();

            for (int i = 0; i < dbMathTasks.Length; i++)
            {
                _mathTaskListItems.Add(new DbMathTaskListItem(dbMathTasks[i]));
            }

            ListViewHeightRequest = mathTaskListItems.Count * 75;
            OnPropertyChanged("mathTaskListItems");
        }

        public int ListViewHeightRequest
        {
            get
            {
                return _ListViewHeightRequest;
            }
            set
            {
                _ListViewHeightRequest = value;
                OnPropertyChanged("ListViewHeightRequest");
            }
        }

        public Command LoadSimilarCommand { get; set; }

        private async void LoadSimilar()
        {
            if (_SelectedMathTaskListItem != null)
                await navigation.PushAsync(new SimilarTasksStatisticsPage(_SelectedMathTaskListItem.dbMathTask, false));
        }
    }
}
