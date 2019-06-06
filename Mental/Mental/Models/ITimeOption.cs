using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public interface ITimeOption
    {
        void StartTimer();
        string GetTime();
        bool CanExecuteOperation();
    }
}
