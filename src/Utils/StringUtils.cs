using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

    public static bool EqualsIgnoreCase(this char? inChar, string? cmpString)
    {
        if (inChar == null && cmpString == null)
        {
            return true;
        }
        if (inChar == null && cmpString != null)
        {
            return false;
        }
        return inChar.EqualsIgnoreCase(cmpString);
    }

    public static bool EqualsIgnoreCase(this char inChar, string? cmpString)
    {
        return inChar.ToString()?.Equals(cmpString, StringComparison.CurrentCultureIgnoreCase) ?? false;
    }



    public static List<int> ToIntList(this string? inString)
    {
        if (string.IsNullOrEmpty(inString))
        {
            return new List<int>();
        }
        return inString.Split(new string[] { "\n" }, StringSplitOptions.TrimEntries).Select(x =>
                    int.TryParse(x, out var lineValue)
                     ? lineValue
                     : throw new ArgumentException($"Provided value \"{x}\" was not a number")).ToList();
    }

    public static List<string> ToStringList(this string? inString)
    {
        if (string.IsNullOrEmpty(inString))
        {
            return new List<string>();
        }
        return inString.Split(new string[] { "\n" }, StringSplitOptions.TrimEntries).ToList();
    }

    public static List<T> ToDataList<T>(this string? response)
    {
        if (response == null)
        {
            return new List<T>();
        }
        if (typeof(T) == typeof(int))
        {
            return response.ToIntList() as List<T> ?? new List<T>();
        }
        if (typeof(T) == typeof(string))
        {
            return response.ToStringList() as List<T> ?? new List<T>();
        }
        return new List<T>();
    }

}
