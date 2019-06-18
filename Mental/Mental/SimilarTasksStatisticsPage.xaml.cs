using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models.DbModels;
using Mental.Models;
using Entry = Microcharts.Entry;
using Microcharts;

namespace Mental
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimilarTasksStatisticsPage : ContentPage
    {
        private int AmountOfData = 3;
        private int LoadMoreCounter = 0;

        private DbMathTask SelectedListItemDbMathTask;
        private DbMathTask dbMathTaskToSave;

        private List<DbMathTask> ListOfMathTasks = new List<DbMathTask>();
        public List<DbMathTaskListItem> DbMathTaskListItems { get; set; }

        public SimilarTasksStatisticsPage()
        {
            this.BindingContext = this;
            dbMathTaskToSave = new DbMathTask()
            {
                AmountOfXDigits = 0,
                AmountOfWrongAnswers = 0,
                AmountOfCorrectAnswers = 15,
                AmountOfMinutes = 1,
                AmountOfTasks = 0,
                DigitsAfterDotSing = 0,
                Id = 100,
                IsChainLengthFixed = false,
                IsInteger = true,
                IsSpecialModeActivated = false,
                LongestTimeExpressionString = "5 + 5",
                LongestTimeSpentForExpression = 13,
                MaxChainLength = 2,
                MaxValue = 35,
                MinValue = 0,
                Operations = "+-",
                ShortestTimeExpressionString = "2 + 2",
                ShortestTimeSpentForExpression = 1,
                TaskType = 0,
                TimeOptions = 0,
            };
            SelectedListItemDbMathTask = dbMathTaskToSave;
            InitializeComponent();
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();
        }

        public SimilarTasksStatisticsPage(DbMathTask _dbMathTask, bool Save)
        {
            this.BindingContext = this;
            if (Save)
                dbMathTaskToSave = _dbMathTask;
            else
                SaveResultButton.IsVisible = false;
            SelectedListItemDbMathTask = _dbMathTask;
            InitializeComponent();
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();
        }

        private void FillView()
        {
            string operations = SelectedListItemDbMathTask.Operations;
            if (operations.Contains("+"))
                PlusLabel.IsVisible = true;
            if (operations.Contains("-"))
                MinusLabel.IsVisible = true;
            if (operations.Contains("*"))
                MultiplyLabel.IsVisible = true;
            if (operations.Contains("/"))
                DivideLabel.IsVisible = true;

            if (SelectedListItemDbMathTask.TaskType == 0)
                TaskTypeLabel.Text = "Task type: Find Result";
            else
                TaskTypeLabel.Text = "Task type: Find X";

            if (SelectedListItemDbMathTask.TimeOptions == 0)
                TimeOptionLabel.Text = "Time option: Countdown timer";
            else
                TimeOptionLabel.Text = "Time option: Limited amount of tasks";

            MinValueLabel.Text = "Min Value: " + SelectedListItemDbMathTask.MinValue.ToString();
            MaxValueLabel.Text = "Max Value: " + SelectedListItemDbMathTask.MaxValue.ToString();

            if (SelectedListItemDbMathTask.IsChainLengthFixed)
                ChainLengthFixedLabel.Text = "Chain length is fixed";
            else
                ChainLengthFixedLabel.Text = "Chain length is not fixed";

            MaxChainLengthLabel.Text = "Max chain length: " + SelectedListItemDbMathTask.MaxChainLength.ToString();
            if (SelectedListItemDbMathTask.IsInteger)
                DataTypeLabel.Text = "Numbers type: Integer";
            else
            {
                DigitsAfterDotSignLabel.IsVisible = true;
                DataTypeLabel.Text = "Numbers type: Fractional";
                DigitsAfterDotSignLabel.Text = "Precision: " + SelectedListItemDbMathTask.DigitsAfterDotSing;
            }

            if (SelectedListItemDbMathTask.IsSpecialModeActivated)
            {
                SpecialModeActiavetedLabel.IsVisible = true;
                SpecialModeActiavetedLabel.Text = "Special mode";
                SpecialModeRestricationsLabel.IsVisible = true;
                SpecialModeRestricationsLabel.Text = "Amount of digits for special numbers: " + SelectedListItemDbMathTask.AmountOfXDigits;
            }

            TaskResultsLabel.Text = "Correct answers: " + SelectedListItemDbMathTask.AmountOfCorrectAnswers + " Wrong answers: " + SelectedListItemDbMathTask.AmountOfWrongAnswers;
            LongestTimeSpentForExpressionLabel.Text = "Longest time spent for expression: " + TimeSpan.FromSeconds(SelectedListItemDbMathTask.LongestTimeSpentForExpression);
            LongestTimeExpressionLabel.Text = "Expression: " + SelectedListItemDbMathTask.LongestTimeExpressionString;
            ShortestTimeSpentForExpressionLabel.Text = "Shortest time spent for expression: " + TimeSpan.FromSeconds(SelectedListItemDbMathTask.ShortestTimeSpentForExpression);
            ShortestTimeExpressionLabel.Text = "Expression: " + SelectedListItemDbMathTask.ShortestTimeExpressionString;
        }

        private void InitializeChart()
        {
            List<Entry> entries = new List<Entry>();
            for (int i = ListOfMathTasks.Count - 1; i >= 0; i--)
            {
                if (SelectedListItemDbMathTask == ListOfMathTasks[i])
                    entries.Add(new Entry(ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("000080"), Label = "Selected", ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString()});
                else
                    entries.Add(new Entry(ListOfMathTasks[i].GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("1CC9F0"), Label = "Hello", ValueLabel = ListOfMathTasks[i].GetEfficiencyParameterString()});
            }
            if (dbMathTaskToSave != null)
                entries.Add(new Entry(dbMathTaskToSave.GetEfficiencyParameterValue()) { Color = SkiaSharp.SKColor.Parse("FF1493"), Label = "Current", ValueLabel = dbMathTaskToSave.GetEfficiencyParameterString()});
            LineChart1.Chart = new LineChart() { Entries = entries };
        }

        private void GetMathTasksFromDb()
        {
            DbMathTask[] dbMathTasksArray;
            if (SelectedListItemDbMathTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    dbMathTasksArray = db.mathTasks.Where(t => t.TimeOptions == SelectedListItemDbMathTask.TimeOptions && t.TaskType == SelectedListItemDbMathTask.TaskType && t.MinValue == SelectedListItemDbMathTask.MinValue && t.MaxValue == SelectedListItemDbMathTask.MaxValue).OrderByDescending(t => t.Id).Skip(AmountOfData * LoadMoreCounter).Take(AmountOfData).ToArray();
                }
                LoadMoreCounter += 1;
                ListOfMathTasks.AddRange(dbMathTasksArray);
            }         
        }

        private void FillListView()
        {
            SimilarTasksListView.ItemsSource = null;

            DbMathTaskListItems = new List<DbMathTaskListItem>();

            for (int i = 0; i < ListOfMathTasks.Count; i++)
            {
                DbMathTaskListItems.Add(new DbMathTaskListItem(ListOfMathTasks[i]));
            }
            SimilarTasksListView.HeightRequest = ListOfMathTasks.Count * 50;
            SimilarTasksListView.ItemsSource = DbMathTaskListItems;
        }

        private void ShowHideDetailedTaskOptionsButtonClicked(object sender, EventArgs e)
        {
            if(DetailedTaskOptions.IsVisible == false)
            {
                if (SelectedListItemDbMathTask != null)
                {
                    DetailedTaskOptions.IsVisible = true;
                    DetailedTaskOptions.ForceLayout();
                    FillView();
                    ShowHideDetailedTaskOptions.Text = "Hide Task Options";
                }
            }
            else
            {
                DetailedTaskOptions.IsVisible = false;
                DetailedTaskOptions.ForceLayout();
                ShowHideDetailedTaskOptions.Text = "Show Task Options";
            }
        }

        private void SaveResultsButtonClicked(object sender, EventArgs e)
        {
            using (var db = new ApplicationContext("mental.db"))
            {
                db.mathTasks.Add(dbMathTaskToSave);
                db.SaveChanges();
            }
            dbMathTaskToSave = null;
            LoadMoreCounter = 0;
            SaveResultButton.IsVisible = false;
            ListOfMathTasks.Clear();
            GetMathTasksFromDb();
            SelectedListItemDbMathTask = ListOfMathTasks[0];         
            FillListView();
            InitializeChart();
        }

        private void LoadMoreButtonClicked(object sender, EventArgs e)
        {
            GetMathTasksFromDb();
            FillListView();
            InitializeChart();
        }

        private async void GeneralStatisticsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GeneralStatisticsPage());
        }

        private void ListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            DbMathTaskListItem taskListItem = (DbMathTaskListItem)e.Item;
            SelectedListItemDbMathTask = taskListItem.dbMathTask;
            FillView();
            InitializeChart();
        }

        private void ClearRecordsClicked(object sender, EventArgs e)
        {
            if (SelectedListItemDbMathTask != null)
            {
                using (var db = new ApplicationContext("mental.db"))
                {
                    DbMathTask[] mathTasksToDelete = db.mathTasks.Where(t => t.TimeOptions == SelectedListItemDbMathTask.TimeOptions && t.TaskType == SelectedListItemDbMathTask.TaskType && t.MinValue == SelectedListItemDbMathTask.MinValue && t.MaxValue == SelectedListItemDbMathTask.MaxValue).ToArray();
                    db.mathTasks.RemoveRange(mathTasksToDelete);
                    db.SaveChanges();
                }
            }

            dbMathTaskToSave = null;
            SelectedListItemDbMathTask = null;

            LoadMoreCounter = 0;
            SaveResultButton.IsVisible = false;
            ListOfMathTasks.Clear();
            
            FillListView();
            InitializeChart();
        }
    }
}