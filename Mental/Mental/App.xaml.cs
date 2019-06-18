using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Mental.Models.DbModels;
using Mental.Models;
using System.Collections.Generic;
using Mental.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Mental
{
    public partial class App : Application
    {
        private MathTasksOptions StoredMathTaskOptions;

        public MathTasksOptions GetStoredMathTaskOptions()
        {
            return StoredMathTaskOptions;
        }

        public void SaveMathTaskOptions(MathTasksOptions _mathTasksOption)
        {
            StoredMathTaskOptions = _mathTasksOption;

            App.Current.Properties["TaskType"] = (byte)_mathTasksOption.TaskType;
            App.Current.Properties["TimeOptions"] = (byte)_mathTasksOption.TimeOptions;

            string str = string.Empty;
            for(int i = 0;i < _mathTasksOption.Operations.Count;i++)
            {
                str += _mathTasksOption.Operations[i];
            }

            App.Current.Properties["Operations"] = str;

            App.Current.Properties["SpecialMode"] = _mathTasksOption.IsSpecialModeActivated;
            if (_mathTasksOption.IsSpecialModeActivated)
                App.Current.Properties["AmountOfXDigits"] = _mathTasksOption.AmountOfXDigits;

            App.Current.Properties["IsIntegerNumbers"] = _mathTasksOption.IsIntegerNumbers;
            if (!_mathTasksOption.IsIntegerNumbers)
                App.Current.Properties["DigitsAfterDotSign"] = _mathTasksOption.DigitsAfterDotSign;

            App.Current.Properties["MinValue"] = _mathTasksOption.MinValue;
            App.Current.Properties["MaxValue"] = _mathTasksOption.MaxValue;

            App.Current.Properties["IsChainLengthFixed"] = _mathTasksOption.IsChainLengthFixed;
            App.Current.Properties["MaxChainLength"] = _mathTasksOption.MaxChainLength;

            App.Current.Properties["AmountOfTasks"] = _mathTasksOption.AmountOfTasks;
            App.Current.Properties["AmountOfMinutes"] = _mathTasksOption.AmountOfMinutes;
        }

        public void LoadMathTaskOptions()
        {
            bool[] Flags = new bool[9];

            MathTasksOptions mathTasksOptions = new MathTasksOptions();
            object obj;

            if (App.Current.Properties.TryGetValue("TaskType",out obj))
            {
                mathTasksOptions.TaskType = (TaskType)(byte)obj;
                Flags[0] = true;
            }

            if(App.Current.Properties.TryGetValue("TimeOptions",out obj))
            {
                mathTasksOptions.TimeOptions = (TimeOptions)(byte)obj;
                Flags[1] = true;
                if (mathTasksOptions.TimeOptions == TimeOptions.CountdownTimer)
                {
                    if(App.Current.Properties.TryGetValue("AmountOfMinutes",out obj))
                    {
                        mathTasksOptions.AmountOfMinutes = (int)obj;
                    }
                }
                else
                {
                    if(App.Current.Properties.TryGetValue("AmountOfTasks",out obj))
                    {
                        mathTasksOptions.AmountOfTasks = (int)obj;
                    }
                }
            }

            List<string> operations = new List<string>();

            if (App.Current.Properties.TryGetValue("Operations", out obj))
            {
                string str = (string)obj;
                for (int i = 0; i < str.Length; i++)
                {
                    operations.Add(str.Substring(i, 1));
                }
                mathTasksOptions.Operations = operations;
                Flags[2] = true;
            }

            if (App.Current.Properties.TryGetValue("SpecialMode",out obj))
            {
                mathTasksOptions.IsSpecialModeActivated = (bool)obj;
                Flags[3] = true;

                if (mathTasksOptions.IsSpecialModeActivated)
                {
                    if(App.Current.Properties.TryGetValue("AmountOfXDigits",out obj))
                    {
                        mathTasksOptions.AmountOfXDigits = (int)obj;
                    }
                }
            }

            if(App.Current.Properties.TryGetValue("IsIntegerNumbers",out obj))
            {
                mathTasksOptions.IsIntegerNumbers = (bool)obj;
                Flags[4] = true;

                if (mathTasksOptions.IsIntegerNumbers)
                {
                    if(App.Current.Properties.TryGetValue("DigitsAfterDotSign",out obj))
                    {
                        mathTasksOptions.DigitsAfterDotSign = (int)obj;
                    }
                }
            }

            if(App.Current.Properties.TryGetValue("MinValue",out obj))
            {
                mathTasksOptions.MinValue = (int)obj;
                Flags[5] = true;
            }
            if(App.Current.Properties.TryGetValue("MaxValue",out obj))
            {
                mathTasksOptions.MaxValue = (int)obj;
                Flags[6] = true;
            }
            if(App.Current.Properties.TryGetValue("IsChainLengthFixed",out obj))
            {
                mathTasksOptions.IsChainLengthFixed = (bool)obj;
                Flags[7] = true;
            }
            if(App.Current.Properties.TryGetValue("MaxChainLength",out obj))
            {
                mathTasksOptions.MaxChainLength = (int)obj;
                Flags[8] = true;
            }

            for(int i = 0;i < Flags.Length;i++)
            {
                if (Flags[i] == false)
                {
                    StoredMathTaskOptions = new MathTasksOptions() {
                        TaskType = TaskType.CountResult,
                        TimeOptions = TimeOptions.CountdownTimer,
                        Operations = new List<string> { "+" },
                        IsSpecialModeActivated = false,
                        AmountOfXDigits = 1,
                        IsIntegerNumbers = true,
                        DigitsAfterDotSign = 1,
                        MinValue = 0,
                        MaxValue = 10,
                        IsChainLengthFixed = true,
                        MaxChainLength = 2,
                        AmountOfMinutes = 1,
                        AmountOfTasks = 1
                    };
                    return;
                }
            }

            StoredMathTaskOptions = mathTasksOptions;
        }

        public App()
        {
            InitializeComponent();
            using (var a = new ApplicationContext("mental.db"))
            {
                //a.Database.EnsureDeleted();
                a.Database.EnsureCreated();
            }

            if (StoredMathTaskOptions == null)
                LoadMathTaskOptions();

            // MainPage = new NavigationPage(new MathTasksOptionsPage());
            MainPage = new NavigationPage(new Mental.Views.MathTasksOptionsPage());
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
            LoadMathTaskOptions();
        }
    }
}
