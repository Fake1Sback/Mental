using Mental.Models;
using Mental.Models.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using Microcharts.Forms;
using Microcharts;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Mental.Views;

namespace Mental.ViewModels
{
    public class SimilarMathTasksStatisticsVM : INotifyPropertyChanged
    {
        private int AmountOfData = 3;
        private int LoadMoreCounter = 0;

        private DbMathTask SelectedListItemDbMathTask;
        private DbMathTask dbMathTaskToSave;

        private List<DbMathTask> ListOfMathTasks = new List<DbMathTask>();
        public List<DbMathTaskListItem> DbMathTaskListItems { get; set; }

        public List<DbMathTaskListItem> DbMathTaskListItemsProp
        {
            get
            {
                return DbMathTaskListItems;
            }
            set
            {
                DbMathTaskListItems = value;
                OnPropertyChanged("DbMathTaskListItemsProp");
            }
        }

        private INavigation navigation;

        private bool DetailedTasksOptionsLayoutVisibility = false;
        private bool _DetailedStatisticsButtonVisibility = true;
        private bool _SaveButtonVisibility = true;
        private bool _GeneralStatisticsButtonVisibility = true;

        public SimilarMathTasksStatisticsVM(INavigation _navigation,DbMathTask _dbMathTask,bool Save)
        {
            navigation = _navigation;
            SaveButtonVisibility = Save;
            GeneralStatisticsButtonVisibility = Save;
            if (Save)
                dbMathTaskToSave = _dbMathTask;
            SelectedListItemDbMathTask = _dbMathTask;
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();

            ShowHideDetailedStatisticsCommand = new Command(ShowHideDetailedStatistics);
            SaveRecordToDbCommand = new Command(SaveRecordToDb);
            LoadMoreRecordsCommand = new Command(LoadMoreRecords);
            ClearRecordsCommand = new Command(ClearRecords);
            LoadGeneralStatistics = new Command(LoadMoreRecords);
            LoadGeneralStatistics = new Command(async () => { await navigation.PushAsync(new GeneralStatisticsPage()); });
        }

        public bool DetailedTaskOptionsVisibility
        {
            get
            {
                return DetailedTasksOptionsLayoutVisibility;
            }
            set
            {
                DetailedTasksOptionsLayoutVisibility = value;
                OnPropertyChanged("DetailedTaskOptionsVisibility");
            }
        }

        public bool PlusLabelVisibility
        {
            get
            {
                return SelectedListItemDbMathTask.Operations.Contains("+");
            }
            private set { }
        }

        public bool MinusLabelVisibility
        {
            get
            {
                return SelectedListItemDbMathTask.Operations.Contains("-");
            }
            private set { }
        }

        public bool MultiplyLabelVisibility
        {
            get
            {
                return SelectedListItemDbMathTask.Operations.Contains("*");
            }
            private set { }
        }

        public bool DivideLabelVisibility
        {
            get
            {
                return SelectedListItemDbMathTask.Operations.Contains("/");
            }
            private set { }
        }

        public string TaskTypeLabel
        {
            get
            {
                if (SelectedListItemDbMathTask.TaskType == (byte)TaskType.CountResult)
                    return "Task Type: Find Result";
                else
                    return "Task Type: Find X";
            }
            private set { }
        }

