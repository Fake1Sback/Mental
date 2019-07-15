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
    public class SimilarMathTasksStatisticsVM : BaseVM
    {
        private int AmountOfData = 4;
        private int LoadMoreCounter = 0;

        private DbMathTask PatternDbMathTask;
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
        public DbMathTaskListItem SelectedListViewItem
        {
            set
            {
                DbMathTaskListItem dbMathTaskListItem = value;
                if (dbMathTaskListItem != null)
                {
                    SelectedListItemDbMathTask = dbMathTaskListItem.dbMathTask;
                    foreach (DbMathTaskListItem item in DbMathTaskListItems)
                    {
                        item.SetDefaultColor();
                    }
                    dbMathTaskListItem.SetActiveColor();
                    InitializeChart();
                }
            }
        }

        private INavigation navigation;
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
            PatternDbMathTask = _dbMathTask;
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();

            SaveRecordToDbCommand = new Command(SaveRecordToDb);
            LoadMoreRecordsCommand = new Command(LoadMoreRecords);
            ClearRecordsCommand = new Command(ClearRecords);
            LoadGeneralStatistics = new Command(LoadMoreRecords);
            LoadGeneralStatistics = new Command(async () => { await navigation.PushAsync(new GeneralStatisticsPage()); });
        }


        //--------Top Frame values----------------

        public string TopFrameOperationsString
        {
            get
            {
                string str = string.Empty;
                for(int i = 0;i < PatternDbMathTask.Operations.Length;i++)
                {
                    str += PatternDbMathTask.Operations[i] + " ";
                }
                str = str.Remove(str.Length - 1);
                return str;
            }
        }

        public int TopFrameMinValue
        {
            get
            {
                return PatternDbMathTask.MinValue;
            }
        }

        public int TopFrameMaxValue
        {
            get
            {
                return PatternDbMathTask.MaxValue;
            }
        }

        public string TopFrameTaskTypeString
        {
            get
            {
                if (PatternDbMathTask.TaskType == (byte)TaskType.CountResult)
                    return "Result";
                else
                    return " X ";
            }
        }

        public string TopFrameTimeParametersImgSrc
        {
            get
            {
                if (PatternDbMathTask.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return "list_numbered_white_24.png";
                else
                    return "access_time_white_24.png";
            }
        }

        public string TopFrameTimeParameters
        {
            get
            {
                if (PatternDbMathTask.TimeOptions == (byte)TimeOptions.CountdownTimer)
                    return PatternDbMathTask.TaskComplexityParameter.ToString() + " min";
                else if (PatternDbMathTask.TimeOptions == (byte)TimeOptions.FixedAmountOfOperations)
                    return PatternDbMathTask.TaskComplexityParameter.ToString() + " tasks";
                else
                    return PatternDbMathTask.TaskComplexityParameter.ToString() + " sec";
            }
        }

        public string TopFrameNumbersType
        {
            get
            {
                if (PatternDbMathTask.IsInteger)
                    return "INTEGER";
                else
                    return "FRACTIONAL";
            }
        }

        public string TopFrameChainLengthImgSrc
        {
            get
            {
                if (PatternDbMathTask.IsChainLengthFixed)
                    return "Chain_24.png";
                else
                    return "Broken_Chain_2_24.png";
            }
        }

        public string TopFrameMaxChainLength
        {
            get
            {
                return "Max" +
                    ": " + PatternDbMathTask.MaxChainLength.ToString();
            }
        }

        public string TopFrameRestrictionsImgSrc
        {
            get
            {
                if (PatternDbMathTask.IsRestrictionActivated)
                    return "Restrictions_24.png";
                else
                    return "";

            }
        }

        public string TopFrameDigitsAfterDotSign
        {
            get
            {
                string str = ".";
                for(int i = 0;i < PatternDbMathTask.DigitsAfterDotSing;i++)
                {
                    str += "X";
                }
                return str;
            }
        }

        public bool TopFrameDigitsAfterDotSignVisibility
        {
            get
            {
                if (!PatternDbMathTask.IsInteger)
                    return true;
                else
                    return false;
            }
        }

        //-------------------------------
   
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
            SelectedListItemDbMathTask = null;
            InitializeChart();
        }

        public Command ClearRecordsCommand { get; set; }

        private void ClearRecords()
        {
            using (var db = new ApplicationContext("mental.db"))
            {
                DbMathTask[] mathTasksToDelete = db.mathTasks.Where(t => t.TimeOptions == PatternDbMathTask.TimeOptions &&
                t.Operations == PatternDbMathTask.Operations &&
                t.TaskType == PatternDbMathTask.TaskType &&
                t.TaskComplexityParameter == PatternDbMathTask.TaskComplexityParameter &&
                t.MinValue == PatternDbMathTask.MinValue &&
                t.MaxValue == PatternDbMathTask.MaxValue &&
                t.IsChainLengthFixed == PatternDbMathTask.IsChainLengthFixed &&
                t.MaxChainLength == PatternDbMathTask.MaxChainLength &&
                t.IsInteger == PatternDbMathTask.IsInteger &&
                t.IsRestrictionActivated == PatternDbMathTask.IsRestrictionActivated).ToArray();

                if (mathTasksToDelete != null)
                {
                    if (mathTasksToDelete.Length != 0)
                    {
                        if (!PatternDbMathTask.IsInteger)
                            mathTasksToDelete = mathTasksToDelete.Where(t => t.DigitsAfterDotSing == PatternDbMathTask.DigitsAfterDotSing).ToArray();

                        if (PatternDbMathTask.IsRestrictionActivated)
                            mathTasksToDelete = mathTasksToDelete.Where(t => t.RestrictionsString == PatternDbMathTask.RestrictionsString).ToArray();

                        db.mathTasks.RemoveRange(mathTasksToDelete);
                        db.SaveChanges();
                    }
                }
            }

            SelectedListItemDbMathTask = null;
            LoadMoreCounter = 0;
            ListOfMathTasks.Clear();

            FillListView();
            InitializeChart();
        }

        public Command LoadGeneralStatistics { get; set; }

        //--------------------------------------------

        private void GetMathTasksFromDb()
        {
            DbMathTask[] dbMathTasksArray;

            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTasksArray = db.mathTasks.Where(t => t.TimeOptions == PatternDbMathTask.TimeOptions &&
                t.Operations == PatternDbMathTask.Operations &&
                t.TaskType == PatternDbMathTask.TaskType &&
                t.TaskComplexityParameter == PatternDbMathTask.TaskComplexityParameter &&
                t.MinValue == PatternDbMathTask.MinValue &&
                t.MaxValue == PatternDbMathTask.MaxValue &&
                t.IsChainLengthFixed == PatternDbMathTask.IsChainLengthFixed &&
                t.MaxChainLength == PatternDbMathTask.MaxChainLength &&
                t.IsInteger == PatternDbMathTask.IsInteger &&
                t.IsRestrictionActivated == PatternDbMathTask.IsRestrictionActivated).OrderByDescending(t => t.Id).Skip(AmountOfData * LoadMoreCounter).Take(AmountOfData).ToArray();
            }

            if (dbMathTasksArray != null)
            {
                if (dbMathTasksArray.Length != 0)
                {
                    if (!PatternDbMathTask.IsInteger)
                        dbMathTasksArray = dbMathTasksArray.Where(t => t.DigitsAfterDotSing == PatternDbMathTask.DigitsAfterDotSing).ToArray();

                    if (PatternDbMathTask.IsRestrictionActivated)
                        dbMathTasksArray = dbMathTasksArray.Where(t => t.RestrictionsString == PatternDbMathTask.RestrictionsString).ToArray();

                    LoadMoreCounter += 1;
                    ListOfMathTasks.AddRange(dbMathTasksArray);
                }
            }
        }

        private void FillListView()
        {
            DbMathTaskListItems = new List<DbMathTaskListItem>();

            for (int i = 0; i < ListOfMathTasks.Count; i++)
            {
                DbMathTaskListItems.Add(new DbMathTaskListItem(ListOfMathTasks[i]));
            }

            OnPropertyChanged("DbMathTaskListItemsProp");
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
                        entries.Add(new Entry((float)ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("#99ffcc"), TextColor = SkiaSharp.SKColor.Parse("#99ffcc"), Label = "Selected", ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString() });
                    else
                    {
                        if(ListOfMathTasks[i].TaskDateTime.Date == DateTime.Now.Date)
                        {
                            entries.Add(new Entry((float)ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FAFAFA"), TextColor = SkiaSharp.SKColor.Parse("FAFAFA"), Label = ListOfMathTasks[i].TaskDateTime.ToString(@"HH:mm"), ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString() });
                        }
                        else
                            entries.Add(new Entry((float)ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FAFAFA"), TextColor = SkiaSharp.SKColor.Parse("FAFAFA"),  Label = ListOfMathTasks[i].TaskDateTime.ToString(@"dd:MM:yy"), ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString() });
                    }
                       
                }
                if (dbMathTaskToSave != null)
                    entries.Add(new Entry((float)dbMathTaskToSave.GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("#ff3333"), TextColor = SkiaSharp.SKColor.Parse("#ff3333"), Label = "Current", ValueLabel = dbMathTaskToSave.GetEfficiencyParameterString() });
                return new LineChart() { Entries = entries, LineMode = LineMode.Straight, PointMode = PointMode.Circle,  PointAreaAlpha = 0, LineSize = 3, PointSize = 10, LineAreaAlpha = 0, BackgroundColor = SkiaSharp.SKColor.Parse("#6699ff")};
            }
        }
    }
}
