using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models.DbModels
{
    public class DbStroopTaskOptions
    {
        public int Id { get; set; }
        public int ButtonsAmount { get; set; }
        public byte StroopTaskType { get; set; }

        public byte TimeOptions { get; set; }
        public int AmountOfMinutes { get; set; }
        public int AmountOfTasks { get; set; }
        public int AmountOfSecondsForAnswer { get; set; }

        public bool IsLatestTaskOption { get; set; }
    }
}
