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
            ExpressionValuesGenerator = new ExpressionValuesGenerator(tasksOptions, RandomValuesGenerator);
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

            ConstantExpression constantExpression = Expression.Constant(RandomValuesGenerator.GenerateRandomValue(), typeof(int));

            ExpressionValuesGenerator.GenerateBinaryExpression(Operation, binaryExpression, constantExpression);

            #region
            //switch (Operation)
            //{
            //    case "+":
            //        binaryExpression = Expression.Add(binaryExpression, constantExpression);
            //        break;
            //    case "-":
            //        binaryExpression = Expression.Subtract(binaryExpression, constantExpression);
            //        break;
            //    case "*":
            //        if (tasksOptions.IsSpecialModeActivated)
            //        {
            //            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 2);
            //            if (valueplace == 0)
            //                binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(),typeof(int)));
            //            else
            //                binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(),typeof(int)), binaryExpression);
            //        }
            //        else
            //            binaryExpression = Expression.Multiply(binaryExpression, constantExpression);
            //        break;
            //    case "/":
            //        if (tasksOptions.IsSpecialModeActivated)
            //        {
            //            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 2);
            //            if (valueplace == 0)
            //                binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)));
            //            else
            //                binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)), binaryExpression);
            //        }
            //        binaryExpression = Expression.Divide(binaryExpression, constantExpression);
            //        break;
            //    default:
            //        binaryExpression = Expression.Add(binaryExpression, constantExpression);
            //        break;
            //}
            #endregion

            return binaryExpression;
        }

        private BinaryExpression BuildFirstBinaryExpression()
        {
            ConstantExpression param1 = Expression.Constant(RandomValuesGenerator.GenerateRandomValue(), typeof(int));
            ConstantExpression param2 = Expression.Constant(RandomValuesGenerator.GenerateRandomValue(), typeof(int));

            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count,false)];

            BinaryExpression binaryExpression = ExpressionValuesGenerator.GetBinaryExpression(Operation, param1, param2);

            #region
            //switch (Operation)
            //{
            //    case "+":
            //        binaryExpression = Expression.Add(param1, param2);
            //        break;
            //    case "-":
            //        binaryExpression = Expression.Subtract(param1, param2);
            //        break;
            //    case "*":
            //        if (tasksOptions.IsSpecialModeActivated)
            //        {
            //            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 2);
            //            if (valueplace == 0)
            //                binaryExpression = Expression.Multiply(param1, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(),typeof(int)));
            //            else
            //                binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)), param2);
            //        }
            //        else
            //            binaryExpression = Expression.Multiply(param1, param2);
            //        break;
            //    case "/":
            //        if (tasksOptions.IsSpecialModeActivated)
            //        {
            //            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 2);
            //            if (valueplace == 0)
            //                binaryExpression = Expression.Divide(param1, Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue()));
            //            else
            //                binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateDigitRestrictedValue(), typeof(int)), param2);
            //        }
            //        else
            //            binaryExpression = Expression.Divide(param1, param2);
            //        break;
            //    default:
            //        binaryExpression = Expression.Add(param1, param2);
            //        break;
            //}
            #endregion

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
