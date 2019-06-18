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
        private bool ParameterDigitsLimitation = false;

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
            ParameterDigitsLimitation = false;

            if (!tasksOptions.IsChainLengthFixed)
                ChainLength = RandomValuesGenerator.GenerateValuesInRange(2, tasksOptions.MaxChainLength, true);

            variablePlace = RandomValuesGenerator.GenerateValuesInRange(0, ChainLength, false);

            BinaryExpression binaryExpression = BuildFirstBinaryExpression(variablePlace);

            for (int i = 2; i < ChainLength; i++)
            {
                if (i == variablePlace)
                    binaryExpression = ChainBinaryExpression(binaryExpression, true);
                else
                    binaryExpression = ChainBinaryExpression(binaryExpression, false);
            }

            expression = Expression.Lambda<Func<double, double>>(binaryExpression, new ParameterExpression[] { parameterExpression });
            if (ParameterDigitsLimitation)
                XValue = RandomValuesGenerator.GenerateDoubleDigitRestrictedValue();
            else
                XValue = RandomValuesGenerator.GenerateDoubleValue();
            Result = expression.Compile().Invoke(XValue);
        }

        private BinaryExpression BuildFirstBinaryExpression(int variablePlace)
        {
            BinaryExpression binaryExpression;

            Expression param1;
            Expression param2;

            bool FirstParamVariable = false;
            bool SecondParamVariable = false;

            if (variablePlace == 0)
            {
                parameterExpression = Expression.Parameter(typeof(double), "x");
                param1 = parameterExpression;
                FirstParamVariable = true;
            }
            else
                param1 = Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue(),digits));

            if (variablePlace == 1)
            {
                parameterExpression = Expression.Parameter(typeof(double), "x");
                param2 = parameterExpression;
                SecondParamVariable = true;
            }
            else
                param2 = Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue(),digits));

            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            //binaryExpression = ExpressionValuesGenerator.GetBinaryExpression(Operation, param1, param2);

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
                    if (FirstParamVariable || SecondParamVariable)
                    {
                        if (tasksOptions.IsSpecialModeActivated)
                        {
                            if (FirstParamVariable)
                            {
                                binaryExpression = Expression.Multiply(param1, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)));
                            }
                            else if (SecondParamVariable)
                            {
                                binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)), param2);
                            }
                            ParameterDigitsLimitation = true;
                        }
                        else
                            binaryExpression = Expression.Multiply(param1, param2);
                    }
                    binaryExpression = Expression.Multiply(param1, param2);
                    break;
                case "/":
                    if (FirstParamVariable || SecondParamVariable)
                    {
                        if (tasksOptions.IsSpecialModeActivated)
                        {
                            if (FirstParamVariable)
                            {
                                binaryExpression = Expression.Divide(param1, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)));
                            }
                            else if (SecondParamVariable)
                            {
                                binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)), param2);
                            }
                            ParameterDigitsLimitation = true;
                        }
                        else
                            binaryExpression = Expression.Divide(param1, param2);
                    }
                    binaryExpression = Expression.Divide(param1, param2);
                    break;
                default:
                    binaryExpression = Expression.Add(param1, param2);
                    break;
            }
            #endregion;

            return binaryExpression;
        }

        private BinaryExpression ChainBinaryExpression(BinaryExpression binaryExpression, bool IsVariable)
        {
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0, tasksOptions.Operations.Count, false)];

            Expression param;

            if (IsVariable)
            {
                parameterExpression = Expression.Parameter(typeof(double), "x");
                param = parameterExpression;
            }
            else
                param = Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleValue(),digits));

            //ExpressionValuesGenerator.GenerateBinaryExpression(Operation, binaryExpression, param);

            #region
            switch (Operation)
            {
                case "+":
                    binaryExpression = Expression.Add(binaryExpression, param);
                    break;
                case "-":
                    binaryExpression = Expression.Subtract(binaryExpression, param);
                    break;
                case "*":
                    if (!IsVariable)
                    {
                        if (tasksOptions.IsSpecialModeActivated)
                        {
                            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1, true);
                            if (valueplace == 0)
                                binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)));
                            else
                                binaryExpression = Expression.Multiply(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)), binaryExpression);
                        }
                        else
                            binaryExpression = Expression.Multiply(binaryExpression, param);
                    }
                    else
                    {
                        binaryExpression = Expression.Multiply(binaryExpression, param);
                        ParameterDigitsLimitation = true;
                    }
                    break;
                case "/":
                    if (!IsVariable)
                    {
                        if (tasksOptions.IsSpecialModeActivated)
                        {
                            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1, true);
                            if (valueplace == 0)
                                binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)));
                            else
                                binaryExpression = Expression.Divide(Expression.Constant(Math.Round(RandomValuesGenerator.GenerateDoubleDigitRestrictedValue(),digits)), binaryExpression);
                        }
                        else
                            binaryExpression = Expression.Divide(binaryExpression, param);
                    }
                    else
                    {
                        binaryExpression = Expression.Divide(binaryExpression, param);
                        ParameterDigitsLimitation = true;
                    }
                    break;
                default:
                    binaryExpression = Expression.Add(binaryExpression, param);
                    break;
            }
            #endregion

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

