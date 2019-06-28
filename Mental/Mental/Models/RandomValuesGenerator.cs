using System;
using System.Collections.Generic;
using System.Text;
using Mental.Models;

namespace Mental.Models
{
    public class RandomValuesGenerator
    {
        private MathTasksOptions tasksOptions;
        private Random random;

        private bool DivideOperation = false;

        public RandomValuesGenerator(MathTasksOptions _tasksOptions)
        {
            tasksOptions = _tasksOptions;
            random = new Random();
        }

        private int FindAmountOfDigits(int Value)
        {
            int AmountOfDigits = 0;
            if (Value < 0)
                Value = Value * -1;
            while (Value >= 0)
            {
                Value = Value / 10;
                AmountOfDigits += 1;
                if (Value == 0)
                    break;
            }
            return AmountOfDigits;
        }

        public int GenerateRandomValue(string Operation, bool IsFirstValue)
        {
            int Result;
            if (Operation == "/")
                DivideOperation = true;
            else
                DivideOperation = false;

            if (tasksOptions.IsRestrictionsActivated)
            {
                OperationRestriction operationRestriction;
                switch (Operation)
                {
                    case "+":
                        operationRestriction = tasksOptions.restrictions.restrictions[0];
                        break;
                    case "-":
                        operationRestriction = tasksOptions.restrictions.restrictions[1];
                        break;
                    case "*":
                        operationRestriction = tasksOptions.restrictions.restrictions[2];
                        break;
                    case "/":
                        operationRestriction = tasksOptions.restrictions.restrictions[3];
                        break;
                    default:
                        operationRestriction = tasksOptions.restrictions.restrictions[0];  //FIX !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        break;
                }
                if (operationRestriction.IsBlockActivated)
                {
                    int DigitLength = 0;

                    if (IsFirstValue)
                    {
                        if (operationRestriction.IsDigit1HardRestriction)
                            DigitLength = operationRestriction.Digit1Restriction;
                        else
                            DigitLength = GenerateValuesInRange(FindAmountOfDigits(tasksOptions.MinValue), operationRestriction.Digit1Restriction, true);
                    }
                    else
                    {
                        if (operationRestriction.IsDigit2HardRestriction)
                            DigitLength = operationRestriction.Digit2Restriction;
                        else
                            DigitLength = GenerateValuesInRange(FindAmountOfDigits(tasksOptions.MinValue), operationRestriction.Digit2Restriction, true);
                    }

                    int ValueToReturn = 0;
                    int maxValue = 10;
                    int minValue = 1;

                    for (int i = 1; i < DigitLength; i++)
                    {
                        minValue *= 10;
                        maxValue *= 10;
                    }

                    if (FindAmountOfDigits(minValue) == FindAmountOfDigits(tasksOptions.MinValue))
                    {
                        if (Math.Abs(minValue) < Math.Abs(tasksOptions.MinValue))
                        {
                            minValue = tasksOptions.MinValue;
                        }
                    }
                    if (FindAmountOfDigits(maxValue - 1) == FindAmountOfDigits(tasksOptions.MaxValue))
                    {
                        if (Math.Abs(maxValue - 1) > Math.Abs(tasksOptions.MaxValue))
                        {
                            maxValue = tasksOptions.MaxValue;
                        }
                    }

                    Result = ValueToReturn = random.Next(minValue, maxValue);                 
                }
                else
                    Result = random.Next(tasksOptions.MinValue, tasksOptions.MaxValue + 1);
            }
            else
                Result = random.Next(tasksOptions.MinValue, tasksOptions.MaxValue + 1);

            if (DivideOperation && !IsFirstValue && Result == 0)
            {
                if (tasksOptions.MaxValue == 0)
                    Result -= 1;
                else
                    Result += 1;
            }
 
            return Result;
        }

        public double GenerateDoubleValue(string Operation,bool FirstValue)
        {
            int integerPart = GenerateRandomValue(Operation,FirstValue);
            return integerPart + random.NextDouble();
        }

        public int GenerateValuesInRange(int min, int max, bool MaxInclusive)
        {
            if (!MaxInclusive)
                return random.Next(min, max);
            else
                return random.Next(min, max + 1);
        }

        public double GenerateDoubleValuesInRange(int min,int max,bool MaxInclusive)
        {
            int integerpart;
            if (!MaxInclusive)
                 integerpart = random.Next(min, max);
            else
                integerpart = random.Next(min, max + 1);

            return integerpart + random.NextDouble();
        }
    }
}
