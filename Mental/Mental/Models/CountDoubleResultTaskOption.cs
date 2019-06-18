using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace Mental.Models
{
    public class CountDoubleResultTaskOption : AbstractTaskType
    {
        private Expression<Func<double>> expression;
        private int digits;
        private double Result;

        public CountDoubleResultTaskOption(MathTasksOptions _tasksOptions)
        {
            tasksOptions = _tasksOptions;
            digits = _tasksOptions.DigitsAfterDotSign;
            RandomValuesGenerator = new RandomValuesGenerator(tasksOptions);
            ChainLength = tasksOptions.MaxChainLength;
           // ExpressionValuesGenerator = new ExpressionValuesGenerator(tasksOptions, RandomValuesGenerator);
        }

        public override void GenerateExpression()
        {
            BinaryExpression binaryExpression = BuildFirstBinaryExpression();

            if (!tasksOptions.IsChainLengthFixed)
                ChainLength = RandomValuesGenerator.GenerateValuesInRange(2, tasksOptions.MaxChainLength, true);

            for (int i = 2; i < ChainLength; i++)
            {
                binaryExpression = ChainBinaryExpression(binaryExpression);
            }

            expression = Expression.Lambda<Func<double>>(binaryExpression);

            Result = expression.Compile().Invoke();
        }

        private BinaryExpression ChainBinaryExpression(BinaryExpression binaryExpression)
        {
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            ConstantExpression constantExpression = Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue(),digits), typeof(double));

            // ExpressionValuesGenerator.GenerateBinaryExpression(Operation, binaryExpression, constantExpression);

            #region
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
                            binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits), typeof(double)));
                        else
                            binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits), typeof(double)), binaryExpression);
                    }
                    else
                        binaryExpression = Expression.Multiply(binaryExpression, constantExpression);
                    break;
                case "/":
                    if (tasksOptions.IsSpecialModeActivated)
                    {
                        int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                        if (valueplace == 0)
                            binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits), typeof(double)));
                        else
                            binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits), typeof(double)), binaryExpression);
                    }
                    binaryExpression = Expression.Divide(binaryExpression, constantExpression);
                    break;
                default:
                    binaryExpression = Expression.Add(binaryExpression, constantExpression);
                    break;
            }
            #endregion

            return binaryExpression;
        }

        private BinaryExpression BuildFirstBinaryExpression()
        {
            ConstantExpression param1 = Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue(),digits), typeof(double));
            ConstantExpression param2 = Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue(),digits), typeof(double));

            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            BinaryExpression binaryExpression;

            // BinaryExpression binaryExpression = ExpressionValuesGenerator.GetBinaryExpression(Operation, param1, param2);

            #region
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
                            binaryExpression = Expression.Multiply(param1, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits), typeof(double)));
                        else
                            binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits), typeof(double)), param2);
                    }
                    else
                        binaryExpression = Expression.Multiply(param1, param2);
                    break;
                case "/":
                    if (tasksOptions.IsSpecialModeActivated)
                    {
                        int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                        if (valueplace == 0)
                            binaryExpression = Expression.Divide(param1, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits),typeof(double)));
                        else
                            binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits), typeof(double)), param2);
                    }
                    else
                        binaryExpression = Expression.Divide(param1, param2);
                    break;
                default:
                    binaryExpression = Expression.Add(param1, param2);
                    break;
            }
            #endregion

            return binaryExpression;
        }

        public override bool CheckAnswer(string Answer)
        {
            double result = expression.Compile().Invoke();
            double answer = Convert.ToDouble(Answer);

            if ((int)result == (int)answer)
            {
                string resultString = result.ToString();
                string answerString = answer.ToString();

                int dotIndex = 0;

                for(int i = 0;i < resultString.Length;i++)
                {
                    if (resultString[i] == ',' || resultString[i] == '.')
                    {
                        dotIndex = i;
                        break;
                    }
                }

                int requiredLength = dotIndex + digits + 1;

                while(resultString.Length < requiredLength || answerString.Length < requiredLength)
                {
                    if (resultString.Length < requiredLength)
                        resultString += "0";
                    else if(answerString.Length < requiredLength)
                        answerString += "0";
                }

                for(int i = dotIndex + 1;i <= dotIndex + digits;i++)
                {
                    if (resultString[i] != answerString[i])
                        return false;
                }

                return true;               
            }
            else
                return false;
        }

        public override string GetResult()
        {
            int dotIndex = 0;
            string ResultString = Result.ToString();

            for(int i = 0;i < ResultString.Length;i++)
            {
                if (ResultString[i] == ',' || ResultString[i] == '.')
                    dotIndex = i;
            }

            return ResultString.Substring(0, dotIndex + digits);
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
