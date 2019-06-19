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
        private List<Entry> _SpecialModeChart;
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
                InitializeSpecailModeOptionsChart(db);
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

        public DonutChart SpecialModeChart
        {
            get
            {
                return new DonutChart() { Entries = _SpecialModeChart };
            }
            set
            {
                _SpecialModeChart = value.Entries.ToList();
                OnPropertyChanged("SpecialModeChart");
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


        private void InitializeTimeOptionsChart(ApplicationContext db)
        {
            int AmountOfCountdownOptionRecords = db.mathTasks.Where(t => t.TimeOptions == 0).Count();
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

        private void InitializeTaskTypeOptionsChart(ApplicationContext db)
        {
            int AmountOfFindResultOptionsRecords = db.mathTasks.Where(t => t.TaskType == 0).Count();
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

        private void InitializeDataTypeOptionsChart(ApplicationContext db)
        {
            int AmountOfIntOptionsRecords = db.mathTasks.Where(t => t.IsInteger == true).Count();
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

        private void InitializeSpecailModeOptionsChart(ApplicationContext db)
        {
            int AmountOfSpecialModeOptionsRecords = db.mathTasks.Where(t => t.IsSpecialModeActivated == true).Count();
            int AmountOfNoSpecialModeOptionsRecords = GeneralAmountOfRecords - AmountOfSpecialModeOptionsRecords;

            List<Entry> entries = new List<Entry>()
            {
                new Entry(AmountOfSpecialModeOptionsRecords)
                {
                    ValueLabel = "SpecialMode",
                    Color = SkiaSharp.SKColor.Parse("000080")
                },
                new Entry(AmountOfNoSpecialModeOptionsRecords)
                {
                    ValueLabel = "No Special Mode",
                    Color = SkiaSharp.SKColor.Parse("F55B70")
                }
            };

            _SpecialModeChart = entries;
            OnPropertyChanged("SpecialModeChart");
        }

        private void InitializeOperationsModeOptionsChart(ApplicationContext db)
        {
            int AmountOfPlusOperations = db.mathTasks.Where(t => t.Operations.Contains("+")).Count();
            int AmountOfMinusOperations = db.mathTasks.Where(t => t.Operations.Contains("-")).Count();
            int AmountOfMultiplyOperations = db.mathTasks.Where(t => t.Operations.Contains("*")).Count();
            int AmountOfDivideOperations = db.mathTasks.Where(t => t.Operations.Contains("/")).Count();

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

        private void InitializeFixedChainLengthOptionsChart(ApplicationContext db)
        {
            int AmountOfFixedChainLengthOptionRecords = db.mathTasks.Where(t => t.IsChainLengthFixed == true).Count();
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
