using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models.DbModels;
using Mental.Models;
using Microcharts;
using Microcharts.Forms;
using Entry = Microcharts.Entry;

namespace Mental
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GeneralStatisticsPage : ContentPage
	{
        private int GeneralAmountOfRecords;
        private int LoadCounter = 0;
        private int AmountOfDataInListView = 5;

        public List<DbMathTaskListItem> mathTaskListItems { get; set; }

        public GeneralStatisticsPage()
        {
            InitializeComponent();
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
        }

        private void InitializeTimeOptionsChart(ApplicationContext db)
        {
            // int GeneralAmountOfRecords = db.mathTasks.Count();
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

            TimeOptionsChart.Chart = new DonutChart() { Entries = entries };
        }

        private void InitializeTaskTypeOptionsChart(ApplicationContext db)
        {
           // int GeneralAmountOfRecords = db.mathTasks.Count();
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

            TaskTypeChart.Chart = new DonutChart() { Entries = entries };
        }

        private void InitializeDataTypeOptionsChart(ApplicationContext db)
        {
           // int GeneralAmountOfRecords = db.mathTasks.Count();
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

            DataTypeChart.Chart = new DonutChart() { Entries = entries };
        }

        private void InitializeSpecailModeOptionsChart(ApplicationContext db)
        {
          //  int GeneralAmountOfRecords = db.mathTasks.Count();
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

            SpecialModeChart.Chart = new DonutChart() { Entries = entries };
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

            OperationsChart.Chart = new RadialGaugeChart() { Entries = entries };
        }

        private void InitializeFixedChainLengthOptionsChart(ApplicationContext db)
        {
           // int GeneralAmountOfRecords = db.mathTasks.Count();
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

            ChainLengthChart.Chart = new DonutChart() { Entries = entries };
        }

        private void LoadMoreDbMathTaskInfo()
        {
            LatestTasksListView.ItemsSource = null;
            DbMathTask[] dbMathTasks;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTasks = db.mathTasks.OrderByDescending(t => t.Id).Skip(AmountOfDataInListView * LoadCounter).Take(AmountOfDataInListView).ToArray();
            }
            if (mathTaskListItems == null)
                mathTaskListItems = new List<DbMathTaskListItem>();
            LoadCounter += 1;
            for(int i = 0;i < dbMathTasks.Length;i++)
            {
                mathTaskListItems.Add(new DbMathTaskListItem(dbMathTasks[i]));
            }
            LatestTasksListView.HeightRequest = mathTaskListItems.Count * 50;
            LatestTasksListView.ItemsSource = mathTaskListItems;
        }

        private void LoadMoreDbInfoButtonClicked(object sender, EventArgs e)
        {
            LoadMoreDbMathTaskInfo();
        }

        private async void LatestTasksListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            DbMathTaskListItem listItem = (DbMathTaskListItem)e.Item;
            await Navigation.PushAsync(new SimilarTasksStatisticsPage(listItem.dbMathTask,false));
        }
    }
}