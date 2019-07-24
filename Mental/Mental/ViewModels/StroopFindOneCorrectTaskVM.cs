using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;
using Mental.Views;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public class StroopFindOneCorrectTaskVM : BaseStroopTaskVM
    {
        private Random random;

        public StroopFindOneCorrectTaskVM(INavigation _navigation, StroopTaskOptions _stroopTaskOptions, ITimeOption _timeOption,StroopTaskPage _stroopTaskPage) : base(_navigation, _stroopTaskOptions, _timeOption,_stroopTaskPage)
        {
            QuestionLabelVisibility = false;
            YesNoLayoutVisibility = false;
            random = new Random();
            ColorButtonClickedCommand = new Command(ColorButtonClicked);
            GenerateTask();
        }

        public void ColorButtonClicked(object obj)
        {
            Button button = obj as Button;
            Color TextColor = button.TextColor;
            string ButtonText = button.Text;
            bool Result;

            int Index = Array.FindIndex(colorsStrings, s => s == ButtonText);
            int Index2 = Array.FindIndex(colors, c => c == TextColor);
            Result = Index == Index2;

            if (Result)
                AmountOfCorrectAnswers += 1;
            else
                AmountOfWrongAnswers += 1;

            if (timeOption.CanExecuteOperation(Result))
            {              
                GenerateTask();
            }
            else
            {
                stroopTaskPage.HideTaskFrame();
                VMTimerBlocker = true;
            }
        }

        protected override void GenerateTask()
        {
            int EqualityValue = random.Next(0, colors.Length);
            int EqualityButtonValue = random.Next(0, stroopTaskOptions.ButtonsAmount);

            colorsList = new List<Color>(colors);
            colorsStringsList = new List<string>(colorsStrings);

            bool First = true;

            for (int i = 0; i < stroopTaskOptions.ButtonsAmount; i++)
            {
                if (First)
                {
                    ColorButtonText[EqualityButtonValue] = colorsStringsList[EqualityValue];
                    colorsStringsList.RemoveAt(EqualityValue);
                    ColorButtonColor[EqualityButtonValue] = colorsList[EqualityValue];
                    colorsList.RemoveAt(EqualityValue);
                    First = false;
                }

                if (i == EqualityButtonValue)
                    continue;


                int RandomTextValue = random.Next(0, colorsStringsList.Count);
                int RandomColorValue;
                int[] RandomColorValuesArray;

                if (colorsStringsList.Count > 1 && colorsList.Count > 1)
                {
                    RandomColorValuesArray = Enumerable.Range(0, colorsList.Count).Except(new int[] { RandomTextValue }).ToArray();
                    RandomColorValue = RandomColorValuesArray[random.Next(0, RandomColorValuesArray.Length)];
                }
                else
                    RandomColorValue = RandomTextValue;

                ColorButtonText[i] = colorsStringsList[RandomTextValue];
                colorsStringsList.RemoveAt(RandomTextValue);
                ColorButtonColor[i] = colorsList[RandomColorValue];
                colorsList.RemoveAt(RandomColorValue);
            }
        }

    }
}
