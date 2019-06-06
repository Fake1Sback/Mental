using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public abstract class GenericAbstractTaskType
    {
        protected MathTasksOptions tasksOptions;
        protected int ChainLength;
        protected RandomValuesGenerator RandomValuesGenerator;
        protected ExpressionValuesGenerator ExpressionValuesGenerator;
        //protected Random random;

        public int AmountOfCorrectAnswers;
        public int AmountOfWrongAnswers;

        public abstract void GenerateExpression();
        public abstract string GetExpressionString();
        public abstract bool CheckAnswer(int Answer);
        public abstract string GetResult();
    }
}
