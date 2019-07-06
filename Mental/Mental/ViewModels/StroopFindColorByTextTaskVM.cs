using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mental.Models;
using Mental.Models.DbModels;
using Xamarin.Forms;
using Mental.Views;

namespace Mental.ViewModels
{
    public class StroopFindColorByTextTaskVM : BaseStroopTaskVM
    {
        private Random random;

        public StroopFindColorByTextTaskVM(INavigation _navigation,StroopTaskOptions _stroopTaskOptions,ITimeOption _timeOption) : base(_navigation,_stroopTaskOptions,_timeOption)
        {
            QuestionLabelVisibility = true;
            YesNoLayoutVisibility = false;
            random = new Random();
            ColorButtonClickedCommand = new Command(ColorButtonClicked);
            GenerateTask();
        }

        private void GenerateTask()
        {
            colorsStringsList = new List<string>(colorsStrings);
            colorsList = new List<Color>(colors);

            int TrueTextStringIndex = random.Next(0, colorsStringsList.Count);
            int TrueButtonIndex = random.Next(0, stroopTaskOptions.ButtonsAmount);

            QuestionLabelString = colorsStringsList[TrueTextStringIndex];
            

            int[] arr = Enumerable.Range(0, colorsList.Count).Except(new int[] { TrueTextStringIndex }).ToArray();
            int RandomIndex = random.Next(0, arr.Length);
            QuestrionLabelTextColor = colorsList[arr[RandomIndex]];
            
            ColorButtonColor[TrueButtonIndex] = colorsList[TrueTextStringIndex];
            colorsStringsList.RemoveAt(TrueTextStringIndex);
            colorsList.RemoveAt(TrueTextStringIndex);
            colorsList.RemoveAt(RandomIndex);

            int RandomIndex2 = random.Next(0, colorsStringsList.Count);
            ColorButtonText[TrueButtonIndex] = colorsStringsList[RandomIndex2];
            colorsStringsList.RemoveAt(RandomIndex2);

            for(int i = 0;i < stroopTaskOptions.ButtonsAmount;i++)
            {
                if (i == TrueButtonIndex)
                    continue;

                int RandomTextIndex = random.Next(0, colorsStringsList.Count);
                ColorButtonText[i] = colorsStringsList[RandomTextIndex];
                colorsStringsList.RemoveAt(RandomTextIndex);
                int RandomColorIndex = random.Next(0, colorsList.Count);
                ColorButtonColor[i] = colorsList[RandomColorIndex];
                colorsList.RemoveAt(RandomColorIndex);
            }
        }

        private void ColorButtonClicked(object obj)
        {
            Button button = obj as Button;
            Color ButtonTextColor = button.TextColor;
            int ColorIndex = Array.FindIndex(colors, c => c == ButtonTextColor);
            int StringIndex = Array.FindIndex(colorsStrings, s => s == QuestionLabelString);

            bool IsCorrect = ColorIndex == StringIndex;

            if (IsCorrect)
                AmountOfCorrectAnswers += 1;
            else
                AmountOfWrongAnswers += 1;

            if (timeOption.CanExecuteOperation(IsCorrect))
            {             
                GenerateTask();
            }
            else
            {
                NavigateToSimilarStatisticsPage();
            }
        }
    }
}
