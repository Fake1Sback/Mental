﻿using System;
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
        protected ExpressionValuesGenerator ExpressionValuesGenerator;
        //protected Random random;

        public int AmountOfCorrectAnswers;
        public int AmountOfWrongAnswers;

        public abstract void GenerateExpression();
        public abstract string GetExpressionString();
        //  public abstract bool CheckAnswer(int Answer);
        public abstract bool CheckAnswer(string Answer);
        public abstract string GetResult();
    }
}
