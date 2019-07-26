using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;
using Microcharts;
using Xamarin.Forms;
using ChartEntry = Microcharts.ChartEntry;
using Mental.Views;

namespace Mental.ViewModels
{
    public class StroopTaskSimilarStatisticsVM : BaseVM
    {
        private int AmountOfData = 4;
        private int LoadMoreCounter = 0;

        private DbStroopTask _SelectedDbStroopTask;
        public DbStroopTaskListItem SelectedDbStroopTask
        {
            set
            {
                DbStroopTaskListItem dbStroopTaskListItem = value;
                if(dbStroopTaskListItem != null)
                {
                    _SelectedDbStroopTask = dbStroopTaskListItem.DbStroopTask;
                    foreach (DbStroopTaskListItem item in DbStroopTasksListItems)
                        item.SetDefaultColor();
                    dbStroopTaskListItem.SetActiveColor();
                    InitializeChart();
                }
            }
        }

        private DbStroopTask _DbStroopTaskToSave;
        private DbStroopTask _PatternDbStroopTask;

        private List<DbStroopTask> _DbStroopTasksList = new List<DbStroopTask>();

        public List<DbStroopTaskListItem> DbStroopTasksListItems { get; set; }
        public List<DbStroopTaskListItem> DbStroopTasksListItemsProp
        {
            get
            {
                return DbStroopTasksListItems;
            }
            set
            {
                DbStroopTasksListItems = value;
                OnPropertyChanged(" DbStroopTasksListItemsProp");
            }
        }
    
        private INavigation navigation;

        private bool _SaveButtonVisibility;
        private bool _GeneralStatisticsButtonVisibility = true;

