using System;
using System.Linq;
using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;

namespace AdventCode.Tasks;

/// <summary>
/// Day1s Test Tasks
/// </summary>
public class Day1Task : BaseCodeTask, IAdventCodeTask
{
    private readonly ILogger<Day1Task> _logger;
    public override int TaskDay => 1;
    #region TestData
    protected override string TestData => @"199
200
208
210
200
207
240
269
260
263";
    #endregion;

    public Day1Task(IAdventWebClient client, ILogger<Day1Task> logger) : base(client)
    {
        _logger = logger;
    }
    //Correct answer is:1228
    public override async Task<string?> GetFirstTaskAnswer()
    {
        var inputLines = await GetDataAsList<int>();
        int numberIncreases = 0, numberDecreases = 0;

        //traditional solution
        for (var i = 0; i < inputLines.Count; i++)
        {
            if (i == 0) continue;
            var current = inputLines[i];
            var previous = inputLines[i - 1];
            if (current > previous)
            {
                numberIncreases++;
            }
            else if (previous > current)
            {
                numberDecreases++;
            }
        }
        //functional solution
        var sum = inputLines.Zip(inputLines.Skip(1), (first, second) => second > first ? 1 : 0).Sum();
        return sum.ToString();
    }
    //correct answer is: 1257
    public override async Task<string?> GetSecondTaskAnswer()
    {
        var inputLines = await GetDataAsList<int>();
        int numberIncreases = 0, numberDecreases = 0;
        for (var i = 0; i < inputLines.Count - 2; i++)
        {
            if (i == 0) continue;
            var current = GetWindowSum(inputLines[i], inputLines[i + 1], inputLines[i + 2]);
            var previous = GetWindowSum(inputLines[i - 1], inputLines[i], inputLines[i + 1]);
            if (current > previous)
            {
                numberIncreases++;
            }
            else if (previous > current)
            {
                numberDecreases++;
            }
        }
        return numberIncreases.ToString();
    }

    private static int GetWindowSum(int first, int second, int third) => first + second + third;
}
