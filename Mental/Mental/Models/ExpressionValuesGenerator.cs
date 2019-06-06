using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace Mental.Models
{
    public class ExpressionValuesGenerator
    {
        private MathTasksOptions tasksOptions;
        private RandomValuesGenerator RandomValuesGenerator;

        public ExpressionValuesGenerator(MathTasksOptions _tasksOptions, RandomValuesGenerator _randomValuesGenerator)
        {
            tasksOptions = _tasksOptions;
            RandomValuesGenerator = _randomValuesGenerator;
        }

        public void GenerateBinaryExpression(string Operation,Expression binaryExpression,Expression constantExpression)
        {
            switch (Operation)
            {
                case "+":
                    binaryExpression = Expression.Add(binaryExpression, constantExpression);
                    break;
                case "-":
                    binaryExpression = Expression.Subtract(binaryExpression, constantExpression);
                    break;
                case "*":
                    if (tasksOptions.IsSpecialModeActivated)
                    {
                        int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                        if (valueplace == 0)
                            binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)));
                        else
                            binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)), binaryExpression);
                    }
                    else
                        binaryExpression = Expression.Multiply(binaryExpression, constantExpression);
                    break;
                case "/":
                    if (tasksOptions.IsSpecialModeActivated)
                    {
                        int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                        if (valueplace == 0)
                            binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)));
                        else
                            binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)), binaryExpression);
                    }
                    binaryExpression = Expression.Divide(binaryExpression, constantExpression);
                    break;
                default:
                    binaryExpression = Expression.Add(binaryExpression, constantExpression);
                    break;
            }
        }

        public BinaryExpression GetBinaryExpression(string Operation,Expression param1,Expression param2)
        {
            BinaryExpression binaryExpression;

            switch (Operation)
            {
                case "+":
                    binaryExpression = Expression.Add(param1, param2);
                    break;
                case "-":
                    binaryExpression = Expression.Subtract(param1, param2);
                    break;
                case "*":
                    if (tasksOptions.IsSpecialModeActivated)
                    {
                        int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                        if (valueplace == 0)
                            binaryExpression = Expression.Multiply(param1, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)));
                        else
                            binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)), param2);
                    }
                    else
                        binaryExpression = Expression.Multiply(param1, param2);
                    break;
                case "/":
                    if (tasksOptions.IsSpecialModeActivated)
                    {
                        int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                        if (valueplace == 0)
                            binaryExpression = Expression.Divide(param1, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue()));
                        else
                            binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)), param2);
                    }
                    else
                        binaryExpression = Expression.Divide(param1, param2);
                    break;
                default:
                    binaryExpression = Expression.Add(param1, param2);
                    break;
            }

            return binaryExpression;
        }
    }
}