        public StroopTaskSimilarStatisticsVM(INavigation _navigation,DbStroopTask _dbStroopTask,bool Save)
        {
            navigation = _navigation;
            SaveButtonVisibility = Save;
            GeneralStatisticsButtonVisibility = Save;
            if (Save)
                _DbStroopTaskToSave = _dbStroopTask;
            _SelectedDbStroopTask = _dbStroopTask;
            _PatternDbStroopTask = _dbStroopTask;
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

        //---------------------TopFrameValue-------------------------------

        public int TopFrameAmountOfButtons
        {
            get
            {
                return _PatternDbStroopTask.AmountOfButtons;
            }
        }

        public string TopFrameStroopTaskTypeImgSrc
        {
            get
            {
                if (_PatternDbStroopTask.StroopTaskOption == (byte)StroopTaskType.FindOneCorrect)
                    return "Find_One_24.png";
                else if (_PatternDbStroopTask.StroopTaskOption == (byte)StroopTaskType.TrueOrFalse)
                    return "True_False_24.png";
                else
                    return "Color_By_Text_24.png";
            }
        }

        public string TopFrameStroopTaskType
        {
            get
            {
                if (_PatternDbStroopTask.StroopTaskOption == (byte)StroopTaskType.FindOneCorrect)
                    return "Find 1 Correct";
                else if (_PatternDbStroopTask.StroopTaskOption == (byte)StroopTaskType.TrueOrFalse)
                    return "True / False";
                else
                    return "Find Color by Text";
            }
        }

        public string TopFrameTimeParametersImgSrc
        {
            get
            {
                if (_PatternDbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_24.png";
                else if (_PatternDbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return "access_time_white_24.png";
                else
                    return "Stopwatch_24.png";
            }
        }

        public string TopFrameTimeParameters
        {
            get
            {
                if (_PatternDbStroopTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return _PatternDbStroopTask.TaskComplexityParameter.ToString() + " min";
                else if (_PatternDbStroopTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return _PatternDbStroopTask.TaskComplexityParameter.ToString() + " tasks";
                else
                    return _PatternDbStroopTask.TaskComplexityParameter.ToString() + " sec";
            }
        }

        //-----------------------------------------------------------------

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

        public Command SaveRecordToDbCommand { get; set; }

        private void SaveRecordToDb()
        {
            if (_DbStroopTaskToSave != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    db.StroopTasks.Add(_DbStroopTaskToSave);
                    db.SaveChanges();
                }
                _DbStroopTaskToSave = null;
                LoadMoreCounter = 0;
                _DbStroopTasksList.Clear();
                GetMathTasksFromDb();
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
            _SelectedDbStroopTask = null;
            InitializeChart();
        }

        public Command ClearRecordsCommand { get; set; }

        private void ClearRecords()
        {
            using (var db = new ApplicationContext("mental.db"))
            {
                DbStroopTask[] dbStroopTasksToDelete = db.StroopTasks.Where(t => t.TimeOption == _PatternDbStroopTask.TimeOption &&
                t.StroopTaskOption == _PatternDbStroopTask.StroopTaskOption &&
                t.TaskComplexityParameter == _PatternDbStroopTask.TaskComplexityParameter &&
                t.AmountOfButtons == _PatternDbStroopTask.AmountOfButtons).ToArray();


                if (dbStroopTasksToDelete != null)
                {
                    if (dbStroopTasksToDelete.Length != 0)
                    {
                        db.StroopTasks.RemoveRange(dbStroopTasksToDelete);
                        db.SaveChanges();
                    }
                }
            }

            LoadMoreCounter = 0;
            _SelectedDbStroopTask = null;
            _DbStroopTasksList.Clear();

            FillListView();
            InitializeChart();

            MessagingCenter.Send<BaseVM>(this, "ReloadRecords");
        }

        public Command LoadGeneralStatistics { get; set; }

        //-------------------------------------------------

        private void GetMathTasksFromDb()
        {
            DbStroopTask[] dbStroopTasks;

            using (var db = new ApplicationContext("mental.db"))
            {
                dbStroopTasks = db.StroopTasks.Where(t => t.TimeOption == _PatternDbStroopTask.TimeOption &&
                t.StroopTaskOption == _PatternDbStroopTask.StroopTaskOption &&
                t.TaskComplexityParameter == _PatternDbStroopTask.TaskComplexityParameter &&
                t.AmountOfButtons == _PatternDbStroopTask.AmountOfButtons).OrderByDescending(t => t.Id).Skip(AmountOfData * LoadMoreCounter).Take(AmountOfData).ToArray();
            }

            if(dbStroopTasks != null)
            {
                if(dbStroopTasks.Length != 0)
                {
                    LoadMoreCounter += 1;
                    _DbStroopTasksList.AddRange(dbStroopTasks);
                }
            }
        }

        private void FillListView()
        {
            DbStroopTasksListItems = new List<DbStroopTaskListItem>();

            for (int i = 0; i < _DbStroopTasksList.Count; i++)
            {
                DbStroopTasksListItems.Add(new DbStroopTaskListItem(_DbStroopTasksList[i]));
            }

            OnPropertyChanged("DbStroopTasksListItems");
        }

        public int SimilarTasksListViewHeightRequest
        {
            get
            {
                return _DbStroopTasksList.Count * 50;
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
                List<ChartEntry> entries = new List<ChartEntry>();
                for (int i = _DbStroopTasksList.Count - 1; i >= 0; i--)
                {
                    if (_SelectedDbStroopTask == _DbStroopTasksList[i])
                        entries.Add(new ChartEntry((float)_DbStroopTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("#99ffcc"), TextColor = SkiaSharp.SKColor.Parse("#99ffcc"), Label = "Selected", ValueLabel = _DbStroopTasksList[i].GetEfficiencyParameterString() });
                    else
                    {
                        if (_DbStroopTasksList[i].TaskDateTime.Date == DateTime.Now.Date)
                        {
                            entries.Add(new ChartEntry((float)_DbStroopTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FAFAFA"), TextColor = SkiaSharp.SKColor.Parse("FAFAFA"), Label = _DbStroopTasksList[i].TaskDateTime.ToString(@"HH:mm"), ValueLabel = _DbStroopTasksList[i].GetEfficiencyParameterString() });
                        }
                        else
                            entries.Add(new ChartEntry((float)_DbStroopTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FAFAFA"), TextColor = SkiaSharp.SKColor.Parse("FAFAFA"), Label = _DbStroopTasksList[i].TaskDateTime.ToString(@"dd:MM:yy"), ValueLabel = _DbStroopTasksList[i].GetEfficiencyParameterString() });
                    }

                }
                if (_DbStroopTaskToSave != null)
                    entries.Add(new ChartEntry((float)_DbStroopTaskToSave.GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("#ff3333"), TextColor = SkiaSharp.SKColor.Parse("#ff3333"), Label = "Current", ValueLabel = _DbStroopTaskToSave.GetEfficiencyParameterString() });
                return new LineChart() { Entries = entries, LineMode = LineMode.Straight, PointMode = PointMode.Circle, PointAreaAlpha = 0, LineSize = 3, PointSize = 10, LineAreaAlpha = 0, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff") };
            }
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