        public string TimeOptionLabel
        {
            get
            {
                if (SelectedListItemDbMathTask.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return "Time Option: Countdown";
                else //(SelectedListItemDbMathTask.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "Time Option: Limited Operations";
            }
            private set { }
        }

        public string TimeParameters
        {
            get
            {
                if (SelectedListItemDbMathTask.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return "Amount of minutes: " + SelectedListItemDbMathTask.TaskComplexityParameter.ToString() + " min";
                else if (SelectedListItemDbMathTask.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "Amount of tasks: " + SelectedListItemDbMathTask.TaskComplexityParameter.ToString() + " tasks";
                else
                    return "Amount of seconds: " + SelectedListItemDbMathTask.TaskComplexityParameter.ToString() + " sec";
            }
            private set { }
        }

        public string MinValue
        {
            get
            {
                return "MinValue: " + SelectedListItemDbMathTask.MinValue.ToString();
            }
            private set { }
        }

        public string MaxValue
        {
            get
            {
                return "MaxValue: " + SelectedListItemDbMathTask.MaxValue.ToString();
            }
            private set { }
        }

        public bool IsChainLengthFixed
        {
            get
            {
                if (SelectedListItemDbMathTask.IsChainLengthFixed)
                    return true;
                else
                    return false;
            }
            private set { }
        }

        public string MaxChainLength
        {
            get
            {
                return "Max Chain Length: " + SelectedListItemDbMathTask.MaxChainLength.ToString();
            }
            private set { }
        }

        public string DataType
        {
            get
            {
                if (SelectedListItemDbMathTask.IsInteger)
                    return "Data Type: INT";
                else
                    return "Data Type: FRACTIONAL";
            }
        }

        public bool DigitsPrecisionLabelVisibility
        {
            get
            {
                if (SelectedListItemDbMathTask.IsInteger)
                    return false;
                else
                    return true;
            }
            private set { }
        }

        public string DigitsAfterDotSign
        {
            get
            {
                return "Digits after dot sign: " + SelectedListItemDbMathTask.DigitsAfterDotSing.ToString();
            }
            private set { }
        }

        public bool RestrictionsLayoutVisibility
        {
            get
            {
                return SelectedListItemDbMathTask.IsRestrictionActivated;
            }
            private set { }
        }

        public string RestrictionsString
        {
            get
            {
                return SelectedListItemDbMathTask.RestrictionsString;
            }
            private set { }
        }

        public string LongestTimeSpentForExpression
        {
            get
            {
                return "Longest time spent for expression: " + SelectedListItemDbMathTask.LongestTimeSpentForExpression;
            }
            private set { }
        }

        public string LongestTimeExpressionString
        {
            get
            {
                return "Expression: " + SelectedListItemDbMathTask.LongestTimeExpressionString;
            }
            private set { }
        }

        public string ShortestTimeSpentForExpression
        {
            get
            {
                return "Shortest time spent for expression: " + SelectedListItemDbMathTask.ShortestTimeSpentForExpression;
            }
            private set { }
        }

        public string ShortestTimeExpressionString
        {
            get
            {
                return "Expression: " + SelectedListItemDbMathTask.ShortestTimeExpressionString;
            }
            private set { }
        }
    
        public DbMathTaskListItem SelectedListViewItem
        {
            set
            {
                DbMathTaskListItem dbMathTaskListItem = value;
                if (dbMathTaskListItem != null)
                {
                    SelectedListItemDbMathTask = dbMathTaskListItem.dbMathTask;
                    OnPropertyChanged("DetailedTaskOptionsVisibility");
                    OnPropertyChanged("PlusLabelVisibility");
                    OnPropertyChanged("MinusLabelVisibility");
                    OnPropertyChanged("MultiplyLabelVisibility");
                    OnPropertyChanged("DivideLabelVisibility");
                    OnPropertyChanged("TaskTypeLabel");
                    OnPropertyChanged("TimeOptionLabel");
                    OnPropertyChanged("AmountOfMinsOrTasks");
                    OnPropertyChanged("MinValue");
                    OnPropertyChanged("MaxValue");
                    OnPropertyChanged("IsChainLengthFixed");
                    OnPropertyChanged("MaxChainLength");
                    OnPropertyChanged("DataType");
                    OnPropertyChanged("DigitsPrecisionLabelVisibility");
                    OnPropertyChanged("DigitsAfterDotSign");
                    OnPropertyChanged("SpecialModeLabelVisibility");
                    OnPropertyChanged("SpecialModeRestrictions");
                    OnPropertyChanged("LongestTimeSpentForExpression");
                    OnPropertyChanged("LongestTimeExpressionString");
                    OnPropertyChanged("ShortestTimeSpentForExpression");
                    OnPropertyChanged("ShortestTimeExpressionString");
                    InitializeChart();
                }
            }
        }

        public bool DetailedStatisticsButtonVisibility
        {
            get
            {
                return _DetailedStatisticsButtonVisibility;
            }
            set
            {
                _DetailedStatisticsButtonVisibility = value;
                OnPropertyChanged("DetailedStatisticsButtonVisibility");
            }
        }

        public bool SaveButtonVisibility
        {
            get
            {
                return _SaveButtonVisibility;
            }
            set
            {
                _SaveButtonVisibility = value;
                OnPropertyChanged("SaveButtonVisibility");
            }
        }

        public bool GeneralStatisticsButtonVisibility
        {
            get
            {
                return _GeneralStatisticsButtonVisibility;
            }
            set
            {
                _GeneralStatisticsButtonVisibility = value;
                OnPropertyChanged("GeneralStatisticsButtonVisibility");
            }
        }

        //--------------------------------------------

        public Command ShowHideDetailedStatisticsCommand { get; set; }

        private void ShowHideDetailedStatistics()
        {
            if (DetailedTaskOptionsVisibility)
                DetailedTaskOptionsVisibility = false;
            else
                DetailedTaskOptionsVisibility = true;
        }

        public Command SaveRecordToDbCommand { get; set; }

        private void SaveRecordToDb()
        {
            if (dbMathTaskToSave != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    db.mathTasks.Add(dbMathTaskToSave);
                    db.SaveChanges();
                }
                dbMathTaskToSave = null;
                LoadMoreCounter = 0;
                ListOfMathTasks.Clear();
                GetMathTasksFromDb();
                SelectedListItemDbMathTask = ListOfMathTasks[0];
                FillListView();
                InitializeChart();
                SaveButtonVisibility = false;
            }
        }

        public Command LoadMoreRecordsCommand { get; set; }

        private void LoadMoreRecords()
        {
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();
        }

        public Command ClearRecordsCommand { get; set; }

        private void ClearRecords()
        {
            if (SelectedListItemDbMathTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    DbMathTask[] mathTasksToDelete = db.mathTasks.Where(t => t.TimeOptions == SelectedListItemDbMathTask.TimeOptions && t.TaskType == SelectedListItemDbMathTask.TaskType && t.TaskComplexityParameter == SelectedListItemDbMathTask.TaskComplexityParameter && t.MinValue == SelectedListItemDbMathTask.MinValue && t.MaxValue == SelectedListItemDbMathTask.MaxValue).ToArray();
                    db.mathTasks.RemoveRange(mathTasksToDelete);
                    db.SaveChanges();
                }
            }

            dbMathTaskToSave = null;
            SelectedListItemDbMathTask = null;

            LoadMoreCounter = 0;

            ListOfMathTasks.Clear();

            DetailedStatisticsButtonVisibility = false;
            DetailedTaskOptionsVisibility = false;
            SaveButtonVisibility = false;

            FillListView();
            InitializeChart();
        }

        public Command LoadGeneralStatistics { get; set; }

        //--------------------------------------------
        private void GetMathTasksFromDb()
        {
            DbMathTask[] dbMathTasksArray;
            if (SelectedListItemDbMathTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    dbMathTasksArray = db.mathTasks.Where(t => t.TimeOptions == SelectedListItemDbMathTask.TimeOptions && t.TaskType == SelectedListItemDbMathTask.TaskType && t.TaskComplexityParameter == SelectedListItemDbMathTask.TaskComplexityParameter && t.MinValue == SelectedListItemDbMathTask.MinValue && t.MaxValue == SelectedListItemDbMathTask.MaxValue).OrderByDescending(t => t.Id).Skip(AmountOfData * LoadMoreCounter).Take(AmountOfData).ToArray();
                }
      
                LoadMoreCounter += 1;
                ListOfMathTasks.AddRange(dbMathTasksArray);
            }
        }

