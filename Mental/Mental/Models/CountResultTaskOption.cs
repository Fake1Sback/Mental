using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Mental.Models
{
    public class CountResultTaskOption : AbstractTaskType
    {
        private Expression<Func<int>> expression;
 
        private int Result;

        public CountResultTaskOption(MathTasksOptions _tasksOptions)
        {
            tasksOptions = _tasksOptions;
            RandomValuesGenerator = new RandomValuesGenerator(tasksOptions);
          //  ExpressionValuesGenerator = new ExpressionValuesGenerator(tasksOptions, RandomValuesGenerator);
            ChainLength = tasksOptions.MaxChainLength;
        }

        public override void GenerateExpression()
        {
            BinaryExpression binaryExpression = BuildFirstBinaryExpression();

            if (!tasksOptions.IsChainLengthFixed)
                ChainLength = RandomValuesGenerator.GenerateValuesInRange(2,tasksOptions.MaxChainLength,true);

            for (int i = 2; i < ChainLength; i++)
            {
                binaryExpression = ChainBinaryExpression(binaryExpression);
            }

            expression = Expression.Lambda<Func<int>>(binaryExpression);

            Result = expression.Compile().Invoke();
        }

        private BinaryExpression ChainBinaryExpression(BinaryExpression binaryExpression)
        {
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count,false)];

            int placeFactor = RandomValuesGenerator.GenerateValuesInRange(0, 1, true);

            switch (Operation)
            {
                case "+":
                    if (placeFactor == 0)
                        binaryExpression = Expression.Add(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                    else
                        binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), binaryExpression);
                    break;
                case "-":
                    if (placeFactor == 0)
                        binaryExpression = Expression.Subtract(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", false)));
                    else
                        binaryExpression = Expression.Subtract(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", true)), binaryExpression);
                    break;
                case "*":
                    if (placeFactor == 0)
                        binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", false)));
                    else
                        binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", true)), binaryExpression);
                    break;
                case "/":
                    if (placeFactor == 0)
                        binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", false)));
                    else
                        binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", true)), binaryExpression);
                    break;
                default:
                    if (placeFactor == 0)
                        binaryExpression = Expression.Add(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                    else
                        binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), binaryExpression);
                    break;
            }
        
            return binaryExpression;
        }

        private BinaryExpression BuildFirstBinaryExpression()
        {          
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            BinaryExpression binaryExpression;

            switch (Operation)
            {
                case "+":
                    binaryExpression = Expression.Add(ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("+",true)),ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("+",false)));
                    break;
                case "-":
                    binaryExpression = Expression.Subtract(ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("-", true)), ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("-", false)));
                    break;
                case "*":         
                        binaryExpression = Expression.Multiply(ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("*", true)), ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("*", false)));
                     break;
                case "/":
                    binaryExpression = Expression.Divide(ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("/", true)), ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("/", false)));
                    break;
                default:
                    binaryExpression = Expression.Add(ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), ConstantExpression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                    break;
            }

            return binaryExpression;
        }

        public override bool CheckAnswer(string Answer)
        {
            int Result = expression.Compile().Invoke();
            int ValueToCompare;
            if (Int32.TryParse(Answer, out ValueToCompare))
            {
                if (Result == ValueToCompare)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        public override string GetResult()
        {
            return Result.ToString();
        }
       
        public override string GetExpressionString()
        {
            string InitialString = expression.ToString();
            InitialString = InitialString.Remove(InitialString.Length - 1);
            InitialString = InitialString.Remove(0, 7);
            return InitialString;
        }
    }
}
