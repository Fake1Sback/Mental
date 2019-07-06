using Mental.Models;
using Mental.Models.DbModels;
using Microcharts;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using Mental.Views;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mental.ViewModels
{
    public class StroopTaskGeneralStatisticsVM : BaseVM
    {
        private int GeneralAmountOfRecords;
        private int LoadCounter = 0;
        private int AmountOfDataInListView = 5;

        private List<DbStroopTaskListItem> _DbStroopTaskListItems;
        private DbStroopTaskListItem _SelectedStroopTaskListItem;

        private INavigation navigation;

        private List<Entry> _TimeOptionsChart;
        private List<Entry> _StroopTaskTypeChart;
        private List<Entry> _AmountOfButtonsChart;

        private int _ListViewHeightRequest;

        public StroopTaskGeneralStatisticsVM(INavigation _navigation)
        {
            navigation = _navigation;
            using (var db = new ApplicationContext("mental.db"))
            {
                GeneralAmountOfRecords = db.StroopTasks.Count();
                InitializeTimeOptionsChart(db);
                InitializeStroopTaskTypeChart(db);
                InitializeAmountOfButtonsChart(db);
            }
            LoadMoreDbMathTaskInfo();
            LoadMoreDbMathTasksCommand = new Command(LoadMoreDbMathTaskInfo);
            LoadSimilarCommand = new Command(LoadSimilar);
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
                    _SelectedStroopTaskListItem = value;
            }
        }

        public DonutChart TimeOptionsChart
        {
            get
            {
                return new DonutChart() { Entries = _TimeOptionsChart };
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
                return new DonutChart() { Entries = _StroopTaskTypeChart };
            }
            set
            {
                _StroopTaskTypeChart = value.Entries.ToList();
                OnPropertyChanged("StroopTaskTypeChart");
            }
        }

        public BarChart AmountOfButtonsChart
        {
            get
            {
                return new BarChart() { Entries = _AmountOfButtonsChart };
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

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfCountdownOptionRecords)
                {
                    ValueLabel = "Countdown",
                    Color = SkiaSharp.SKColor.Parse("000080"),
                    Label = AmountOfCountdownOptionRecords.ToString()
                },
                new Entry(AmountOfLimitedTasksOptionRecords)
                {
                    ValueLabel = "Limited tasks",
                    Color = SkiaSharp.SKColor.Parse("F55B70"),
                    Label = AmountOfLimitedTasksOptionRecords.ToString()
                },
                new Entry(AmountOfLastTaskOptionRecords)
                {
                    ValueLabel = "Last task",
                    Color = SkiaSharp.SKColor.Parse("D3C0D3"),
                    Label = AmountOfLastTaskOptionRecords.ToString()
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

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfFindOneCorrectRecords)
                {
                    ValueLabel = "Find One Correct",
                    Color = SkiaSharp.SKColor.Parse("000080"),
                    Label = AmountOfFindOneCorrectRecords.ToString()
                },
                new Entry(AmountOfTrueOrFalseRecords)
                {
                    ValueLabel = "True or False",
                    Color = SkiaSharp.SKColor.Parse("F55B70"),
                    Label = AmountOfTrueOrFalseRecords.ToString()
                },
                new Entry(AmountOfFindColorByTextRecods)
                {
                    ValueLabel = "Find Color by Text",
                    Color = SkiaSharp.SKColor.Parse("D3C0D3"),
                    Label = AmountOfFindColorByTextRecods.ToString()
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

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOf2ButtonsRecods)
                {
                    ValueLabel = AmountOf2ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse("000080"),
                    Label = "2 Records"
                },
                new Entry(AmountOf4ButtonsRecods)
                {
                    ValueLabel = AmountOf4ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse("F55B70"),
                    Label = "4 Records"
                },
                new Entry(AmountOf6ButtonsRecods)
                {
                    ValueLabel = AmountOf6ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse("D3C0D3"),
                    Label = "6 Records"
                },
                new Entry(AmountOf8ButtonsRecods)
                {
                    ValueLabel = AmountOf8ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse("007F7F"),
                    Label = "8 Records"
                },
                new Entry(AmountOf10ButtonsRecods)
                {
                    ValueLabel = AmountOf10ButtonsRecods.ToString(),
                    Color = SkiaSharp.SKColor.Parse("0545F5"),
                    Label = "10 Records"
                }
            };

            _AmountOfButtonsChart = entries;
            OnPropertyChanged("AmountOfButtonsChart");
        }


   
        public Command LoadMoreDbMathTasksCommand { get; set; }

        private void LoadMoreDbMathTaskInfo()
        {
            DbStroopTask[] dbStroopTasks;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbStroopTasks = db.StroopTasks.OrderByDescending(t => t.Id).Skip(AmountOfDataInListView * LoadCounter).Take(AmountOfDataInListView).ToArray();           
            }
            if (_DbStroopTaskListItems == null)
                _DbStroopTaskListItems = new List<DbStroopTaskListItem>();
            LoadCounter += 1;
            for (int i = 0; i < dbStroopTasks.Length; i++)
            {
                _DbStroopTaskListItems.Add(new DbStroopTaskListItem(dbStroopTasks[i]));
            }
            ListViewHeightRequest = _DbStroopTaskListItems.Count * 50;
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
    }
}
