using Mental.Models;
using Mental.Models.DbModels;
using Mental.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Mental.ViewModels
{
    public class StroopTrueOrFalseTaskVM : BaseStroopTaskVM
    {
        private Random random;

        public StroopTrueOrFalseTaskVM(INavigation _navigation, StroopTaskOptions _stroopTaskOptions, ITimeOption _timeOption,StroopTaskPage _stroopTaskPage) : base(_navigation, _stroopTaskOptions, _timeOption,_stroopTaskPage)
        {
            QuestionLabelVisibility = false;
            YesNoLayoutVisibility = true;
            random = new Random();
            YesButtonClickedCommand = new Command(YesButtonClicked);
            NoButtonClickedCommand = new Command(NoButtonClicked);
            GenerateTask();
        }

        protected override void GenerateTask()
        {
            bool IsTaskTrue = RandomBool();

            colorsList = new List<Color>(colors);
            colorsStringsList = new List<string>(colorsStrings);

            for (int i = 0; i < stroopTaskOptions.ButtonsAmount; i++)
            {
                int index = random.Next(0, colorsStringsList.Count);
                ColorButtonText[i] = colorsStringsList[index];
                colorsStringsList.RemoveAt(index);
                ColorButtonColor[i] = colorsList[index];
                colorsList.RemoveAt(index);
            }

            if (!IsTaskTrue)
            {
                int MaxAmountOfWrongAnswers = stroopTaskOptions.ButtonsAmount / 2;
                int CurrentAmountOfWrongAnswers = random.Next(1, MaxAmountOfWrongAnswers + 1);

                List<int> AllreadyUsedIndexes = new List<int>();

                for (int i = 0; i < CurrentAmountOfWrongAnswers; i++)
                {
                    bool IsChangeString = RandomBool();
                    int RandomValue = random.Next(0, stroopTaskOptions.ButtonsAmount);

                    if (IsChangeString)
                    {
                        int RandomStringListIndex = random.Next(0, colorsStringsList.Count);
                        string DeletedValue = ColorButtonText[RandomValue];
                        ColorButtonText[RandomValue] = colorsStringsList[RandomStringListIndex];
                        colorsStringsList.RemoveAt(RandomStringListIndex);
                        colorsStringsList.Add(DeletedValue);
                    }
                    else
                    {
                        int RandomColorListIndex = random.Next(0, colorsList.Count);
                        Color DeletedColor = ColorButtonColor[RandomValue];
                        ColorButtonColor[RandomValue] = colorsList[RandomColorListIndex];
                        colorsList.RemoveAt(RandomColorListIndex);
                        colorsList.Add(DeletedColor);
                    }
                }
            }
        }

        private void YesButtonClicked()
        {
            bool Result = CompareTaskResults(true);
            CheckEndOfTask(Result);
        }

        private void NoButtonClicked()
        {
            bool Result = CompareTaskResults(false);
            CheckEndOfTask(Result);
        }

        private bool CompareTaskResults(bool UserResult)
        {
            bool FunctionResult = true;

            for (int i = 0; i < stroopTaskOptions.ButtonsAmount; i++)
            {
                string text = ColorButtonText[i];
                int TextIndex = Array.FindIndex(colorsStrings, s => s == text);

                Color color = ColorButtonColor[i];
                int ColorIndex = Array.FindIndex(colors, c => c == color);

                FunctionResult = TextIndex == ColorIndex;
                if (!FunctionResult)
                    break;
            }

            if (UserResult == FunctionResult)
                return true;
            else
                return false;
        }

        private void CheckEndOfTask(bool CompareResult)
        {
            if (CompareResult)
                AmountOfCorrectAnswers += 1;
            else
                AmountOfWrongAnswers += 1;

            if (timeOption.CanExecuteOperation(CompareResult))
            {               
                GenerateTask();
            }
            else
            {
                stroopTaskPage.HideTaskFrame();
                VMTimerBlocker = true;
            }
        }

        private bool RandomBool()
        {
            int randomValue = random.Next(0, 2);
            if (randomValue == 0)
                return true;
            else
                return false;
        }
    }
}
