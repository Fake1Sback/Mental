using Mental.Models;
using Mental.Models.DbModels;
using Microcharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using ChartEntry = Microcharts.ChartEntry;
using Mental.Views;
using Microsoft.EntityFrameworkCore;

namespace Mental.ViewModels
{
    public class SimilarSchulteTableTasksStatisticsVM : BaseVM
    {
        private int AmountOfData = 3;
        private int LoadMoreCounter = 0;

        private DbSchulteTableTask _SelectedDbSchulteTableTask;
        public DbSchulteTableTaskListItem SelectedDbSchulteTableTaskListItem
        {
            set
            {
                DbSchulteTableTaskListItem schulteTableTaskListItem = value;
                if (schulteTableTaskListItem != null)
                {
                    _SelectedDbSchulteTableTask = schulteTableTaskListItem.DbSchulteTableTask;
                    foreach (DbSchulteTableTaskListItem item in DbSchulteTableTasksListItems)
                        item.SetDefaultColor();
                    schulteTableTaskListItem.SetActiveColor();
                    InitializeChart();
                }
            }
        }

        private DbSchulteTableTask _DbSchulteTaskToSave;
        private DbSchulteTableTask _PatternDbSchulteTableTask;

        private List<DbSchulteTableTask> _DbSchulteTableTasksList = new List<DbSchulteTableTask>();

        public List<DbSchulteTableTaskListItem> DbSchulteTableTasksListItems { get; set; }   
        public List<DbSchulteTableTaskListItem> DbSchulteTableTasksListItemsProp
        {
            get
            {
                return DbSchulteTableTasksListItems;
            }
            set
            {
                DbSchulteTableTasksListItems = value;
                OnPropertyChanged("DbSchulteTableTasksListItemsProp");
            }
        }
      
        private INavigation navigation;

        private bool _SaveButtonVisibility;
        private bool _GeneralStatisticsButtonVisibility = true;

        public SimilarSchulteTableTasksStatisticsVM(INavigation _navigation,DbSchulteTableTask _dbSchulteTableTask,bool Save)
        {
            navigation = _navigation;
            SaveButtonVisibility = Save;
            GeneralStatisticsButtonVisibility = Save;
            if (Save)
                _DbSchulteTaskToSave = _dbSchulteTableTask;
            _SelectedDbSchulteTableTask = _dbSchulteTableTask;
            _PatternDbSchulteTableTask = _dbSchulteTableTask;
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();

            SaveRecordToDbCommand = new Command(SaveRecordToDb);
            LoadMoreRecordsCommand = new Command(LoadMoreRecords);
            ClearRecordsCommand = new Command(ClearRecords);
            LoadGeneralStatistics = new Command(async () => { await navigation.PushAsync(new SchulteTableTasksGeneralStatisticsPage()); });

            MessagingCenter.Subscribe<BaseVM>(this, "ReloadRecords", (vm) =>
            {
                if (vm != this)
                {
                    LoadMoreCounter = 0;
                    _SelectedDbSchulteTableTask = null;
                    _DbSchulteTableTasksList.Clear();

                    GetMathTasksFromDb();
                    FillListView();
                    InitializeChart();
                }
            });
        }

        //---------------------TopFrameValue-------------------------------

        public string TopFrameGridSize
        {
            get
            {
                return _PatternDbSchulteTableTask.GridSize + " x " + _PatternDbSchulteTableTask.GridSize;
            }
        }

        public string TopFrameEasyMode
        {
            get
            {
                if (_PatternDbSchulteTableTask.IsEasyModeActivated)
                    return " Easy Mode ";
                else
                    return "Normal Mode";
            }
        }

        public string TopFrameEasyModeSrc
        {
            get
            {
                if (_PatternDbSchulteTableTask.IsEasyModeActivated)
                    return "Easy_Mode_24.png";
                else
                    return "circle_outline_white_24.png";
            }
        }

