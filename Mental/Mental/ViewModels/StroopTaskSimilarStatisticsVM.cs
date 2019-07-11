using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;
using Microcharts;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
using Mental.Views;

namespace Mental.ViewModels
{
    public class StroopTaskSimilarStatisticsVM : BaseVM
    {
        private int AmountOfData = 3;
        private int LoadMoreCounter = 0;

        private DbStroopTask SelectedDbStroopTask;
        private DbStroopTask DbStroopTaskToSave;

        private List<DbStroopTask> DbStroopTasksList = new List<DbStroopTask>();
        public List<DbStroopTaskListItem> _DbStroopTasksListItems { get; set; }

        public List<DbStroopTaskListItem> DbStroopTasksListItems
        {
            get
            {
                return _DbStroopTasksListItems;
            }
            set
            {
                _DbStroopTasksListItems = value;
                OnPropertyChanged("DbStroopTaskListItems");
            }
        }

        public DbStroopTaskListItem SelectedDbStroopTaskListViewItem
        {
            set
            {
                if (value != null)
                {
                    SelectedDbStroopTask = value.DbStroopTask;
                    OnPropertyChanged("TimeOptionLabel");
                    OnPropertyChanged("AmountOfButtonsLabel");
                    OnPropertyChanged("TaskTypeLabel");
                    OnPropertyChanged("EfficiencyLabel");
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

        public StroopTaskSimilarStatisticsVM(INavigation _navigation,DbStroopTask _dbStroopTask,bool Save)
        {
            navigation = _navigation;
            SaveButtonVisibility = Save;
            GeneralStatisticsButtonVisibility = Save;
            if (Save)
                DbStroopTaskToSave = _dbStroopTask;
            SelectedDbStroopTask = _dbStroopTask;
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();

            SaveRecordToDbCommand = new Command(SaveRecordToDb);
            LoadMoreRecordsCommand = new Command(LoadMoreRecords);
            ClearRecordsCommand = new Command(ClearRecords);
            LoadGeneralStatistics = new Command(async ()=> 
            {
                await navigation.PushAsync(new StroopTaskGeneralStatisticsPage());
            });
        }

        public LineChart LineChart
        {
            get
            {
                List<Entry> entries = new List<Entry>();
                for (int i = DbStroopTasksList.Count - 1; i >= 0; i--)
                {
                    if (SelectedDbStroopTask == DbStroopTasksList[i])
                        entries.Add(new Entry((float)DbStroopTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("000080"), Label = "Selected", ValueLabel = DbStroopTasksList[i].GetEfficiencyParameterString() });
                    else
                    {
                        if (DbStroopTasksList[i].TaskDateTime.Date == DateTime.Now.Date)
                        {
                            entries.Add(new Entry((float)DbStroopTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("1CC9F0"), Label = DbStroopTasksList[i].TaskDateTime.ToString(@"HH:mm"), ValueLabel = DbStroopTasksList[i].GetEfficiencyParameterString() });
                        }
                        else
                            entries.Add(new Entry((float)DbStroopTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("1CC9F0"), Label = DbStroopTasksList[i].TaskDateTime.ToString(@"dd:MM:yy"), ValueLabel = DbStroopTasksList[i].GetEfficiencyParameterString() });
                    }

                }
                if (DbStroopTaskToSave != null)
                    entries.Add(new Entry((float)DbStroopTaskToSave.GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FF1493"), Label = "Current", ValueLabel = DbStroopTaskToSave.GetEfficiencyParameterString() });
                return new LineChart() { Entries = entries };
            }
            private set { }
        }

        public string TimeOptionLabel
        {
            get
            {
                if (SelectedDbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return "Time Option: Countdown";
                else if (SelectedDbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return "Time Option: Limited Operations";
                else
                    return "Time Options: Last Task";

            }
        }

        public string AmountOfButtonsLabel
        {
            get
            {
                return "Amount of Buttons: " + SelectedDbStroopTask.AmountOfButtons;
            }
        }

        public string TaskTypeLabel
        {
            get
            {
                if (SelectedDbStroopTask.StroopTaskOption == (byte)StroopTaskType.FindOneCorrect)
                    return "Task: Find 1";
                else if (SelectedDbStroopTask.StroopTaskOption == (byte)StroopTaskType.TrueOrFalse)
                    return "Task: True/False";
                else
                    return "Task: Find Color";
            }
        }

        public string EfficiencyLabel
        {
            get
            {
                return "Efficiency: " + SelectedDbStroopTask.GetEfficiencyParameterString();
                //if (SelectedDbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                //    return "Efficiency: " + (SelectedDbStroopTask.AmountOfCorrectAnswers / (SelectedDbStroopTask.AmountOfCorrectAnswers + SelectedDbStroopTask.AmountOfWrongAnswers) * 100).ToString() + "%";
                //else if (SelectedDbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                //    return "Efficiency: " + TimeSpan.FromMilliseconds(SelectedDbStroopTask.TimeParameter).ToString(@"mm\:ss");
                //else
                //    return "Efficiency: " + SelectedDbStroopTask.TimeParameter.ToString() + "%";
            }
        }

        public string AmountOfCorrectAnswersLabel
        {
            get
            {
                return "Correct Answers: " + SelectedDbStroopTask.AmountOfCorrectAnswers.ToString();
            }
        }

        public string AmountOfWrongAnswersLabel
        {
            get
            {
                return "Wrong Answers: " + SelectedDbStroopTask.AmountOfWrongAnswers.ToString();
            }
        }

        //public string LongestTimeSpentForFindingNumberLabel
        //{
        //    get
        //    {
        //        return "Longest time spent for finding number: " + SelectedDbSchulteTableTask.LongestTimeSpentForFindingNumber + " sec";
        //    }
        //}

        //public string LongestTimeLabel
        //{
        //    get
        //    {
        //        return "Number: " + SelectedDbSchulteTableTask.LongestTimeNumberString;
        //    }
        //}

        //public string ShortestTimeSpentForFindingNumberLabel
        //{
        //    get
        //    {
        //        return "Shortest time spent for finding number: " + SelectedDbSchulteTableTask.ShortestTimeSpentForFindingNumber + " sec";
        //    }
        //}

        //public string ShortestTimeNumberLabel
        //{
        //    get
        //    {
        //        return "Number: " + SelectedDbSchulteTableTask.ShortestTimeNumberString;
        //    }
        //}

        public string DateTimeLabel
        {
            get
            {
                return "Date: " + SelectedDbStroopTask.TaskDateTime.ToString("dd-MM-yy HH:mm");
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
            if (DbStroopTaskToSave != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    db.StroopTasks.Add(DbStroopTaskToSave);
                    db.SaveChanges();
                }
                DbStroopTaskToSave = null;
                LoadMoreCounter = 0;
                DbStroopTasksList.Clear();
                GetMathTasksFromDb();
                SelectedDbStroopTask = DbStroopTasksList[0];
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
            if (SelectedDbStroopTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    DbStroopTask[] dbStroopTasksToDelete = db.StroopTasks.Where(t => t.TimeOption == SelectedDbStroopTask.TimeOption && t.StroopTaskOption == SelectedDbStroopTask.StroopTaskOption && t.TaskComplexityParameter == SelectedDbStroopTask.TaskComplexityParameter && t.AmountOfButtons == SelectedDbStroopTask.AmountOfButtons).ToArray();
                    db.StroopTasks.RemoveRange(dbStroopTasksToDelete);
                    db.SaveChanges();
                }
            }

            DbStroopTaskToSave = null;
            SelectedDbStroopTask = null;

            LoadMoreCounter = 0;

            DbStroopTasksList.Clear();

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
            DbStroopTask[] dbStroopTasks;
            if (SelectedDbStroopTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    dbStroopTasks = db.StroopTasks.Where(t => t.TimeOption == SelectedDbStroopTask.TimeOption && t.StroopTaskOption == SelectedDbStroopTask.StroopTaskOption && t.TaskComplexityParameter == SelectedDbStroopTask.TaskComplexityParameter && t.AmountOfButtons == SelectedDbStroopTask.AmountOfButtons).OrderByDescending(t => t.Id).Skip(AmountOfData * LoadMoreCounter).Take(AmountOfData).ToArray();
                }

                LoadMoreCounter += 1;
                DbStroopTasksList.AddRange(dbStroopTasks);
            }
        }

        private void FillListView()
        {
            _DbStroopTasksListItems = new List<DbStroopTaskListItem>();

            for (int i = 0; i < DbStroopTasksList.Count; i++)
            {
                _DbStroopTasksListItems.Add(new DbStroopTaskListItem(DbStroopTasksList[i]));
            }

            OnPropertyChanged("SimilarTasksListViewHeightRequest");
            OnPropertyChanged("DbStroopTasksListItems");
        }

        public int SimilarTasksListViewHeightRequest
        {
            get
            {
                return DbStroopTasksList.Count * 50;
            }
            private set { }
        }

        private void InitializeChart()
        {
            OnPropertyChanged("LineChart");
        }
    }
}
