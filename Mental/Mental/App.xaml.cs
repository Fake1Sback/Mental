using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models.DbModels;
using Mental.Models;
using System.Collections.Generic;
using Mental.Views;
using System.Linq;

namespace Mental
{
    public partial class App : Application
    {
        private MathTasksOptions StoredMathTaskOptions;
        private SchulteTableTaskOptions StoredSchulteTableTaskOptions;
        private StroopTaskOptions StoredStroopTaskOptions;

        public MathTasksOptions GetStoredMathTaskOptions()
        {
            if (StoredMathTaskOptions != null)
                return StoredMathTaskOptions;
            else
            {
                StoredMathTaskOptions = new MathTasksOptions
                {
                    TaskType = TaskType.CountResult,
                    Operations = new List<string> { "+" },
                    IsRestrictionsActivated = false,
                    restrictions = new TaskRestrictions(),
                    IsIntegerNumbers = true,
                    DigitsAfterDotSign = 1,
                    MinValue = 0,
                    MaxValue = 10,
                    IsChainLengthFixed = true,
                    MaxChainLength = 2,
                    TaskTimeOptions = new TaskTimeOptionsContainer
                    {
                        CurrentTimeOption = TimeOptions.CountdownTimer,
                        AmountOfMinutes = 1,
                        AmountOfTasks = 1,
                        AmountOfSecondsForAnswer = 3
                    }
                };
                return StoredMathTaskOptions;
            }
        }
        public SchulteTableTaskOptions GetStoredSchulteTableTaskOptions()
        {
            if (StoredSchulteTableTaskOptions != null)
                return StoredSchulteTableTaskOptions;
            else
            {
                StoredSchulteTableTaskOptions = new SchulteTableTaskOptions
                {
                    GridSize = 3,
                    IsEasyModeActivated = false,
                    TaskTimeOptions = new TaskTimeOptionsContainer
                    {
                        CurrentTimeOption = TimeOptions.CountdownTimer,
                        AmountOfMinutes = 1,
                        AmountOfSecondsForAnswer = 10,
                        AmountOfTasks = 9
                    }
                };
                return StoredSchulteTableTaskOptions;
            }
        }
        public StroopTaskOptions GetStoredStroopTaskOptions()
        {
            if (StoredStroopTaskOptions != null)
                return StoredStroopTaskOptions;
            else
            {
                StoredStroopTaskOptions = new StroopTaskOptions
                {
                    ButtonsAmount = 2,
                    StroopTaskType = StroopTaskType.FindOneCorrect,
                    TaskTimeOptionsContainer = new TaskTimeOptionsContainer
                    {
                        CurrentTimeOption = TimeOptions.CountdownTimer,
                        AmountOfMinutes = 1,
                        AmountOfTasks = 1,
                        AmountOfSecondsForAnswer = 10
                    }
                };
                return StoredStroopTaskOptions;
            }
        }

        private void LoadLatestMathTaskOptions()
        {
            DbMathTaskOptions dbMathTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTaskOptions = db.LastMathTaskOptions.FirstOrDefault();
            }

            if (dbMathTaskOptions != null)
            {
                StoredMathTaskOptions = dbMathTaskOptions.ToMathTaskOptions();                     
            }
        }

        public void SaveLatestMathTaskOptions(MathTasksOptions mathTasksOptions)
        {
            StoredMathTaskOptions = mathTasksOptions;
            DbMathTaskOptions dbMathTaskOptions = mathTasksOptions.ToDbMathTaskOptions();

            using (var db = new ApplicationContext("mental.db"))
            {
                DbMathTaskOptions optionsToDelete = db.LastMathTaskOptions.FirstOrDefault();
                if (optionsToDelete != null)
                    db.LastMathTaskOptions.Remove(optionsToDelete);
                db.LastMathTaskOptions.Add(dbMathTaskOptions);
                db.SaveChanges();
            }
        }

        private void LoadLatestSchulteTableTaskOptions()
        {
            DbSchulteTableTaskOptions dbSchulteTableTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbSchulteTableTaskOptions = db.LastSchulteTableTaskOptions.FirstOrDefault();
            }

