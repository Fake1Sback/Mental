using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models.DbModels;
using Mental.Models;
using System.Collections.Generic;
using Mental.Views;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        private async void LoadLatestMathTaskOptions()
        {
            DbMathTaskOptions dbMathTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbMathTaskOptions = await db.FavouriteMathTaskOptions.Where(o => o.IsLatestTaskOption == true).FirstOrDefaultAsync();
            }

            if (dbMathTaskOptions != null)
            {
                StoredMathTaskOptions = dbMathTaskOptions.ToMathTaskOptions();                     
            }
        }

        public async void SaveLatestMathTaskOptions(MathTasksOptions mathTasksOptions)
        {
            StoredMathTaskOptions = mathTasksOptions;
            DbMathTaskOptions LatestDbMathTaskOptions = mathTasksOptions.ToDbMathTaskOptions();
            LatestDbMathTaskOptions.IsLatestTaskOption = true;

            using (var db = new ApplicationContext("mental.db"))
            {
                DbMathTaskOptions optionToDelete = await db.FavouriteMathTaskOptions.Where(o => o.IsLatestTaskOption == true).FirstOrDefaultAsync();
                if (optionToDelete != null)
                    db.FavouriteMathTaskOptions.Remove(optionToDelete);
                await db.FavouriteMathTaskOptions.AddAsync(LatestDbMathTaskOptions);
                await db.SaveChangesAsync();
            }
        }

        private async void LoadLatestSchulteTableTaskOptions()
        {
            DbSchulteTableTaskOptions dbSchulteTableTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbSchulteTableTaskOptions = await db.FavouriteSchulteTableTaskOptions.Where(o => o.IsLatestTaskOption == true).FirstOrDefaultAsync();
            }

            if (dbSchulteTableTaskOptions != null)
            {
                StoredSchulteTableTaskOptions = dbSchulteTableTaskOptions.ToSchulteTableTaskOptions();             
            }
        }

        public async void SaveLatestSchulteTableTaskOptions(SchulteTableTaskOptions _schulteTableTaskOptions)
        {
            StoredSchulteTableTaskOptions = _schulteTableTaskOptions;
            DbSchulteTableTaskOptions latestDbSchulteTableTaskOptions = _schulteTableTaskOptions.ToDbSchulteTableTaskOptions();
            latestDbSchulteTableTaskOptions.IsLatestTaskOption = true;
         
            using (var db = new ApplicationContext("mental.db"))
            {
                DbSchulteTableTaskOptions optionToDelete = await db.FavouriteSchulteTableTaskOptions.Where(o => o.IsLatestTaskOption == true).FirstOrDefaultAsync();
                if (optionToDelete != null)
                    db.FavouriteSchulteTableTaskOptions.Remove(optionToDelete);
                await db.FavouriteSchulteTableTaskOptions.AddAsync(latestDbSchulteTableTaskOptions);
                await db.SaveChangesAsync();
            }
        }

        public async void LoadLatestStroopTaskOptions()
        {
            DbStroopTaskOptions dbStroopTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                dbStroopTaskOptions = await db.FavouriteStroopTaskOptions.Where(o => o.IsLatestTaskOption == true).FirstOrDefaultAsync();
            }

            if (dbStroopTaskOptions != null)
            {
                StoredStroopTaskOptions = dbStroopTaskOptions.ToStroopTaskOptions();
            }
        }

        public async void SaveLatestStroopTaskOptions(StroopTaskOptions _stroopTaskOptions)
        {
            StoredStroopTaskOptions = _stroopTaskOptions;
            DbStroopTaskOptions latestDbStroopTaskOptions = _stroopTaskOptions.ToDbStroopTaskOptions();
            latestDbStroopTaskOptions.IsLatestTaskOption = true;

            using (var db = new ApplicationContext("mental.db"))
            {
                DbStroopTaskOptions optionToDelete = await db.FavouriteStroopTaskOptions.Where(o => o.IsLatestTaskOption == true).FirstOrDefaultAsync();
                if (optionToDelete != null)
                    db.FavouriteStroopTaskOptions.Remove(optionToDelete);
                await db.FavouriteStroopTaskOptions.AddAsync(latestDbStroopTaskOptions);
                await db.SaveChangesAsync();
            }
        }

        public App()
        {
            InitializeComponent();
            using (var a = new ApplicationContext("mental.db"))
            {
               // a.Database.EnsureDeleted();
                a.Database.EnsureCreated();
            }

            if (StoredMathTaskOptions == null)
                LoadLatestMathTaskOptions();

            if (StoredSchulteTableTaskOptions == null)
                LoadLatestSchulteTableTaskOptions();

            if (StoredStroopTaskOptions == null)
                LoadLatestStroopTaskOptions();

            MainPage = new NavigationPage(new StartingPage()) { BarBackgroundColor = Color.FromHex("#6699ff") };
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
            LoadLatestSchulteTableTaskOptions();
            LoadLatestStroopTaskOptions();
        }
    }
}
