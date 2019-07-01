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

        public MathTasksOptions GetStoredMathTaskOptions()
        {
            if (StoredMathTaskOptions != null)
                return StoredMathTaskOptions;
            else
            {
                StoredMathTaskOptions = new MathTasksOptions
                {
                    TaskType = TaskType.CountResult,
                    TimeOptions = TimeOptions.CountdownTimer,
                    Operations = new List<string> { "+" },
                    IsRestrictionsActivated = false,
                    restrictions = new TaskRestrictions(),
                    IsIntegerNumbers = true,
                    DigitsAfterDotSign = 1,
                    MinValue = 0,
                    MaxValue = 10,
                    IsChainLengthFixed = true,
                    MaxChainLength = 2,
                    AmountOfMinutes = 1,
                    AmountOfTasks = 1,
                    AmountOfSecondsForAnswer = 3
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
                    TimeOptions = TimeOptions.CountdownTimer,
                    AmountOfMinutes = 1,
                    AmountOfSecondsForAnswer = 10,
                    AmountOfTasks = 9
                };
                return StoredSchulteTableTaskOptions;
            }
        }
      
        private void LoadLatestMathTaskOptions()
        {
            DbMathTaskOptions dbMathTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTaskOptions = db.LastMathTaskOptions.FirstOrDefault();
               // string str = db.LastMathTaskOptions.ToString();
            }

            if (dbMathTaskOptions != null)
            {
                StoredMathTaskOptions = new MathTasksOptions() { Operations = new List<string>() };
                
                if (dbMathTaskOptions.Operations.Contains("+"))
                    StoredMathTaskOptions.Operations.Add("+");
                if (dbMathTaskOptions.Operations.Contains("-"))
                    StoredMathTaskOptions.Operations.Add("-");
                if (dbMathTaskOptions.Operations.Contains("*"))
                    StoredMathTaskOptions.Operations.Add("*");
                if (dbMathTaskOptions.Operations.Contains("/"))
                    StoredMathTaskOptions.Operations.Add("/");

                StoredMathTaskOptions.IsRestrictionsActivated = dbMathTaskOptions.IsRestrictionActivated;
                StoredMathTaskOptions.restrictions.restrictions = TaskRestrictions.GetTaskRestrictionFromString(dbMathTaskOptions.RestrictionsString);

                StoredMathTaskOptions.IsIntegerNumbers = dbMathTaskOptions.IsIntegerNumbers;
                StoredMathTaskOptions.DigitsAfterDotSign = dbMathTaskOptions.DigitsAfterDotSign;

                StoredMathTaskOptions.MaxValue = dbMathTaskOptions.MaxValue;
                StoredMathTaskOptions.MinValue = dbMathTaskOptions.MinValue;

                StoredMathTaskOptions.IsChainLengthFixed = dbMathTaskOptions.IsChainLengthFixed;
                StoredMathTaskOptions.MaxChainLength = dbMathTaskOptions.MaxChainLength;

                StoredMathTaskOptions.AmountOfMinutes = dbMathTaskOptions.AmountOfMinutes;
                StoredMathTaskOptions.AmountOfSecondsForAnswer = dbMathTaskOptions.AmountOfSecondsForAnswer;
                StoredMathTaskOptions.AmountOfTasks = dbMathTaskOptions.AmountOfTasks;

                StoredMathTaskOptions.TaskType = (TaskType)dbMathTaskOptions.TaskType;
                StoredMathTaskOptions.TimeOptions = (TimeOptions)dbMathTaskOptions.TimeOptions;
            }
        }

        public void SaveLatestMathTaskOptions(MathTasksOptions mathTasksOptions)
        {
            DbMathTaskOptions dbMathTaskOptions = new DbMathTaskOptions();
            StoredMathTaskOptions = mathTasksOptions;

            string operations = string.Empty;
            for (int i = 0; i < mathTasksOptions.Operations.Count; i++)
            {
                operations += mathTasksOptions.Operations[i];
            }

            dbMathTaskOptions.Operations = operations;

            dbMathTaskOptions.IsRestrictionActivated = mathTasksOptions.IsRestrictionsActivated;
            dbMathTaskOptions.RestrictionsString = TaskRestrictions.GetTaskRestrictionsString(mathTasksOptions.restrictions.restrictions);

            dbMathTaskOptions.IsIntegerNumbers = mathTasksOptions.IsIntegerNumbers;
            dbMathTaskOptions.DigitsAfterDotSign = mathTasksOptions.DigitsAfterDotSign;

            dbMathTaskOptions.MinValue = mathTasksOptions.MinValue;
            dbMathTaskOptions.MaxValue = mathTasksOptions.MaxValue;

            dbMathTaskOptions.IsChainLengthFixed = mathTasksOptions.IsChainLengthFixed;
            dbMathTaskOptions.MaxChainLength = mathTasksOptions.MaxChainLength;

            dbMathTaskOptions.AmountOfTasks = mathTasksOptions.AmountOfTasks;
            dbMathTaskOptions.AmountOfMinutes = mathTasksOptions.AmountOfMinutes;
            dbMathTaskOptions.AmountOfSecondsForAnswer = mathTasksOptions.AmountOfSecondsForAnswer;

            dbMathTaskOptions.TaskType = (byte)mathTasksOptions.TaskType;
            dbMathTaskOptions.TimeOptions = (byte)mathTasksOptions.TimeOptions;

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
                StoredSchulteTableTaskOptions = new SchulteTableTaskOptions()
                {
                    GridSize = dbSchulteTableTaskOptions.GridSize,
                    IsEasyModeActivated = dbSchulteTableTaskOptions.IsEasyModeActivated,
                    TimeOptions = (TimeOptions)dbSchulteTableTaskOptions.TimeOptions,
                    AmountOfMinutes = dbSchulteTableTaskOptions.AmountOfMinutes,
                    AmountOfTasks = dbSchulteTableTaskOptions.AmountOfTasks,
                    AmountOfSecondsForAnswer = dbSchulteTableTaskOptions.AmountOfSecondsForAnswer
                };               
            }
        }

        public void SaveLatestSchulteTableTaskOptions(SchulteTableTaskOptions _schulteTableTaskOptions)
        {
            DbSchulteTableTaskOptions dbSchulteTableTaskOptions = new DbSchulteTableTaskOptions();
            StoredSchulteTableTaskOptions = _schulteTableTaskOptions;

            dbSchulteTableTaskOptions.GridSize = _schulteTableTaskOptions.GridSize;
            dbSchulteTableTaskOptions.IsEasyModeActivated = _schulteTableTaskOptions.IsEasyModeActivated;
            dbSchulteTableTaskOptions.TimeOptions = (byte)_schulteTableTaskOptions.TimeOptions;
            dbSchulteTableTaskOptions.AmountOfMinutes = _schulteTableTaskOptions.AmountOfMinutes;
            dbSchulteTableTaskOptions.AmountOfTasks = _schulteTableTaskOptions.AmountOfTasks;
            dbSchulteTableTaskOptions.AmountOfSecondsForAnswer = _schulteTableTaskOptions.AmountOfSecondsForAnswer;

            using (var db = new ApplicationContext("mental.db"))
            {
                DbSchulteTableTaskOptions dbSchulteTableTaskOptionsToDelete = db.LastSchulteTableTaskOptions.FirstOrDefault();
                if (dbSchulteTableTaskOptionsToDelete != null)
                    db.LastSchulteTableTaskOptions.Remove(dbSchulteTableTaskOptionsToDelete);
                db.LastSchulteTableTaskOptions.Add(dbSchulteTableTaskOptions);
                db.SaveChanges();
            }          
        }

        public App()
        {
            InitializeComponent();
            using (var a = new ApplicationContext("mental.db"))
            {
                //   a.Database.EnsureDeleted();
                a.Database.EnsureCreated();
            }

            if (StoredMathTaskOptions == null)
                LoadLatestMathTaskOptions();

            if (StoredSchulteTableTaskOptions == null)
                LoadLatestSchulteTableTaskOptions();

            //   MainPage = new NavigationPage(new MathTasksOptionsPage());
            MainPage = new NavigationPage(new SchulteTableTaskOptionsPage());
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
