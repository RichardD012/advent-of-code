using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventCode.Utils;

public static class AdventUtils
{
    private readonly static int? YearOverride = null;
    public static int GetCurrentYear()
    {
        if (YearOverride != null)
            return YearOverride.Value;
        var now = DateTime.Now;
        var currentYear = now.Year;
        if (now.Month != 12)
        {
            currentYear--;
        }
        return currentYear;
    }

}
