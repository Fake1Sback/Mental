using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models.DbModels
{
    public class DbSchulteTableTaskOptions
    {
        public int Id { get; set; }
        public int GridSize { get; set; }
        public bool IsEasyModeActivated { get; set; }
        public byte TimeOptions { get; set; }
        public int AmountOfMinutes { get; set; }
        public int AmountOfTasks { get; set; }
        public int AmountOfSecondsForAnswer { get; set; }
    }
}