            if (dbSchulteTableTaskOptions != null)
            {
                StoredSchulteTableTaskOptions = dbSchulteTableTaskOptions.ToSchulteTableTaskOptions();             
            }
        }

        public void SaveLatestSchulteTableTaskOptions(SchulteTableTaskOptions _schulteTableTaskOptions)
        {
            StoredSchulteTableTaskOptions = _schulteTableTaskOptions;
            DbSchulteTableTaskOptions dbSchulteTableTaskOptions = _schulteTableTaskOptions.ToDbSchulteTableTaskOptions();
         
            using (var db = new ApplicationContext("mental.db"))
            {
                DbSchulteTableTaskOptions dbSchulteTableTaskOptionsToDelete = db.LastSchulteTableTaskOptions.FirstOrDefault();
                if (dbSchulteTableTaskOptionsToDelete != null)
                    db.LastSchulteTableTaskOptions.Remove(dbSchulteTableTaskOptionsToDelete);
                db.LastSchulteTableTaskOptions.Add(dbSchulteTableTaskOptions);
                db.SaveChanges();
            }
        }

        public void LoadLatestStroopTaskOptions()
        {
            DbStroopTaskOptions dbStroopTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbStroopTaskOptions = db.LastStroopTaskOptions.FirstOrDefault();
            }

            if (dbStroopTaskOptions != null)
            {
                StoredStroopTaskOptions = dbStroopTaskOptions.ToStroopTaskOptions();
            }
        }

        public void SaveLatestStroopTaskOptions(StroopTaskOptions _stroopTaskOptions)
        {
            StoredStroopTaskOptions = _stroopTaskOptions;
            DbStroopTaskOptions dbStroopTaskOptions = _stroopTaskOptions.ToDbStroopTaskOptions();

            using (var db = new ApplicationContext("mental.db"))
            {
                DbStroopTaskOptions dbStroopTaskOptionsToDelete = db.LastStroopTaskOptions.FirstOrDefault();
                if (dbStroopTaskOptionsToDelete != null)
                    db.LastStroopTaskOptions.Remove(dbStroopTaskOptionsToDelete);
                db.LastStroopTaskOptions.Add(dbStroopTaskOptions);
                db.SaveChanges();
            }
        }

        public App()
        {
            InitializeComponent();
            using (var a = new ApplicationContext("mental.db"))
            {
                //a.Database.EnsureDeleted();
                a.Database.EnsureCreated();
            }

            //  if (StoredMathTaskOptions == null)
            //    LoadLatestMathTaskOptions();

            //            if (StoredSchulteTableTaskOptions == null)
            //              LoadLatestSchulteTableTaskOptions();

            //        if (StoredStroopTaskOptions == null)
            //          LoadLatestStroopTaskOptions();


            //  MainPage = new NavigationPage(new GeneralStatisticsPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };
            //   MainPage = new NavigationPage(new StroopTaskSimilarStatisticsPage(new DbStroopTask() { AmountOfButtons = 8, AmountOfCorrectAnswers = 20, AmountOfWrongAnswers = 5, StroopTaskOption = (byte)StroopTaskType.TrueOrFalse, TaskComplexityParameter = 25, TimeOption = (byte)TimeOptions.FixedAmountOfOperations, TaskDateTime = DateTime.Now, TimeParameter = 15000 }, true));
            // MainPage = new NavigationPage(new SchulteTableTasksGeneralStatisticsPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };

            MainPage = new NavigationPage(new StartingPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };

            // MainPage = new NavigationPage(new StroopTaskGeneralStatisticsPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };
            // MainPage = new NavigationPage(new SimilarSchulteTableTasksStatisticsPage(new DbSchulteTableTask() { AmountOfCorrectAnswers = 49, AmountOfWrongAnswers = 0, GridSize = 7, IsEasyModeActivated = true, TimeOption = (byte)TimeOptions.FixedAmountOfOperations, TimeParameter = 20000, TaskDateTime = DateTime.Now, TaskComplexityParameter = 49 }, true));

            //MainPage = new NavigationPage(new SimilarTasksStatisticsPage(new DbMathTask { AmountOfCorrectAnswers = 10, AmountOfWrongAnswers = 2, DigitsAfterDotSing = 1, IsChainLengthFixed = true, IsInteger = true, IsRestrictionActivated = false, MaxChainLength = 2, MaxValue = 100, MinValue = 0, Operations = "+-*/", TaskType = (byte)TaskType.CountResult, TimeOptions = (byte)TimeOptions.CountdownTimer, TimeParameter = 3, RestrictionsString = "1111111111111111111111111", TaskDateTime = DateTime.Now, TaskComplexityParameter = 5 }, true)) { BarBackgroundColor = Color.FromHex("#6699ff") };
            //MainPage = new NavigationPage(new SimilarTasksStatisticsPage(new DbMathTask { AmountOfCorrectAnswers = 15, AmountOfWrongAnswers = 0, DigitsAfterDotSing = 1, IsChainLengthFixed = false, IsInteger = true, IsRestrictionActivated = false,  MaxChainLength = 3, MaxValue = 100, MinValue = 0, Operations = "+-*/", TaskType = (byte)TaskType.CountResult, TimeOptions = (byte)TimeOptions.CountdownTimer, TimeParameter = 3, RestrictionsString = "1111111111111111111111111", TaskDateTime = DateTime.Now, TaskComplexityParameter = 5 }, true)) { BarBackgroundColor = Color.FromHex("#6699ff") };

            //MainPage = new NavigationPage(new MathTasksOptionsPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };
            //MainPage = new NavigationPage(new SchulteTableTaskOptionsPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };
            //MainPage = new NavigationPage(new StroopTaskOptionsPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };
            //MainPage = new NavigationPage(new StroopTaskPage(GetStoredStroopTaskOptions(),new CountdownTimeOption(new TaskTimeOptionsContainer() { AmountOfMinutes = 2})));
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            LoadLatestMathTaskOptions();
        }
    }
}
