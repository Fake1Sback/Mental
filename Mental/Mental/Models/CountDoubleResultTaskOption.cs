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

            try
            {
                Result = expression.Compile().Invoke();
            }
            catch(DivideByZeroException)
            {
                GenerateExpression();
            }
        }

        private BinaryExpression ChainBinaryExpression(BinaryExpression binaryExpression)
        {
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            int placeFactor = RandomValuesGenerator.GenerateValuesInRange(0, 1, true);
   
            switch (Operation)
            {
                case "+":
                    if (placeFactor == 0)
                        binaryExpression = Expression.Add(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false), digits)));
                    else
                        binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true), digits)), binaryExpression);
                    break;
                case "-":
                    if (placeFactor == 0)                       
                        binaryExpression = Expression.Subtract(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", false), digits)));
                    else
                        binaryExpression = Expression.Subtract(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", true), digits)), binaryExpression);
                    break;
                case "*":
                    if (placeFactor == 0)
                        binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", false), digits)));
                    else
                        binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", true), digits)), binaryExpression);
                    break;
                case "/":
                    if (placeFactor == 0)
                        binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", false), digits)));
                    else
                        binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", true), digits)), binaryExpression);
                    break;
                default:
                    if (placeFactor == 0)
                        binaryExpression = Expression.Add(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false), digits)));
                    else
                        binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true), digits)), binaryExpression);
                    break;
            }

            return binaryExpression;
        }

        private BinaryExpression BuildFirstBinaryExpression()
        {
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];
      
            BinaryExpression binaryExpression;

            // BinaryExpression binaryExpression = ExpressionValuesGenerator.GetBinaryExpression(Operation, param1, param2);

            #region
            switch (Operation)
            {
                case "+":
                    binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+",true),digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+",false),digits)));
                    break;
                case "-":
                    binaryExpression = Expression.Subtract(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", false), digits)));
                    break;
                case "*":
                        binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", false), digits)));
                    break;
                case "/":
                        binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", false), digits)));
                    break;
                default:
                    binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false), digits)));
                    break;
            }
            #endregion

            return binaryExpression;
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

        public override bool CheckAnswer(string Answer)
        {
            bool FirstCheck = CompareResults(Answer, false);
            if (!FirstCheck)
            {
                bool SecondCheck = CompareResults(Answer, true);
                if (SecondCheck)
                    return true;
                else
                    return false;
            }
            else
                return true;
            #region
            //double result = expression.Compile().Invoke();
            //double answer = Convert.ToDouble(Answer);

            //if ((int)result == (int)answer)
            //{
            //    string resultString = result.ToString();
            //    string answerString = answer.ToString();

            //    int dotIndex = 0;

            //    for(int i = 0;i < resultString.Length;i++)
            //    {
            //        if (resultString[i] == ',' || resultString[i] == '.')
            //        {
            //            dotIndex = i;
            //            break;
            //        }
            //    }

            //    int requiredLength = dotIndex + digits + 1;

            //    while(resultString.Length < requiredLength || answerString.Length < requiredLength)
            //    {
            //        if (resultString.Length < requiredLength)
            //            resultString += "0";
            //        else if(answerString.Length < requiredLength)
            //            answerString += "0";
            //    }

            //    for(int i = dotIndex + 1;i <= dotIndex + digits;i++)
            //    {
            //        if (resultString[i] != answerString[i])
            //            return false;
            //    }

            //    return true;               
            //}
            //else
            //    return false;
            #endregion
        }

        private bool CompareResults(string Answer, bool IsRounded)
        {
            double result;
            double answer = Convert.ToDouble(Answer);

            if (IsRounded)
                result = Math.Round(Result, digits);
            else
                result = Result;


            if ((int)result == (int)answer)
            {
                string resultString = result.ToString();
                string answerString = answer.ToString();

                int dotIndex = 0;

                for (int i = 0; i < resultString.Length; i++)
                {
                    if (resultString[i] == ',' || resultString[i] == '.')
                    {
                        dotIndex = i;
                        break;
                    }
                }

                int requiredLength = dotIndex + digits + 1;

                while (resultString.Length < requiredLength || answerString.Length < requiredLength)
                {
                    if (resultString.Length < requiredLength)
                        resultString += "0";
                    else if (answerString.Length < requiredLength)
                        answerString += "0";
                }

                for (int i = dotIndex + 1; i <= dotIndex + digits; i++)
                {
                    if (resultString[i] != answerString[i])
                        return false;
                }

                return true;
            }
            else
                return false;
        }
    }
}
