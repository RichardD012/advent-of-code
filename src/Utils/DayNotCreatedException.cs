using System;

namespace AdventCode.Utils;

public class DayNotCreatedException : Exception
{
    public int TaskDay { get; private set; }
    public DayNotCreatedException(int day)
    {
        TaskDay = day;
    }

}
