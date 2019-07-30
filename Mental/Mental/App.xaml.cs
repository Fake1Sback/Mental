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
            LatestDbMathTaskOptions LatestdbMathTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                LatestdbMathTaskOptions = await db.LastMathTaskOptions.FirstOrDefaultAsync();
            }

            if (LatestdbMathTaskOptions != null)
            {
                StoredMathTaskOptions = LatestdbMathTaskOptions.ToMathTaskOptions();                     
            }
        }

        public async void SaveLatestMathTaskOptions(MathTasksOptions mathTasksOptions)
        {
            StoredMathTaskOptions = mathTasksOptions;
            LatestDbMathTaskOptions LatestdbMathTaskOptions = mathTasksOptions.ToLatestDbMathTaskOptions();

            using (var db = new ApplicationContext("mental.db"))
            {
                LatestDbMathTaskOptions[] optionsToDelete = await db.LastMathTaskOptions.ToArrayAsync();
                if (optionsToDelete != null)
                    db.LastMathTaskOptions.RemoveRange(optionsToDelete);
                await db.LastMathTaskOptions.AddAsync(LatestdbMathTaskOptions);
                await db.SaveChangesAsync();
            }
        }

        private async void LoadLatestSchulteTableTaskOptions()
        {
            LatestDbSchulteTableTaskOptions latestDbSchulteTableTaskOptions = new LatestDbSchulteTableTaskOptions();
            using (var db = new ApplicationContext("mental.db"))
            {
                latestDbSchulteTableTaskOptions = await db.LastSchulteTableTaskOptions.FirstOrDefaultAsync();
            }

            if (latestDbSchulteTableTaskOptions != null)
            {
                StoredSchulteTableTaskOptions = latestDbSchulteTableTaskOptions.ToSchulteTableTaskOptions();             
            }
        }

        public async void SaveLatestSchulteTableTaskOptions(SchulteTableTaskOptions _schulteTableTaskOptions)
        {
            StoredSchulteTableTaskOptions = _schulteTableTaskOptions;
            LatestDbSchulteTableTaskOptions latestDbSchulteTableTaskOptions = _schulteTableTaskOptions.ToLatestDbSchulteTaskOptions();    
         
            using (var db = new ApplicationContext("mental.db"))
            {
                LatestDbSchulteTableTaskOptions[] optionsToDelete = await db.LastSchulteTableTaskOptions.ToArrayAsync();
                if(optionsToDelete != null)
                    db.LastSchulteTableTaskOptions.RemoveRange(optionsToDelete);
                await db.LastSchulteTableTaskOptions.AddAsync(latestDbSchulteTableTaskOptions);
                await db.SaveChangesAsync();
            }
        }

        public async void LoadLatestStroopTaskOptions()
        {
            LatestDbStroopTaskOptions latestDbStroopTaskOptions;
            using (var db = new ApplicationContext("mental.db"))
            {
                latestDbStroopTaskOptions = await db.LastStroopTaskOptions.FirstOrDefaultAsync();
            }

            if (latestDbStroopTaskOptions != null)
            {
                StoredStroopTaskOptions = latestDbStroopTaskOptions.ToStroopTaskOptions();
            }
        }

        public async void SaveLatestStroopTaskOptions(StroopTaskOptions _stroopTaskOptions)
        {
            StoredStroopTaskOptions = _stroopTaskOptions;
            LatestDbStroopTaskOptions latestDbStroopTaskOptions = _stroopTaskOptions.ToLatestDbStroopTaskOptions();

            using (var db = new ApplicationContext("mental.db"))
            {
                LatestDbStroopTaskOptions[] optionsToDelete = await db.LastStroopTaskOptions.ToArrayAsync();
                if (optionsToDelete != null)
                    db.LastStroopTaskOptions.RemoveRange(optionsToDelete);
                await db.LastStroopTaskOptions.AddAsync(latestDbStroopTaskOptions);
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

            LatestDbMathTaskOptions[] LastOptions;
            DbMathTaskOptions[] FavouriteOptions;

            using (var a = new ApplicationContext("mental.db"))
            {
                LastOptions = a.LastMathTaskOptions.ToArray();
                FavouriteOptions = a.FavouriteMathTaskOptions.ToArray();
            }

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