        public string TopFrameTimeParametersImgSrc
        {
            get
            {
                if (_PatternDbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_24.png";
                else if (_PatternDbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return "access_time_white_24.png";
                else
                    return "Stopwatch_24.png";
            }
        }

        public string TopFrameTimeParameters
        {
            get
            {
                if (_PatternDbSchulteTableTask.TimeOption == (byte)TimeOptions.CountdownTimer)
                    return _PatternDbSchulteTableTask.TaskComplexityParameter.ToString() + " min";
                else if (_PatternDbSchulteTableTask.TimeOption == (byte)TimeOptions.FixedAmountOfOperations)
                    return _PatternDbSchulteTableTask.TaskComplexityParameter.ToString() + " tasks";
                else
                    return _PatternDbSchulteTableTask.TaskComplexityParameter.ToString() + " sec";
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

        private async void SaveRecordToDb()
        {
            if (_DbSchulteTaskToSave != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    await db.SchulteTableTasks.AddAsync(_DbSchulteTaskToSave);
                    await db.SaveChangesAsync();
                }
                _DbSchulteTaskToSave = null;
                _SelectedDbSchulteTableTask = null;
                LoadMoreCounter = 0;
                _DbSchulteTableTasksList.Clear();
                GetMathTasksFromDb();
                FillListView();
                InitializeChart();
              //  SaveButtonVisibility = false;
            }
        }

        public Command LoadMoreRecordsCommand { get; set; }

        private void LoadMoreRecords()
        {
            GetMathTasksFromDb();
            FillListView();
            _SelectedDbSchulteTableTask = null;
            InitializeChart();
        }

        public Command ClearRecordsCommand { get; set; }

        private async void ClearRecords()
        {
            using (var db = new ApplicationContext("mental.db"))
            {
                DbSchulteTableTask[] dbSchulteTableTasksToDelete = await db.SchulteTableTasks.Where(t => t.TimeOption == _PatternDbSchulteTableTask.TimeOption &&
                t.TaskComplexityParameter == _PatternDbSchulteTableTask.TaskComplexityParameter &&
                t.GridSize == _PatternDbSchulteTableTask.GridSize &&
                t.IsEasyModeActivated == _PatternDbSchulteTableTask.IsEasyModeActivated).ToArrayAsync();


                if (dbSchulteTableTasksToDelete != null)
                {
                    if(dbSchulteTableTasksToDelete.Length != 0)
                    {
                        db.SchulteTableTasks.RemoveRange(dbSchulteTableTasksToDelete);
                        await db.SaveChangesAsync();
                    }
                }

            }

            _SelectedDbSchulteTableTask = null;
            _DbSchulteTableTasksList.Clear();

            FillListView();
            InitializeChart();

            MessagingCenter.Send<BaseVM>(this, "ReloadRecords");
        }

        public Command LoadGeneralStatistics { get; set; }

        //-------------------------------------------------

        private async void GetMathTasksFromDb()
        {
            DbSchulteTableTask[] dbSchulteTableTasks;

            using (var db = new ApplicationContext("mental.db"))
            {
                dbSchulteTableTasks = await db.SchulteTableTasks.Where(t => t.TimeOption == _PatternDbSchulteTableTask.TimeOption &&
                t.TaskComplexityParameter == _PatternDbSchulteTableTask.TaskComplexityParameter &&
                t.GridSize == _PatternDbSchulteTableTask.GridSize &&
                t.IsEasyModeActivated == _PatternDbSchulteTableTask.IsEasyModeActivated).OrderByDescending(t => t.Id).Skip(AmountOfData * LoadMoreCounter).Take(AmountOfData).ToArrayAsync();
            }

            if(dbSchulteTableTasks != null)
            {
                if(dbSchulteTableTasks.Length != 0)
                {
                    LoadMoreCounter += 1;
                    _DbSchulteTableTasksList.AddRange(dbSchulteTableTasks);
                }
            }
        }

        private void FillListView()
        {
            DbSchulteTableTasksListItems = new List<DbSchulteTableTaskListItem>();

            for (int i = 0; i < _DbSchulteTableTasksList.Count; i++)
            {
                DbSchulteTableTasksListItems.Add(new DbSchulteTableTaskListItem(_DbSchulteTableTasksList[i]));
            }

            OnPropertyChanged("DbSchulteTableTasksListItemsProp");
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
                for (int i = _DbSchulteTableTasksList.Count - 1; i >= 0; i--)
                {
                    if (_SelectedDbSchulteTableTask == _DbSchulteTableTasksList[i])
                        entries.Add(new ChartEntry((float)_DbSchulteTableTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("#99ffcc"), TextColor = SkiaSharp.SKColor.Parse("#99ffcc"), Label = "Selected", ValueLabel = _DbSchulteTableTasksList[i].GetEfficiencyParameterString() });
                    else
                    {
                        if (_DbSchulteTableTasksList[i].TaskDateTime.Date == DateTime.Now.Date)
                        {
                            entries.Add(new ChartEntry((float)_DbSchulteTableTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FAFAFA"), TextColor = SkiaSharp.SKColor.Parse("FAFAFA"), Label = _DbSchulteTableTasksList[i].TaskDateTime.ToString(@"HH:mm"), ValueLabel = _DbSchulteTableTasksList[i].GetEfficiencyParameterString() });
                        }
                        else
                            entries.Add(new ChartEntry((float)_DbSchulteTableTasksList[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FAFAFA"), TextColor = SkiaSharp.SKColor.Parse("FAFAFA"), Label = _DbSchulteTableTasksList[i].TaskDateTime.ToString(@"dd:MM:yy"), ValueLabel = _DbSchulteTableTasksList[i].GetEfficiencyParameterString() });
                    }

                }
                if (_DbSchulteTaskToSave != null)
                    entries.Add(new ChartEntry((float)_DbSchulteTaskToSave.GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("#ff3333"), TextColor = SkiaSharp.SKColor.Parse("#ff3333"), Label = "Current", ValueLabel = _DbSchulteTaskToSave.GetEfficiencyParameterString() });
                return new LineChart() { Entries = entries, LineMode = LineMode.Straight, PointMode = PointMode.Circle, PointAreaAlpha = 0, LineSize = 7, PointSize = 30, LineAreaAlpha = 0, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff"), LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal, LabelTextSize = 40, LabelColor = SkiaSharp.SKColor.Parse("#fafafa"), IsAnimated = false };
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
