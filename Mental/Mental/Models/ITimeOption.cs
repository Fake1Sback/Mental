using System;
using System.Collections.Generic;
using System.Text;

namespace Mental.Models
{
    public interface ITimeOption
    {
        void TimerWork();

        void TimerRestart();
        bool CheckTimerEnd();
        string GetTimeString();
        int GetMillis();
        bool CanExecuteOperation(bool IsAnswerCorrect);
    }
}
