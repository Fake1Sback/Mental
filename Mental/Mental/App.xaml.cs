using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models.DbModels;
using Mental.Models;
using System.Collections.Generic;
using Mental.Views;
using System.Linq;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
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
                StoredSchulteTableTaskOptions = new  SchulteTableTaskOptions
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

                //StoredMathTaskOptions = new MathTasksOptions() { Operations = new List<string>() };
                
                //if (dbMathTaskOptions.Operations.Contains("+"))
                //    StoredMathTaskOptions.Operations.Add("+");
                //if (dbMathTaskOptions.Operations.Contains("-"))
                //    StoredMathTaskOptions.Operations.Add("-");
                //if (dbMathTaskOptions.Operations.Contains("*"))
                //    StoredMathTaskOptions.Operations.Add("*");
                //if (dbMathTaskOptions.Operations.Contains("/"))
                //    StoredMathTaskOptions.Operations.Add("/");

                //StoredMathTaskOptions.TaskType = (TaskType)dbMathTaskOptions.TaskType;

                //StoredMathTaskOptions.TaskTimeOptions = new TaskTimeOptionsContainer
                //{
                //    CurrentTimeOption = (TimeOptions)dbMathTaskOptions.TimeOptions,
                //    AmountOfMinutes = dbMathTaskOptions.AmountOfMinutes,
                //    AmountOfTasks = dbMathTaskOptions.AmountOfTasks,
                //    AmountOfSecondsForAnswer = dbMathTaskOptions.AmountOfSecondsForAnswer
                //};

                //StoredMathTaskOptions.IsRestrictionsActivated = dbMathTaskOptions.IsRestrictionActivated;
                //StoredMathTaskOptions.restrictions.restrictions = TaskRestrictions.GetTaskRestrictionFromString(dbMathTaskOptions.RestrictionsString);

                //StoredMathTaskOptions.IsIntegerNumbers = dbMathTaskOptions.IsIntegerNumbers;
                //StoredMathTaskOptions.DigitsAfterDotSign = dbMathTaskOptions.DigitsAfterDotSign;

                //StoredMathTaskOptions.MaxValue = dbMathTaskOptions.MaxValue;
                //StoredMathTaskOptions.MinValue = dbMathTaskOptions.MinValue;

                //StoredMathTaskOptions.IsChainLengthFixed = dbMathTaskOptions.IsChainLengthFixed;
                //StoredMathTaskOptions.MaxChainLength = dbMathTaskOptions.MaxChainLength;               
            }
        }

        public void SaveLatestMathTaskOptions(MathTasksOptions mathTasksOptions)
        {
            StoredMathTaskOptions = mathTasksOptions;
            DbMathTaskOptions dbMathTaskOptions = mathTasksOptions.ToDbMathTaskOptions();
            
            //string operations = string.Empty;
            //for (int i = 0; i < mathTasksOptions.Operations.Count; i++)
            //{
            //    operations += mathTasksOptions.Operations[i];
            //}

            //dbMathTaskOptions.Operations = operations;

            //dbMathTaskOptions.IsRestrictionActivated = mathTasksOptions.IsRestrictionsActivated;
            //dbMathTaskOptions.RestrictionsString = TaskRestrictions.GetTaskRestrictionsString(mathTasksOptions.restrictions.restrictions);

            //dbMathTaskOptions.IsIntegerNumbers = mathTasksOptions.IsIntegerNumbers;
            //dbMathTaskOptions.DigitsAfterDotSign = mathTasksOptions.DigitsAfterDotSign;

            //dbMathTaskOptions.MinValue = mathTasksOptions.MinValue;
            //dbMathTaskOptions.MaxValue = mathTasksOptions.MaxValue;

            //dbMathTaskOptions.IsChainLengthFixed = mathTasksOptions.IsChainLengthFixed;
            //dbMathTaskOptions.MaxChainLength = mathTasksOptions.MaxChainLength;

            //dbMathTaskOptions.AmountOfTasks = mathTasksOptions.TaskTimeOptions.AmountOfTasks;
            //dbMathTaskOptions.AmountOfMinutes = mathTasksOptions.TaskTimeOptions.AmountOfMinutes;
            //dbMathTaskOptions.AmountOfSecondsForAnswer = mathTasksOptions.TaskTimeOptions.AmountOfSecondsForAnswer;

            //dbMathTaskOptions.TaskType = (byte)mathTasksOptions.TaskType;
            //dbMathTaskOptions.TimeOptions = (byte)mathTasksOptions.TaskTimeOptions.CurrentTimeOption;

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

            if(dbSchulteTableTaskOptions != null)
            {
                StoredSchulteTableTaskOptions = dbSchulteTableTaskOptions.ToSchulteTableTaskOptions();

                //StoredSchulteTableTaskOptions = new SchulteTableTaskOptions()
                //{
                //    GridSize = dbSchulteTableTaskOptions.GridSize,
                //    IsEasyModeActivated = dbSchulteTableTaskOptions.IsEasyModeActivated,
                //    TaskTimeOptions = new TaskTimeOptionsContainer
                //    {
                //        CurrentTimeOption = (TimeOptions)dbSchulteTableTaskOptions.TimeOptions,
                //        AmountOfMinutes = dbSchulteTableTaskOptions.AmountOfMinutes,
                //        AmountOfTasks = dbSchulteTableTaskOptions.AmountOfTasks,
                //        AmountOfSecondsForAnswer = dbSchulteTableTaskOptions.AmountOfSecondsForAnswer
                //    }
                //};               
            }
        }

        public void SaveLatestSchulteTableTaskOptions(SchulteTableTaskOptions _schulteTableTaskOptions)
        {
            StoredSchulteTableTaskOptions = _schulteTableTaskOptions;
            DbSchulteTableTaskOptions dbSchulteTableTaskOptions = _schulteTableTaskOptions.ToDbSchulteTableTaskOptions();
           
            //dbSchulteTableTaskOptions.GridSize = _schulteTableTaskOptions.GridSize;
            //dbSchulteTableTaskOptions.IsEasyModeActivated = _schulteTableTaskOptions.IsEasyModeActivated;
            //dbSchulteTableTaskOptions.TimeOptions = (byte)_schulteTableTaskOptions.TaskTimeOptions.CurrentTimeOption;
            //dbSchulteTableTaskOptions.AmountOfMinutes = _schulteTableTaskOptions.TaskTimeOptions.AmountOfMinutes;
            //dbSchulteTableTaskOptions.AmountOfTasks = _schulteTableTaskOptions.TaskTimeOptions.AmountOfTasks;
            //dbSchulteTableTaskOptions.AmountOfSecondsForAnswer = _schulteTableTaskOptions.TaskTimeOptions.AmountOfSecondsForAnswer;

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

                //StoredStroopTaskOptions = new StroopTaskOptions();
                //StoredStroopTaskOptions.StroopTaskType = (StroopTaskType)dbStroopTaskOptions.StroopTaskType;
                //StoredStroopTaskOptions.ButtonsAmount = dbStroopTaskOptions.ButtonsAmount;
                //StoredStroopTaskOptions.TaskTimeOptionsContainer = new TaskTimeOptionsContainer
                //{
                //    AmountOfMinutes = dbStroopTaskOptions.AmountOfMinutes,
                //    AmountOfSecondsForAnswer = dbStroopTaskOptions.AmountOfSecondsForAnswer,
                //    AmountOfTasks = dbStroopTaskOptions.AmountOfTasks,
                //    CurrentTimeOption = (TimeOptions)dbStroopTaskOptions.TimeOptions
                //};
            }
        }

        public void SaveLatestStroopTaskOptions(StroopTaskOptions _stroopTaskOptions)
        {
            StoredStroopTaskOptions = _stroopTaskOptions;
            DbStroopTaskOptions dbStroopTaskOptions = _stroopTaskOptions.ToDbStroopTaskOptions();
            

            //dbStroopTaskOptions.TimeOptions = (byte)_stroopTaskOptions.TaskTimeOptionsContainer.CurrentTimeOption;
            //dbStroopTaskOptions.AmountOfMinutes = _stroopTaskOptions.TaskTimeOptionsContainer.AmountOfMinutes;
            //dbStroopTaskOptions.AmountOfTasks = _stroopTaskOptions.TaskTimeOptionsContainer.AmountOfTasks;
            //dbStroopTaskOptions.AmountOfSecondsForAnswer = _stroopTaskOptions.TaskTimeOptionsContainer.AmountOfSecondsForAnswer;
            //dbStroopTaskOptions.ButtonsAmount = _stroopTaskOptions.ButtonsAmount;
            //dbStroopTaskOptions.StroopTaskType = (byte)_stroopTaskOptions.StroopTaskType;

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


            MainPage = new NavigationPage(new GeneralStatisticsPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };

           // MainPage = new NavigationPage(new SimilarTasksStatisticsPage(new DbMathTask { AmountOfCorrectAnswers = 23, AmountOfWrongAnswers = 3, DigitsAfterDotSing = 1, IsChainLengthFixed = true, IsInteger = true, IsRestrictionActivated = false, LongestTimeExpressionString = "4 +3 ", LongestTimeSpentForExpression = 4, MaxChainLength = 3, MaxValue = 100, MinValue = 0, Operations = "+-*/", ShortestTimeExpressionString = "3 + 1", ShortestTimeSpentForExpression = 3, TaskType = (byte)TaskType.CountResult, TimeOptions = (byte)TimeOptions.CountdownTimer, TimeParameter = 3, RestrictionsString = "1111111111111111111111111", TaskDateTime = DateTime.Now, TaskComplexityParameter = 5 }, true)) { BarBackgroundColor = Color.FromHex("#6699ff") };

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
