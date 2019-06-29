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

        private string ParameterOperation = "+";
        private bool IsParameterFirst = true;
   
        public CountVariableTaskOption(MathTasksOptions _tasksOptions)
        {
            tasksOptions = _tasksOptions;
            RandomValuesGenerator = new RandomValuesGenerator(tasksOptions);
            ChainLength = tasksOptions.MaxChainLength;
        }

        public override void GenerateExpression()
        { 
            if (!tasksOptions.IsChainLengthFixed)
                ChainLength = RandomValuesGenerator.GenerateValuesInRange(2,tasksOptions.MaxChainLength,true);

            variablePlace = RandomValuesGenerator.GenerateValuesInRange(0, ChainLength,false);

            parameterExpression = Expression.Parameter(typeof(int), "x");

            BinaryExpression binaryExpression = BuildFirstBinaryExpression(variablePlace);
               
            for(int i = 2;i < ChainLength;i++)
            {
                if (i == variablePlace)
                    binaryExpression = ChainBinaryExpression(binaryExpression, true);
                else
                    binaryExpression = ChainBinaryExpression(binaryExpression, false);
            }

            expression = Expression.Lambda<Func<int,int>>(binaryExpression, new ParameterExpression[] { parameterExpression });
            XValue = RandomValuesGenerator.GenerateRandomValue(ParameterOperation, IsParameterFirst);

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
                        binaryExpression = Expression.Add(parameterExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                        ParameterOperation = "+"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), parameterExpression);
                        ParameterOperation = "+"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                    break;
                case "-":
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Subtract(parameterExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", false)));
                        ParameterOperation = "-"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Subtract(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", true)), parameterExpression);
                        ParameterOperation = "-"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Subtract(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", true)), Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", false)));
                    break;
                case "*":
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Multiply(parameterExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", false)));
                        ParameterOperation = "*"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", true)), parameterExpression);
                        ParameterOperation = "*"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", true)), Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", false)));
                    break;
                case "/":
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Divide(parameterExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", false)));
                        ParameterOperation = "/"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", true)), parameterExpression);
                        ParameterOperation = "/"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", true)), Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", false)));
                    break;
                default:
                    if (variablePlace == 0)
                    {
                        binaryExpression = Expression.Add(parameterExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                        ParameterOperation = "+"; IsParameterFirst = true;
                    }
                    else if (variablePlace == 1)
                    {
                        binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), parameterExpression);
                        ParameterOperation = "+"; IsParameterFirst = false;
                    }
                    else
                        binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                    break;
            }

            return binaryExpression;
        }

        private BinaryExpression ChainBinaryExpression(BinaryExpression binaryExpression, bool IsVariable)
        {
            string Operation = tasksOptions.Operations[RandomValuesGenerator.GenerateValuesInRange(0,tasksOptions.Operations.Count,false)];

            int placeFactor = RandomValuesGenerator.GenerateValuesInRange(0, 1, true);
       
            switch (Operation)
            {
                case "+":
                    if(placeFactor == 0)
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Add(binaryExpression, parameterExpression);
                            ParameterOperation = "+"; IsParameterFirst = false;
                        }
                        else
                            binaryExpression = Expression.Add(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Add(parameterExpression, binaryExpression);
                            ParameterOperation = "+"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), binaryExpression);
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
                            binaryExpression = Expression.Subtract(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", false)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Subtract(parameterExpression, binaryExpression);
                            ParameterOperation = "-"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Subtract(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("-", true)), binaryExpression);
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
                            binaryExpression = Expression.Multiply(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", false)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Multiply(parameterExpression, binaryExpression);
                            ParameterOperation = "*"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Multiply(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("*", true)), binaryExpression);
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
                            binaryExpression = Expression.Divide(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", false)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Divide(parameterExpression, binaryExpression);
                            ParameterOperation = "/"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Divide(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("/", true)), binaryExpression);
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
                            binaryExpression = Expression.Add(binaryExpression, Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", false)));
                    }
                    else
                    {
                        if (IsVariable)
                        {
                            binaryExpression = Expression.Add(parameterExpression, binaryExpression);
                            ParameterOperation = "+"; IsParameterFirst = true;
                        }
                        else
                            binaryExpression = Expression.Add(Expression.Constant(RandomValuesGenerator.GenerateRandomValue("+", true)), binaryExpression);
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
            return Result.ToString();
        }

        public override bool CheckAnswer(string Answer)
        {
            #region
            /*
            if (Convert.ToInt32(Answer) == XValue)
                return true;
            else
                return false;
                */
            #endregion

            int AnswerInt = Int32.Parse(Answer);
            if (expression.Compile().Invoke(AnswerInt) == Result)
                return true;
            else
                return false;
        }
    }
}
