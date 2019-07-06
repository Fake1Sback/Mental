using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class StroopTaskOptions
    {
        public int ButtonsAmount;
        public ColorShowingOptions ColorShowingOption;
        public StroopTaskType StroopTaskType;
        public TaskTimeOptionsContainer TaskTimeOptionsContainer;
    }

    public enum StroopTaskType
    {
        FindOneCorrect,
        TrueOrFalse,
        FindColorByText
    }

    public enum ColorShowingOptions
    {
        BackgroundColor,
        BorderColor
    }
}