        private void FillListView()
        {
            DbMathTaskListItems = new List<DbMathTaskListItem>();

            for (int i = 0; i < ListOfMathTasks.Count; i++)
            {
                DbMathTaskListItems.Add(new DbMathTaskListItem(ListOfMathTasks[i]));
            }

            OnPropertyChanged("SimilarTasksListViewHeightRequest");
            OnPropertyChanged("DbMathTaskListItemsProp");
        }

        public int SimilarTasksListViewHeightRequest
        {
            get
            {
                return ListOfMathTasks.Count * 50;
            }
            private set { }
        }

        private void InitializeChart()
        {
            OnPropertyChanged("LineChart");
        }

        public LineChart LineChart
        {
            get
            {
                List<Entry> entries = new List<Entry>();
                for (int i = ListOfMathTasks.Count - 1; i >= 0; i--)
                {
                    if (SelectedListItemDbMathTask == ListOfMathTasks[i])
                        entries.Add(new Entry(ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("000080"), Label = "Selected", ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString() });
                    else
                    {
                        if(ListOfMathTasks[i].TaskDateTime.Date == DateTime.Now.Date)
                        {
                            entries.Add(new Entry(ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("1CC9F0"), Label = ListOfMathTasks[i].TaskDateTime.ToString(@"HH:mm"), ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString() });
                        }
                        else
                            entries.Add(new Entry(ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("1CC9F0"), Label = ListOfMathTasks[i].TaskDateTime.ToString(@"dd:MM:yy"), ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString() });
                    }
                       
                }
                if (dbMathTaskToSave != null)
                    entries.Add(new Entry(dbMathTaskToSave.GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FF1493"), Label = "Current", ValueLabel = dbMathTaskToSave.GetEfficiencyParameterString() });
                return new LineChart() { Entries = entries };
            }
            private set { }
        }

        //------------------------------------------------

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
