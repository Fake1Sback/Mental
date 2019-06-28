using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace Mental.Models
{
    public class CountDoubleVariableTaskOption : AbstractTaskType
    {
        private Expression<Func<double, double>> expression;

        private int variablePlace;
        private int digits;
        private double XValue;
        private ParameterExpression parameterExpression;
        private double Result;

        private string ParameterOperation = "+";
        private bool IsParameterFirst = true;

        public CountDoubleVariableTaskOption(MathTasksOptions _tasksOptions)
        {
            tasksOptions = _tasksOptions;
            digits = tasksOptions.DigitsAfterDotSign;
            RandomValuesGenerator = new RandomValuesGenerator(tasksOptions);
            ChainLength = tasksOptions.MaxChainLength;
           // ExpressionValuesGenerator = new ExpressionValuesGenerator(tasksOptions, RandomValuesGenerator);
        }

        public override void GenerateExpression()
        {
            if (!tasksOptions.IsChainLengthFixed)
                ChainLength = RandomValuesGenerator.GenerateValuesInRange(2, tasksOptions.MaxChainLength, true);

            variablePlace = RandomValuesGenerator.GenerateValuesInRange(0, ChainLength, false);

            parameterExpression = Expression.Parameter(typeof(double), "x");

            BinaryExpression binaryExpression = BuildFirstBinaryExpression(variablePlace);

            for (int i = 2; i < ChainLength; i++)
            {
                if (i == variablePlace)
                    binaryExpression = ChainBinaryExpression(binaryExpression, true);
                else
                    binaryExpression = ChainBinaryExpression(binaryExpression, false);
            }

            expression = Expression.Lambda<Func<double, double>>(binaryExpression, new ParameterExpression[] { parameterExpression });
            XValue = RandomValuesGenerator.GenerateDoubleValue(ParameterOperation, IsParameterFirst);

            try
            {
                Result = expression.Compile().Invoke(XValue);
            }
            catch(DivideByZeroException)
            {
                GenerateExpression();
            }
        }

        private BinaryExpression BuildFirstBinaryExpression(int variablePlace)
        {
            BinaryExpression binaryExpression;

            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            switch (Operation)
            {
                case "+":
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Add(parameterExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false),digits)));
                        ParameterOperation = "+"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true),digits)), parameterExpression);
                        ParameterOperation = "+"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false), digits)));
                    break;
                case "-":
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Subtract(parameterExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", false), digits)));
                        ParameterOperation = "-"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Subtract(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", true), digits)), parameterExpression);
                        ParameterOperation = "-"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Subtract(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", false), digits)));
                    break;
                case "*":
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Multiply(parameterExpression , Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", false), digits)));
                        ParameterOperation = "*"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", true), digits)), parameterExpression);
                        ParameterOperation = "*"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", false), digits)));
                    break;
                case "/":
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Divide(parameterExpression , Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", false), digits)));
                        ParameterOperation = "/"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", true), digits)), parameterExpression);
                        ParameterOperation = "/"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", false), digits)));
                    break;
                default:
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Add(parameterExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false), digits)));
                        ParameterOperation = "+"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true), digits)), parameterExpression);
                        ParameterOperation = "+"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true), digits)), Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false), digits)));
                    break;
            }

            return binaryExpression;
        }

        private BinaryExpression ChainBinaryExpression(BinaryExpression binaryExpression, bool IsVariable)
        {
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            int placeFactor = RandomValuesGenerator.GenerateValuesInRange(0, 1, true);

            switch (Operation)
            {
                case "+":
                    if (placeFactor == 0)
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Add(binaryExpression, parameterExpression);
                            ParameterOperation = "+"; IsParameterFirst = false;
                        }
                        else
                            binaryExpression = Expression.Add(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false),digits)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Add(parameterExpression, binaryExpression);
                            ParameterOperation = "+"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true),digits)), binaryExpression);
                    }
                    break;
                case "-":
                    if (placeFactor == 0)
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Subtract(binaryExpression, parameterExpression);
                            ParameterOperation = "-"; IsParameterFirst = false;
                        }
                        else
                            binaryExpression = Expression.Subtract(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", false), digits)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Subtract(parameterExpression, binaryExpression);
                            ParameterOperation = "-"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Subtract(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("-", true), digits)), binaryExpression);
                    }
                    break;
                case "*":
                    if (placeFactor == 0)
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Multiply(binaryExpression, parameterExpression);
                            ParameterOperation = "*"; IsParameterFirst = false;
                        }
                        else
                            binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", false), digits)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Multiply(parameterExpression, binaryExpression);
                            ParameterOperation = "*"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("*", true), digits)), binaryExpression);
                    }
                    break;
                case "/":
                    if (placeFactor == 0)
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Divide(binaryExpression, parameterExpression);
                            ParameterOperation = "/"; IsParameterFirst = false;
                        }
                        else
                            binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", false), digits)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Divide(parameterExpression, binaryExpression);
                            ParameterOperation = "/"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("/", true), digits)), binaryExpression);
                    }
                    break;
                default:
                    if (placeFactor == 0)
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Add(binaryExpression, parameterExpression);
                            ParameterOperation = "+"; IsParameterFirst = false;
                        }
                        else
                            binaryExpression = Expression.Add(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", false), digits)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Add(parameterExpression, binaryExpression);
                            ParameterOperation = "+"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Add(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue("+", true), digits)), binaryExpression);
                    }
                    break;
            }

            return binaryExpression;
        }

        public override string GetExpressionString()
        {
            string InitialString = expression.ToString();
            InitialString = InitialString.Remove(0, 6);
            InitialString = InitialString.Remove(InitialString.Length - 1);
            return InitialString;
        }

        public override string GetResult()
        {
            int dotIndex = 0;
            string ResultString = Result.ToString();

            for (int i = 0; i < ResultString.Length; i++)
            {
                if (ResultString[i] == ',' || ResultString[i] == '.')
                    dotIndex = i;
            }

            return ResultString.Substring(0, dotIndex + digits + 1);
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
        }

        private bool CompareResults(string Answer,bool IsRounded)
        {
            double result;
            double answer = Convert.ToDouble(Answer);

            if (IsRounded)
                result = Math.Round(XValue, digits);
            else
                result = XValue;
            

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

