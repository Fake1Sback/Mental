using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace Mental.Models
{
    public abstract class AbstractTaskType
    {
        protected MathTasksOptions tasksOptions;
        protected int ChainLength;
        protected RandomValuesGenerator RandomValuesGenerator;

        public int AmountOfCorrectAnswers;
        public int AmountOfWrongAnswers;

        public abstract void GenerateExpression();
        public abstract string GetExpressionString();
        public abstract bool CheckAnswer(string Answer);
        public abstract string GetResult();
    }
}
