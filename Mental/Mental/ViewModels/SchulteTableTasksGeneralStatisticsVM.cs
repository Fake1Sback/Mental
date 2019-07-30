using Mental.Models;
using Mental.Models.DbModels;
using Microcharts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ChartEntry = Microcharts.ChartEntry;
using Mental.Views;

namespace Mental.ViewModels
{
    public class SchulteTableTasksGeneralStatisticsVM : BaseVM
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

        private List<DbSchulteTableTaskListItem> _SchulteTableTaskListItems;
        private DbSchulteTableTaskListItem _SelectedSchulteTableTaskListItem;

        private INavigation navigation;

        private List<ChartEntry> _TimeOptionsChart;
        private List<ChartEntry> _EasyModeChart;
        private List<ChartEntry> _GridSizeChart;

        private int _ListViewHeightRequest;

        public SchulteTableTasksGeneralStatisticsVM(INavigation _navigation)
        {
            navigation = _navigation;
            Initialize();

            LoadMoreDbMathTasksCommand = new Command(LoadMoreDbMathTaskInfo);
            LoadSimilarCommand = new Command(LoadSimilar);

            MessagingCenter.Subscribe<BaseVM>(this, "ReloadRecords", (p) =>
            {
                Initialize();
            });
        }

        private void Initialize()
        {
            using (var db = new ApplicationContext("mental.db"))
            {
                GeneralAmountOfRecords = db.SchulteTableTasks.Count();
                InitializeTimeOptionsChart(db);
                InitializeEasyModeOptionsChart(db);
                InitializeGridSizeOptionsChart(db);
            }

            int AmountOfPages = GeneralAmountOfRecords / AmountOfDataInListView;
            if (GeneralAmountOfRecords % AmountOfDataInListView == 0)
                AmountOfPages -= 1;

            StartPaginationIndex = 0;
            CurrentPaginationIndex = 0;
            LastPaginationIndex = AmountOfPages;

            LoadMoreDbMathTaskInfo();
        }

        public List<DbSchulteTableTaskListItem> DbSchulteTableTasksListItems
        {
            get
            {
                return new List<DbSchulteTableTaskListItem>(_SchulteTableTaskListItems);
            }
        }

        public DbSchulteTableTaskListItem SelectedSchulteTableTaskListItem
        {
            set
            {
                if (value != null)
                {
                    foreach(DbSchulteTableTaskListItem item in DbSchulteTableTasksListItems)
                        item.SetDefaultColor();

                    _SelectedSchulteTableTaskListItem = value;
                    _SelectedSchulteTableTaskListItem.SetActiveColor();
                }
                   
            }
        }

        public DonutChart TimeOptionsChart
        {
            get
            {
                return new DonutChart() { Entries = _TimeOptionsChart, HoleRadius = 0, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = StatisticsLabelTextSize };
            }
            set
            {
                _TimeOptionsChart = value.Entries.ToList();
                OnPropertyChanged("TimeOptionsChart");
            }
        }

        public DonutChart EasyModeChart
        {
            get
            {
                return new DonutChart() { Entries = _EasyModeChart, HoleRadius = 0, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelTextSize = StatisticsLabelTextSize };
            }
            set
            {
                _EasyModeChart = value.Entries.ToList();
                OnPropertyChanged("EasyModeChart");
            }
        }

