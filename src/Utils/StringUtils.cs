using System;

namespace AdventCode.Utils;

public static class StringUtils
{
    public static bool EqualsIgnoreCase(this string? inString, string? cmpString)
    {
        if (inString == null && cmpString == null)
        {
            return true;
        }
        if (inString == null && cmpString != null)
        {
            return false;
        }
        return inString != null && inString.Equals(cmpString, StringComparison.CurrentCultureIgnoreCase);
    }

}
