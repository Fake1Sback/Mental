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
using Entry = Microcharts.Entry;
using Mental.Views;

namespace Mental.ViewModels
{
    public class SchulteTableTasksGeneralStatisticsVM : INotifyPropertyChanged
    {
        private int GeneralAmountOfRecords;
        private int LoadCounter = 0;
        private int AmountOfDataInListView = 5;

        private List<DbSchulteTableTaskListItem> _SchulteTableTaskListItems;
        private DbSchulteTableTaskListItem _SelectedSchulteTableTaskListItem;

        private INavigation navigation;

        private List<Entry> _TimeOptionsChart;
        private List<Entry> _EasyModeChart;
        private List<Entry> _GridSizeChart;

        private int _ListViewHeightRequest;

        public SchulteTableTasksGeneralStatisticsVM(INavigation _navigation)
        {
            navigation = _navigation;
            using (var db = new ApplicationContext("mental.db"))
            {
                GeneralAmountOfRecords = db.SchulteTableTasks.Count();
                InitializeTimeOptionsChart(db);
                InitializeEasyModeOptionsChart(db);
                InitializeGridSizeOptionsChart(db);
            }
            LoadMoreDbMathTaskInfo();
            LoadMoreDbMathTasksCommand = new Command(LoadMoreDbMathTaskInfo);
            LoadSimilarCommand = new Command(LoadSimilar);
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
                    _SelectedSchulteTableTaskListItem = value;
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

        public DonutChart EasyModeChart
        {
            get
            {
                return new DonutChart() { Entries = _EasyModeChart };
            }
            set
            {
                _EasyModeChart = value.Entries.ToList();
                OnPropertyChanged("EasyModeChart");
            }
        }

        public BarChart GridSizeChart
        {
            get
            {
                return new BarChart() { Entries = _GridSizeChart };
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

        private async void InitializeEasyModeOptionsChart(ApplicationContext db)
        {
            int AmountOfEasyModeActivatedRecords = await db.SchulteTableTasks.Where(t => t.IsEasyModeActivated == true).CountAsync();
            int AmountOfEasyModeDisabledRecords = GeneralAmountOfRecords - AmountOfEasyModeActivatedRecords;

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfEasyModeActivatedRecords)
                {
                    ValueLabel = "Easy mode",
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfEasyModeDisabledRecords)
                {
                    ValueLabel = "Standard",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
                }
            };
            _EasyModeChart = entries;
            OnPropertyChanged(" EasyModeChart");
        }

        private async void InitializeGridSizeOptionsChart(ApplicationContext db)
        {
            int AmountOf3GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 3).CountAsync();
            int AmountOf4GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 4).CountAsync();
            int AmountOf5GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 5).CountAsync();
            int AmountOf6GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 6).CountAsync();
            int AmountOf7GridSizeRecods = await db.SchulteTableTasks.Where(t => t.GridSize == 7).CountAsync();

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOf3GridSizeRecods)
                {
                    ValueLabel = "3",
                    Color = SkiaSharp.SKColor.Parse("000080"),
                    Label = AmountOf3GridSizeRecods.ToString()
                },
                new Entry(AmountOf4GridSizeRecods)
                {
                    ValueLabel = "4",
                    Color = SkiaSharp.SKColor.Parse("F55B70"),
                    Label = AmountOf4GridSizeRecods.ToString()
                },
                new Entry( AmountOf5GridSizeRecods)
                {
                    ValueLabel = "5",
                    Color = SkiaSharp.SKColor.Parse("D3C0D3"),
                    Label = AmountOf5GridSizeRecods.ToString()
                },
                new Entry(AmountOf6GridSizeRecods)
                {
                    ValueLabel = "6",
                    Color = SkiaSharp.SKColor.Parse("007F7F"),
                    Label = AmountOf6GridSizeRecods.ToString()
                },
                new Entry( AmountOf7GridSizeRecods)
                {
                    ValueLabel = "7",
                    Color = SkiaSharp.SKColor.Parse("0545F5"),
                    Label = AmountOf7GridSizeRecods.ToString()
                }
            };

            _GridSizeChart = entries;
            OnPropertyChanged("GridSizeChart");
        }
       
        public Command LoadMoreDbMathTasksCommand { get; set; }

        private void LoadMoreDbMathTaskInfo()
        {
            DbSchulteTableTask[] dbSchulteTableTasks;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbSchulteTableTasks = db.SchulteTableTasks.OrderByDescending(t => t.Id).Skip(AmountOfDataInListView * LoadCounter).Take(AmountOfDataInListView).ToArray();
            }
            if (_SchulteTableTaskListItems == null)
                _SchulteTableTaskListItems = new List<DbSchulteTableTaskListItem>();
            LoadCounter += 1;
            for (int i = 0; i < dbSchulteTableTasks.Length; i++)
            {
                _SchulteTableTaskListItems.Add(new DbSchulteTableTaskListItem(dbSchulteTableTasks[i]));
            }
            ListViewHeightRequest = _SchulteTableTaskListItems.Count * 50;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