        public PointChart GridSizeChart
        {
            get
            {
                return new PointChart() { Entries = _GridSizeChart, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), PointAreaAlpha = 200, LabelTextSize = StatisticsLabelTextSize, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, PointSize = 40, LabelColor = SkiaSharp.SKColor.Parse("#fafafa") };
            }
            set
            {
                _GridSizeChart = value.Entries.ToList();
                OnPropertyChanged("GridSizeChart");
            }
        }

        private async void InitializeTimeOptionsChart(ApplicationContext db)
        {
            int AmountOfCountdownOptionRecords = await db.SchulteTableTasks.Where(t => t.TimeOption == 0).CountAsync();
            int AmountOfLimitedTasksOptionRecords = await db.SchulteTableTasks.Where(t => t.TimeOption == 1).CountAsync();
            int AmountOfLastTaskOptionRecords = GeneralAmountOfRecords - (AmountOfCountdownOptionRecords + AmountOfLimitedTasksOptionRecords);

            List<ChartEntry> entries = new List<ChartEntry>()
            {
                new ChartEntry(AmountOfCountdownOptionRecords)
                {
                    ValueLabel = $"Countdown ({AmountOfCountdownOptionRecords.ToString()})",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new ChartEntry(AmountOfLimitedTasksOptionRecords)
                {
                    ValueLabel = $"Limited tasks ({AmountOfLimitedTasksOptionRecords.ToString()})",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                },
                new ChartEntry(AmountOfLastTaskOptionRecords)
                {
                    ValueLabel = $"Last task ({AmountOfLastTaskOptionRecords.ToString()})",
                    Color = SkiaSharp.SKColor.Parse(Color3)
                }
            };

            _TimeOptionsChart = entries;
            OnPropertyChanged("TimeOptionsChart");
        }

        private async void InitializeEasyModeOptionsChart(ApplicationContext db)
        {
            int AmountOfEasyModeActivatedRecords = await db.SchulteTableTasks.Where(t => t.IsEasyModeActivated == true).CountAsync();
            int AmountOfEasyModeDisabledRecords = GeneralAmountOfRecords - AmountOfEasyModeActivatedRecords;

            List<ChartEntry> entries = new List<ChartEntry>()
            {
                new ChartEntry(AmountOfEasyModeActivatedRecords)
                {
                    ValueLabel = $"Easy mode ({AmountOfEasyModeActivatedRecords.ToString()})",
                    Color = SkiaSharp.SKColor.Parse(Color1)
                },
                new ChartEntry(AmountOfEasyModeDisabledRecords)
                {
                    ValueLabel = $"Standard ({AmountOfEasyModeDisabledRecords.ToString()})",
                    Color = SkiaSharp.SKColor.Parse(Color2)
                }
            };
            _EasyModeChart = entries;
            OnPropertyChanged("EasyModeChart");
        }

        private async void InitializeGridSizeOptionsChart(ApplicationContext db)
        {
            int AmountOf3GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 3).CountAsync();
            int AmountOf4GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 4).CountAsync();
            int AmountOf5GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 5).CountAsync();
            int AmountOf6GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 6).CountAsync();
            int AmountOf7GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 7).CountAsync();

            List<ChartEntry> entries = new List<ChartEntry>()
            {
                new ChartEntry(AmountOf3GridSizeRecods)
                {
                    ValueLabel =  AmountOf3GridSizeRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color1),
                    Label = "3 x 3",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry(AmountOf4GridSizeRecods)
                {
                    ValueLabel = AmountOf4GridSizeRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color2),
                    Label = "4 x 4",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry( AmountOf5GridSizeRecods)
                {
                    ValueLabel = AmountOf5GridSizeRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color3),
                    Label =  "5 x 5",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry(AmountOf6GridSizeRecods)
                {
                    ValueLabel = AmountOf6GridSizeRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color4),
                    Label =  "6 x 6",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                },
                new ChartEntry( AmountOf7GridSizeRecods)
                {
                    ValueLabel = AmountOf7GridSizeRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse(Color5),
                    Label =  "7 x 7",
                    TextColor = SkiaSharp.SKColor.Parse("#fafafa")
                }
            };

            _GridSizeChart = entries;
            OnPropertyChanged("GridSizeChart");
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
            DbSchulteTableTask[] dbSchulteTableTasks;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbSchulteTableTasks = db.SchulteTableTasks.OrderByDescending(t => t.Id).Skip(AmountOfDataInListView * CurrentPaginationIndex).Take(AmountOfDataInListView).ToArray();
            }

            _SchulteTableTaskListItems = new List<DbSchulteTableTaskListItem>();

            for (int i = 0; i < dbSchulteTableTasks.Length; i++)
            {
                _SchulteTableTaskListItems.Add(new DbSchulteTableTaskListItem(dbSchulteTableTasks[i]));
            }

            ListViewHeightRequest = _SchulteTableTaskListItems.Count * 65;
            OnPropertyChanged("DbSchulteTableTasksListItems");
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
            if (_SelectedSchulteTableTaskListItem != null)
                await navigation.PushAsync(new SimilarSchulteTableTasksStatisticsPage(_SelectedSchulteTableTaskListItem.DbSchulteTableTask, false));
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
