using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventCode.Utils;

public static class AdventUtils
{
#if DEBUG
    //manual override for testing upcoming days when you may not have the automatic day detection in effect
    private static readonly int? DayOverride = 5;
    private readonly static int? YearOverride = 2020;
#endif
    private static int? CurrentYear = null;
    public static int GetCurrentYear()
    {
        if (CurrentYear != null)
        {
            return CurrentYear.Value;
        }
#if DEBUG
        if (YearOverride != null)
        {
            CurrentYear = YearOverride.Value;
            return YearOverride.Value;
        }
        var now = DateTime.Now;
        var currentYear = now.Year;
        if (now.Month != 12)
        {
            currentYear--;
        }
        CurrentYear = currentYear;
#else
        var correctYear = 0;
        var correctInput = false;
        do
        {
            Console.Write("Please Enter the Year: ");
            var day = Console.ReadLine();
            if ((int.TryParse(day, out var parsedDay) == false))
            {
                Console.WriteLine("Please only enter a valid number");
            }
            else
            {
                correctInput = true;
                correctYear = parsedDay;
            }
        } while (correctInput == false);
        CurrentYear = correctYear;
#endif
        return CurrentYear.Value;
    }

    public static int GetDay()
    {
#if DEBUG
#pragma warning disable CS0162
        if (DayOverride != null && DayOverride >= 1 && DayOverride <= 25)
            return DayOverride.Value;
        var currentTime = DateTime.Now;
        var convertedTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(currentTime, TimeZoneInfo.Local.Id, "Eastern Standard Time");
        return convertedTime.Day <= 25 ? convertedTime.Day : 25;
#pragma warning restore CS0162
#else
        var correctDay = 0;
        var correctInput = false;
        do
        {
            Console.Write("Please Enter the Day: ");
            var day = Console.ReadLine();
            if ((int.TryParse(day, out var parsedDay) == false) || (parsedDay < 1 || parsedDay > 25))
            {
                Console.WriteLine("Please only enter a number 1-25");
            }
            else
            {
                correctInput = true;
                correctDay = parsedDay;
            }
        } while (correctInput == false);
        return correctDay;
#endif
    }

}
