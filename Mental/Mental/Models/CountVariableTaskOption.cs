using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace Mental.Models
{
    public class CountVariableTaskOption : AbstractTaskType
    {
        private Expression<Func<int,int>> expression;

        private int variablePlace;
        private int XValue;
        private ParameterExpression parameterExpression;
        private int Result;
        private bool ParameterDigitsLimitation = false;
   
        public CountVariableTaskOption(MathTasksOptions _tasksOptions)
        {
            tasksOptions = _tasksOptions;
            RandomValuesGenerator = new RandomValuesGenerator(tasksOptions);
            ExpressionValuesGenerator = new ExpressionValuesGenerator(tasksOptions, RandomValuesGenerator);
        }

        public override void GenerateExpression()
        {
            ParameterDigitsLimitation = false;

            if (!tasksOptions.IsChainLengthFixed)
                ChainLength = RandomValuesGenerator.GenerateValuesInRange(2,tasksOptions.MaxChainLength,true);

            variablePlace = RandomValuesGenerator.GenerateValuesInRange(0, ChainLength,false);

            BinaryExpression binaryExpression = BuildFirstBinaryExpression(variablePlace);
               
            for(int i = 2;i < ChainLength;i++)
            {
                if (i == variablePlace)
                    binaryExpression = ChainBinaryExpression(binaryExpression, true);
                else
                    binaryExpression = ChainBinaryExpression(binaryExpression, false);
            }

            expression = Expression.Lambda<Func<int,int>>(binaryExpression, new ParameterExpression[] { parameterExpression });
            if (ParameterDigitsLimitation)
                XValue = RandomValuesGenerator.GenerateDigitRestrictedValue();
            else
                XValue = RandomValuesGenerator.GenerateRandomValue();
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
                parameterExpression = Expression.Parameter(typeof(int), "x");
                param1 = parameterExpression;
                FirstParamVariable = true;
            }
            else
                param1 = Expression.Constant(RandomValuesGenerator.GenerateRandomValue());

            if (variablePlace == 1)
            {
                parameterExpression = Expression.Parameter(typeof(int), "x");
                param2 = parameterExpression;
                SecondParamVariable = true;
            }
            else
                param2 = Expression.Constant(RandomValuesGenerator.GenerateRandomValue());

            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0,tasksOptions.Operations.Count,false)];

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
                                binaryExpression = Expression.Multiply(param1, Expression.Constant(RandomValuesGenerator.GenerateRandomValue()));
                            }
                            else if (SecondParamVariable)
                            {
                                binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateRandomValue()), param2);
                            }
                            ParameterDigitsLimitation = true;
                        }
                        else
                            binaryExpression = Expression.Multiply(param1, param2);
                    }
                    binaryExpression = Expression.Multiply(param1, param2);
                    break;
                case "/":
                    if(FirstParamVariable || SecondParamVariable)
                    {
                        if (tasksOptions.IsSpecialModeActivated)
                        {
                            if (FirstParamVariable)
                            {
                                binaryExpression = Expression.Divide(param1, Expression.Constant(RandomValuesGenerator.GenerateRandomValue()));
                            }
                            else if (SecondParamVariable)
                            {
                                binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateRandomValue()), param2);
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
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0,tasksOptions.Operations.Count,false)];

            Expression param;

            if (IsVariable)
            {
                parameterExpression = Expression.Parameter(typeof(int), "x");
                param = parameterExpression;
            }
            else
                param = Expression.Constant(RandomValuesGenerator.GenerateRandomValue());

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
                            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                            if (valueplace == 0)
                                binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue()));
                            else
                                binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateRandomValue()), binaryExpression);
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
                            int valueplace = RandomValuesGenerator.GenerateValuesInRange(0, 1,true);
                            if (valueplace == 0)
                                binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue()));
                            else
                                binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateRandomValue()), binaryExpression);
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
            return Result.ToString();
        }

        public override bool CheckAnswer(int Answer)
        {
            if (Answer == XValue)
                return true;
            else
                return false;
        }     
    }
}
