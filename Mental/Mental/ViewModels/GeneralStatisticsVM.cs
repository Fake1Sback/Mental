﻿using System;
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
    public class GeneralStatisticsVM : INotifyPropertyChanged
    {
        private int GeneralAmountOfRecords;
        private int LoadCounter = 0;
        private int AmountOfDataInListView = 5;

        private List<DbMathTaskListItem> _mathTaskListItems;
        private DbMathTaskListItem _SelectedMathTaskListItem;

        private INavigation navigation;

        private List<Entry> _TimeOptionsChart;
        private List<Entry> _TaskTypeChart;
        private List<Entry> _DataTypeChart;
        private List<Entry> _RestrictionsChart;
        private List<Entry> _OperationsChart;
        private List<Entry> _ChainLengthChart;

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
            }
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
                    _SelectedMathTaskListItem = value;
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

        public DonutChart TaskTypeChart
        {
            get
            {
                return new DonutChart() { Entries = _TaskTypeChart };
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
                return new DonutChart() { Entries = _DataTypeChart };
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
                return new DonutChart() { Entries = _RestrictionsChart };
            }
            set
            {
                _RestrictionsChart = value.Entries.ToList();
                OnPropertyChanged("RestrictionsChart");
            }
        }

        public RadialGaugeChart OperationsChart
        {
            get
            {
                return new RadialGaugeChart() { Entries = _OperationsChart };
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
                return new DonutChart() { Entries = _ChainLengthChart };
            }
            set
            {
                _ChainLengthChart = value.Entries.ToList();
                OnPropertyChanged("ChainLengthChart");
            }
        }


        private async void InitializeTimeOptionsChart(ApplicationContext db)
        {           
            int AmountOfCountdownOptionRecords = await db.mathTasks.Where(t => t.TimeOptions == 0).CountAsync();
            int AmountOfLimitedTasksOptionsRecords = GeneralAmountOfRecords - AmountOfCountdownOptionRecords;

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfCountdownOptionRecords)
                {
                    ValueLabel = "Countdown",
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfLimitedTasksOptionsRecords)
                {
                    ValueLabel = "Limited tasks",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
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
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfFindXOptionsRecords)
                {
                    ValueLabel = "Find X",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
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
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfFractionalOptionsRecords)
                {
                    ValueLabel = "Fractional",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
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
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfNoSpecialModeOptionsRecords)
                {
                    ValueLabel = "No Restrictions",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
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
                    ValueLabel = "+",
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfMinusOperations)
                {
                    ValueLabel = "-",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
                },
                new Entry(AmountOfMultiplyOperations)
                {
                    ValueLabel = "*",
                    Color = SkiaSharp.SKColor.Parse("D3C0D3")
                },
                 new Entry(AmountOfDivideOperations)
                {
                    ValueLabel = "/",
                    Color = SkiaSharp.SKColor.Parse("007F7F")
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
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfNotFixedChainLengthOptionsRecords)
                {
                    ValueLabel = "Not Fixed",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
                }
            };

            _ChainLengthChart = entries;
            OnPropertyChanged("ChainLengthChart");
        }

        public Command LoadMoreDbMathTasksCommand { get; set; }

        private void LoadMoreDbMathTaskInfo()
        {
            DbMathTask[] dbMathTasks;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTasks = db.mathTasks.OrderByDescending(t => t.Id).Skip(AmountOfDataInListView * LoadCounter).Take(AmountOfDataInListView).ToArray();
            }
            if (_mathTaskListItems == null)
                _mathTaskListItems = new List<DbMathTaskListItem>();
            LoadCounter += 1;
            for (int i = 0; i < dbMathTasks.Length; i++)
            {
                _mathTaskListItems.Add(new DbMathTaskListItem(dbMathTasks[i]));
            }
            ListViewHeightRequest = mathTaskListItems.Count * 50;
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}