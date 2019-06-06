using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public class RandomValuesGenerator
    {
        private MathTasksOptions tasksOptions;
        private Random random;

        public RandomValuesGenerator(MathTasksOptions _tasksOptions)
        {
            tasksOptions = _tasksOptions;
            random = new Random();
        }

        public int GenerateValuesInRange(int min,int max,bool MaxInclusive)
        {
            if (!MaxInclusive)
                return random.Next(min, max);
            else
                return random.Next(min, max + 1);
        }

        public int GenerateRandomValue()
        {
            return random.Next(tasksOptions.MinValue, tasksOptions.MaxValue + 1);
        }

        public int GenerateDigitRestrictedValue()
        {  
            int maxValue = 10;
            int minValue = 1;

            for (int i = 1;i < tasksOptions.AmountOfXDigits;i++)
            {
                minValue *= 10;
                maxValue *= 10;
            }

            int minusFactor = random.Next(0, 2);

            if (minusFactor == 0)
                return random.Next(minValue, maxValue);
            else
                return random.Next(minValue, maxValue) * -1;
        }

        public double GenerateDoubleValue()
        {
            int integerPart = GenerateRandomValue();
            return integerPart + random.NextDouble();
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
