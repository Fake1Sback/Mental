using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mental.Models;

namespace Mental.Services
{
    public static class OptionsInfoDictionary
    {
        private static List<InfoListItem> InfoItems = new List<InfoListItem>
        {
             new InfoListItem{Index = "OperationsInfo", Caption = "Operations", Info = "Defines what arithmetic operations will be used for generating questions in task.\n  Available operations:\n  +  --- Addition\n  -  --- Substraction\n  *  --- Multiplication\n  /  --- Division\nAt least 1 operation must be chosen."},
             new InfoListItem{Index = "ChainLengthInfo",Caption = "Chain Length", Info = "Defines how much arithmetic operations you need to do in order to answer 1 question. If Chain Length is fixed its length will equal to the maximum value. If Chain Length is not fixed its length may vary from minimum value (2 operations) to maximum value."},
             new InfoListItem{Index = "GeneratedValuesInfo", Caption = "Generated Values", Info = "Defines which minimum and maximum numbers will be used in math task."},
             new InfoListItem{Index = "RestrictionsInfo", Caption = "Restrictions", Info = "Defines amount of digits in numbers for each arithmetic operation. Restrictions are applied separately for left and right values. If Hard Restrction is activated amount of digits in number will be equal to the slider value, if not amount of digits in number may vary from minimum value to slider value."},
             new InfoListItem{Index = "NumbersTypeInfo", Caption = "Numbers Type", Info = "Defines numbers of which type will be used in math task.\nAvailable options:\n  Integers --- 1,5,10...etc\n  Fractionals --- 1.2,2.15...etc\nDigits after dot sign option defines how much digits will be in fractional part of fractional number."},
             new InfoListItem{Index = "MathTaskTypeInfo", Caption = "Task Type", Info = "Defines which type of math task will be generated.\nAvailable math tasks:\n  Count Result --- Find result by completing arithmetic operations with constant values.\nFor example: 2 + 2 = ?\n  Count Variable --- Find variable value (x value) having the result and constant values.\n For example:  2 + ? = 4"},
             new InfoListItem{Index = "TimeOptionsInfo", Caption = "Time Options", Info = "Defines conditions under which task will end. Available options:\n  Countdown --- Countdown timer in minutes. Task will end when time ends.\n  Limited tasks --- Fixed amount of questions. Task will end when all questions are answered.\n Last Task --- Countdown timer in seconds. Task will end when time ends or mistake is made."},
             new InfoListItem{Index = "GridSizeInfo", Caption = "Grid Size", Info = "Defines amount of grid items you need to find in order to complete task."},
             new InfoListItem{Index = "EasyModeInfo", Caption = "Easy Mode", Info = "If Easy Mode is active allready found grid items will be highlighted."},
             new InfoListItem{Index = "ButtonsAmountInfo", Caption = "Buttons amount", Info = "Defines how much options to choose from, or how much options you need to check in order to complete task."},
             new InfoListItem{Index = "StroopTaskTypeInfo", Caption = "Task Type", Info = "Defines which type of Stroop task will be generated. Available Stroop tasks:\n  Find 1 Correct --- Find button in which text color equals to color that this text means.\n  True or False --- Check if on all generated buttons text color equals to color that this text means.\n  Find Color by Text --- Find one button in which text color equals to color that requested button text means."},
             new InfoListItem{Index = "FavouriteRecordsLimitation", Caption = "Favourites Limitation", Info = "You can`t have more than 10 favourite task options records in database. To save new records you have to delete previous."}
        };

        public static string GetCaption(string Index)
        {
            return InfoItems.Where(i => i.Index == Index).Select(i => i.Caption).FirstOrDefault();
        }

        public static string GetInfoText(string Index)
        {
            return InfoItems.Where(i => i.Index == Index).Select(i => i.Info).FirstOrDefault();
        }
    }
}
