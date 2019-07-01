using Mental.Models;
using Mental.Models.DbModels;
using Microcharts;
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
    public class SimilarSchulteTableTasksStatisticsVM : BaseVM
    {
        private int AmountOfData = 3;
        private int LoadMoreCounter = 0;

        private DbSchulteTableTask SelectedDbSchulteTableTask;
        private DbSchulteTableTask DbSchulteTaskToSave;

        private List<DbSchulteTableTask> DbSchulteTableTasksList = new List<DbSchulteTableTask>();
        public List<DbSchulteTableTaskListItem> DbSchulteTableTasksListItems { get; set; }

        public List<DbSchulteTableTaskListItem> dbSchulteTableTaskListItems
        {
            get
            {
                return DbSchulteTableTasksListItems;
            }
            set
            {
                DbSchulteTableTasksListItems = value;
                OnPropertyChanged("dbSchulteTableTaskListItems");
            }
        }

        public DbSchulteTableTaskListItem SelectedDbSchulteTableListViewItem
        {
            set
            {
                if(value != null)
                {
                    SelectedDbSchulteTableTask = value.DbSchulteTableTask;
                    OnPropertyChanged("TimeOptionLabel");
                    OnPropertyChanged("EasyModeLabel");
                    OnPropertyChanged("GridSizeLabel");
                    OnPropertyChanged("EfficiencyLabel");
                    OnPropertyChanged("LongestTimeSpentForFindingNumberLabel");
                    OnPropertyChanged("LongestTimeLabel");
                    OnPropertyChanged("ShortestTimeSpentForFindingNumberLabel");
                    OnPropertyChanged("ShortestTimeNumberLabel");
                    OnPropertyChanged("DateTimeLabel");
                    InitializeChart();
                }
            }
        }

        private INavigation navigation;

        private bool _DetailedTaskOptionsButtonVisibility = true;
        private bool _DetailedTasksOptionsLayoutVisibility = false;
        private bool _SaveButtonVisibility;
        private bool _GeneralStatisticsButtonVisibility = true;

        public SimilarSchulteTableTasksStatisticsVM(INavigation _navigation,DbSchulteTableTask _dbSchulteTableTask,bool Save)
        {
            navigation = _navigation;
            SaveButtonVisibility = Save;
            GeneralStatisticsButtonVisibility = Save;
            if (Save)
                DbSchulteTaskToSave = _dbSchulteTableTask;
            SelectedDbSchulteTableTask = _dbSchulteTableTask;
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();

            SaveRecordToDbCommand = new Command(SaveRecordToDb);
            LoadMoreRecordsCommand = new Command(LoadMoreRecords);
            ClearRecordsCommand = new Command(ClearRecords);
            LoadGeneralStatistics = new Command(LoadMoreRecords);
            LoadGeneralStatistics = new Command(async () => { await navigation.PushAsync(new SchulteTableTasksGeneralStatisticsPage()); });
        }

        public LineChart LineChart
        {
            get
            {
                List<Entry> entries = new List<Entry>();
                for (int i = DbSchulteTableTasksList.Count - 1; i >= 0; i--)
                {
                    if (SelectedDbSchulteTableTask == DbSchulteTableTasksList[i])
                        entries.Add(new Entry(DbSchulteTableTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("000080"), Label = "Selected", ValueLabel = DbSchulteTableTasksList[i].GetEfficiencyParameterString() });
                    else
                    {
                        if (DbSchulteTableTasksList[i].TaskDateTime.Date == DateTime.Now.Date)
                        {
                            entries.Add(new Entry(DbSchulteTableTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("1CC9F0"), Label = DbSchulteTableTasksList[i].TaskDateTime.ToString(@"HH:mm"), ValueLabel = DbSchulteTableTasksList[i].GetEfficiencyParameterString() });
                        }
                        else
                            entries.Add(new Entry(DbSchulteTableTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("1CC9F0"), Label = DbSchulteTableTasksList[i].TaskDateTime.ToString(@"dd:MM:yy"), ValueLabel = DbSchulteTableTasksList[i].GetEfficiencyParameterString() });
                    }

                }
                if (DbSchulteTaskToSave != null)
                    entries.Add(new Entry(DbSchulteTaskToSave.GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FF1493"), Label = "Current", ValueLabel = DbSchulteTaskToSave.GetEfficiencyParameterString() });
                return new LineChart() { Entries = entries };
            }
            private set { }
        }  

        public string TimeOptionLabel
        {
            get
            {
                if (SelectedDbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return "Time Option: Countdown";
                else if (SelectedDbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return "Time Option: Limited Operations";
                else
                    return "Time Options: Last Task";

            }
        }

        public string EasyModeLabel
        {
            get
            {
                if (SelectedDbSchulteTableTask.IsEasyModeActivated)
                    return "Easy Mode: +";
                else
                    return "Easy Mode: -";
            }
        }

        public string GridSizeLabel
        {
            get
            {
                return "Grid Size: " + SelectedDbSchulteTableTask.GridSize;
            }
        }

        public string EfficiencyLabel
        {
            get
            {
                if (SelectedDbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return "Efficiency: " + (SelectedDbSchulteTableTask.AmountOfCorrectAnswers / (SelectedDbSchulteTableTask.AmountOfCorrectAnswers + SelectedDbSchulteTableTask.AmountOfWrongAnswers) * 100).ToString() + "%";
                else if (SelectedDbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return "Efficiency: " + TimeSpan.FromMilliseconds(SelectedDbSchulteTableTask.TimeParameter).ToString(@"mm\:ss");
                else
                    return "Efficiency: " + SelectedDbSchulteTableTask.TimeParameter.ToString() + "%";
            }
        }

        public string LongestTimeSpentForFindingNumberLabel
        {
            get
            {
                return "Longest time spent for finding number: " + SelectedDbSchulteTableTask.LongestTimeSpentForFindingNumber + " sec";
            }
        }

        public string LongestTimeLabel
        {
            get
            {
                return "Number: " + SelectedDbSchulteTableTask.LongestTimeNumberString;
            }
        }

        public string ShortestTimeSpentForFindingNumberLabel
        {
            get
            {
                return "Shortest time spent for finding number: " + SelectedDbSchulteTableTask.ShortestTimeSpentForFindingNumber + " sec";
            }
        }

        public string ShortestTimeNumberLabel
        {
            get
            {
                return "Number: " + SelectedDbSchulteTableTask.ShortestTimeNumberString;
            }
        }

        public string DateTimeLabel
        {
            get
            {
                return "Date: " + SelectedDbSchulteTableTask.TaskDateTime.ToString("dd-MM-yy HH:mm");
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

        public bool DetailedTaskOptionsLayoutVisibility
        {
            get
            {
                return _DetailedTasksOptionsLayoutVisibility;
            }
            set
            {
                _DetailedTasksOptionsLayoutVisibility = value;
                OnPropertyChanged("DetailedTaskOptionsLayoutVisibility");
            }
        }

        public bool DetailedTaskOptionsButtonVisibility
        {
            get
            {
                return _DetailedTaskOptionsButtonVisibility;
            }
            set
            {
                _DetailedTaskOptionsButtonVisibility = value;
                OnPropertyChanged("DetailedTaskOptionsButtonVisibility");
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


        public Command ShowHideDetailedStatisticsCommand
        {
            get
            {
                return new Command(() =>
                {
                    DetailedTaskOptionsLayoutVisibility = !DetailedTaskOptionsLayoutVisibility;
                });
            }
        }

        public Command SaveRecordToDbCommand { get; set; }

        private void SaveRecordToDb()
        {
            if (DbSchulteTaskToSave != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    db.SchulteTableTasks.Add(DbSchulteTaskToSave);
                    db.SaveChanges();
                }
                DbSchulteTaskToSave = null;
                LoadMoreCounter = 0;
                DbSchulteTableTasksList.Clear();
                GetMathTasksFromDb();
                SelectedDbSchulteTableTask = DbSchulteTableTasksList[0];
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
            if (SelectedDbSchulteTableTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    DbSchulteTableTask[] dbSchulteTableTasksToDelete = db.SchulteTableTasks.Where(t => t.TimeOption == SelectedDbSchulteTableTask.TimeOption && t.TaskComplexityParameter == SelectedDbSchulteTableTask.TaskComplexityParameter && t.GridSize == SelectedDbSchulteTableTask.GridSize).ToArray();
                    db.SchulteTableTasks.RemoveRange(dbSchulteTableTasksToDelete);
                    db.SaveChanges();
                }
            }

            DbSchulteTaskToSave = null;
            SelectedDbSchulteTableTask = null;

            LoadMoreCounter = 0;

            DbSchulteTableTasksList.Clear();

            DetailedTaskOptionsLayoutVisibility = false;
            DetailedTaskOptionsButtonVisibility = false;
            SaveButtonVisibility = false;

            FillListView();
            InitializeChart();
        }

        public Command LoadGeneralStatistics { get; set; }

        //-------------------------------------------------

        private void GetMathTasksFromDb()
        {
            DbSchulteTableTask[] dbSchulteTableTasks;
            if (SelectedDbSchulteTableTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    dbSchulteTableTasks = db.SchulteTableTasks.Where(t => t.TimeOption == SelectedDbSchulteTableTask.TimeOption && t.TaskComplexityParameter == SelectedDbSchulteTableTask.TaskComplexityParameter && t.GridSize == SelectedDbSchulteTableTask.GridSize).OrderByDescending(t => t.Id).Skip(AmountOfData * LoadMoreCounter).Take(AmountOfData).ToArray();                 
                }

                LoadMoreCounter += 1;
                DbSchulteTableTasksList.AddRange(dbSchulteTableTasks);
            }
        }

        private void FillListView()
        {
            DbSchulteTableTasksListItems = new List<DbSchulteTableTaskListItem>();

            for (int i = 0; i < DbSchulteTableTasksList.Count; i++)
            {
                DbSchulteTableTasksListItems.Add(new DbSchulteTableTaskListItem(DbSchulteTableTasksList[i]));
            }

            OnPropertyChanged("SimilarTasksListViewHeightRequest");
            OnPropertyChanged("dbSchulteTableTaskListItems");
        }

        public int SimilarTasksListViewHeightRequest
        {
            get
            {
                return DbSchulteTableTasksListItems.Count * 50;
            }
            private set { }
        }

        private void InitializeChart()
        {
            OnPropertyChanged("LineChart");
        }
    }
}
