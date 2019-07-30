using Mental.Models;
using Mental.Models.DbModels;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using ChartEntry = Microcharts.ChartEntry;
using Mental.Views;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mental.ViewModels
{
    public class StroopTaskGeneralStatisticsVM : BaseVM
    {
        private int GeneralAmountOfRecords;
        private int AmountOfDataInListView = 5;

        private int _StartPaginationIndex;
        private int _CurrentPaginationIndex;
        private int _LastPaginationIndex;

        private string Color1 = "#0040ff"; //DarkBlue        
        private string Color2 = "#F55B70"; //Red
        private string Color3 = "#ff9900"; //Orange
        private string Color4 = "#009933"; //Green     
        private string Color5 = "#8000ff"; //Violet
        private string Color6 = "#2eb8b8"; //Aqua
        private string Color7 = "#cc4400"; //Brown

        public int StatisticsLabelTextSize = 40;

        private List<DbStroopTaskListItem> _DbStroopTaskListItems;
        private DbStroopTaskListItem _SelectedStroopTaskListItem;

        private INavigation navigation;

        private List<ChartEntry> _TimeOptionsChart;
        private List<ChartEntry> _StroopTaskTypeChart;
        private List<ChartEntry> _AmountOfButtonsChart;

        private int _ListViewHeightRequest;

        public StroopTaskGeneralStatisticsVM(INavigation _navigation)
        {
            navigation = _navigation;
            Initialize();

            LoadMoreDbMathTasksCommand = new Command(LoadMoreDbMathTaskInfo);
            LoadSimilarCommand = new Command(LoadSimilar);

            MessagingCenter.Subscribe<BaseVM>(this, "ReloadRecords",(p) =>
            {
                Initialize();
            });
        }  

        private void Initialize()
        {
            using (var db = new ApplicationContext("mental.db"))
            {
                GeneralAmountOfRecords = db.StroopTasks.Count();
                InitializeTimeOptionsChart(db);
                InitializeStroopTaskTypeChart(db);
                InitializeAmountOfButtonsChart(db);
            }

            int AmountOfPages = GeneralAmountOfRecords / AmountOfDataInListView;
            if (GeneralAmountOfRecords % AmountOfDataInListView == 0)
                AmountOfPages -= 1;

            StartPaginationIndex = 0;
            CurrentPaginationIndex = 0;
            LastPaginationIndex = AmountOfPages;

            LoadMoreDbMathTaskInfo();
        }

        public List<DbStroopTaskListItem> DbStroopTaskListItems
        {
            get
            {
                return new List<DbStroopTaskListItem>(_DbStroopTaskListItems);
            }
        }

        public DbStroopTaskListItem SelectedDbStroopTaskListItem
        {
            set
            {
                if (value != null)
                {
                    foreach (DbStroopTaskListItem item in DbStroopTaskListItems)
                        item.SetDefaultColor();

                    _SelectedStroopTaskListItem = value;
                    _SelectedStroopTaskListItem.SetActiveColor();
                }
            }
        }


        public DonutChart TimeOptionsChart
        {
            get
            {
                return new DonutChart() { Entries = _TimeOptionsChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = StatisticsLabelTextSize, HoleRadius = 0 };
            }
            set
            {
                _TimeOptionsChart = value.Entries.ToList();
                OnPropertyChanged("TimeOptionsChart");
            }
        }

        public DonutChart StroopTaskTypeChart
        {
            get
            {
                return new DonutChart() { Entries = _StroopTaskTypeChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = StatisticsLabelTextSize, HoleRadius = 0};
            }
            set
            {
                _StroopTaskTypeChart = value.Entries.ToList();
                OnPropertyChanged("StroopTaskTypeChart");
            }
        }

        public PointChart AmountOfButtonsChart
        {
            get
            {
                return new PointChart() { Entries = _AmountOfButtonsChart, PointAreaAlpha = 200, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = StatisticsLabelTextSize, ValueLabelOrientation = Orientation.Horizontal, LabelOrientation = Orientation.Horizontal, PointSize = 40, LabelColor = SkiaSharp.SKColor.Parse("#fafafa")};
            }
            set
            {
                _AmountOfButtonsChart = value.Entries.ToList();
                OnPropertyChanged("AmountOfButtonsChart");
            }
        }

      
        private async void InitializeTimeOptionsChart(ApplicationContext db)
        {
            int AmountOfCountdownOptionRecords = await db.StroopTasks.Where(t => t.TimeOption == 0).CountAsync();
            int AmountOfLimitedTasksOptionRecords = await db.StroopTasks.Where(t => t.TimeOption == 1).CountAsync();
            int AmountOfLastTaskOptionRecords = GeneralAmountOfRecords - (AmountOfCountdownOptionRecords + AmountOfLimitedTasksOptionRecords);

            List<ChartEntry> entries = new List<ChartEntry>()
            {
                new ChartEntry(AmountOfCountdownOptionRecords)
                {
                    ValueLabel = $"Countdown ({AmountOfCountdownOptionRecords})",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new ChartEntry(AmountOfLimitedTasksOptionRecords)
                {
                    ValueLabel = $"Limited tasks ({AmountOfLimitedTasksOptionRecords})",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                },
                new ChartEntry(AmountOfLastTaskOptionRecords)
                {
                    ValueLabel = $"Last task ({AmountOfLastTaskOptionRecords})",
                    Color = SkiaSharp.SKColor.Parse(Color3)
                }
            };

            _TimeOptionsChart = entries;
            OnPropertyChanged("TimeOptionsChart");
        }

        private async void InitializeStroopTaskTypeChart(ApplicationContext db)
        {
            int AmountOfFindOneCorrectRecords = await db.StroopTasks.Where(t => t.StroopTaskOption == (byte)StroopTaskType.FindOneCorrect).CountAsync();
            int AmountOfTrueOrFalseRecords = await db.StroopTasks.Where(t => t.StroopTaskOption == (byte)StroopTaskType.TrueOrFalse).CountAsync();
            int AmountOfFindColorByTextRecods = GeneralAmountOfRecords - (AmountOfFindOneCorrectRecords + AmountOfTrueOrFalseRecords);

            List<ChartEntry> entries = new List<ChartEntry>()
            {
                new ChartEntry(AmountOfFindOneCorrectRecords)
                {
                    ValueLabel = $"One Correct ({AmountOfFindOneCorrectRecords})",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new ChartEntry(AmountOfTrueOrFalseRecords)
                {
                    ValueLabel = $"True / False ({AmountOfTrueOrFalseRecords})",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                },
                new ChartEntry(AmountOfFindColorByTextRecods)
                {
                    ValueLabel = $"Color by Text ({AmountOfFindColorByTextRecods})",
                    Color = SkiaSharp.SKColor.Parse(Color3)
                }
            };

            _StroopTaskTypeChart = entries;
            OnPropertyChanged("StroopTaskTypeChart");
        }

        private async void InitializeAmountOfButtonsChart(ApplicationContext db)
        {
            int AmountOf2ButtonsRecods = await db.StroopTasks.Where(t => t.AmountOfButtons == 2).CountAsync();
            int AmountOf4ButtonsRecods = await db.StroopTasks.Where(t => t.AmountOfButtons == 4).CountAsync();
            int AmountOf6ButtonsRecods = await db.StroopTasks.Where(t => t.AmountOfButtons == 6).CountAsync();
            int AmountOf8ButtonsRecods = await db.StroopTasks.Where(t => t.AmountOfButtons == 8).CountAsync();
            int AmountOf10ButtonsRecods = await db.StroopTasks.Where(t => t.AmountOfButtons == 10).CountAsync();

            List<ChartEntry> entries = new List<ChartEntry>()
            {
                new ChartEntry(AmountOf2ButtonsRecods)
                {
                    ValueLabel = AmountOf2ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color1),
                    Label = "2 Records",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry(AmountOf4ButtonsRecods)
                {
                    ValueLabel = AmountOf4ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color2),
                    Label = "4 Records",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry(AmountOf6ButtonsRecods)
                {
                    ValueLabel = AmountOf6ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color3),
                    Label = "6 Records",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry(AmountOf8ButtonsRecods)
                {
                    ValueLabel = AmountOf8ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color4),
                    Label = "8 Records",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry(AmountOf10ButtonsRecods)
                {
                    ValueLabel = AmountOf10ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color5),
                    Label = "10 Records",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                }
            };

            _AmountOfButtonsChart = entries;
            OnPropertyChanged("AmountOfButtonsChart");
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

        private async void LoadMoreDbMathTaskInfo()
        {
            DbStroopTask[] dbStroopTasks;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbStroopTasks = await db.StroopTasks.OrderByDescending(t => t.Id).Skip(AmountOfDataInListView * CurrentPaginationIndex).Take(AmountOfDataInListView).ToArrayAsync();
            }

            _DbStroopTaskListItems = new List<DbStroopTaskListItem>();

            for (int i = 0; i < dbStroopTasks.Length; i++)
            {
                _DbStroopTaskListItems.Add(new DbStroopTaskListItem(dbStroopTasks[i]));
            }
            ListViewHeightRequest = _DbStroopTaskListItems.Count * 65;
            OnPropertyChanged("DbStroopTaskListItems");
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
            if (_SelectedStroopTaskListItem != null)
                await navigation.PushAsync(new StroopTaskSimilarStatisticsPage(_SelectedStroopTaskListItem.DbStroopTask,false));
        }

        public Command ToHomeCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await navigation.PopToRootAsync();
                });
            }
        }
    }
}
