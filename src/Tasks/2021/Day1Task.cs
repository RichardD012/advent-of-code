using System;
using System.Linq;
using System.Threading.Tasks;
using AdventCode.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;

namespace AdventCode.Tasks2021;

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

    public override async Task<string?> GetSecondTaskAnswer()
    {
        var inputLines = await GetDataAsList<int>();
        int numberIncreases = 0, numberDecreases = 0;
        //Traditional walk the array
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
        // More consice way compare
        var sum = inputLines.Skip(3).Where((depth, index) =>
        {
            /** 
            *   This workes because what this is technically comparing on the 
            *   first iteration is inputLines[3] > inputLines[0].
            *   If you look at the problem set, each grouping is technically
            *   only comparing the first element of the first group, and the last
            *   element of the second group becaues the other two
            *   elements overlap in each group. Therefore the only difference
            *   is the last vs the first.
            */
            return depth > inputLines[index];
        }).Count();
        return sum.ToString();
    }

    private static int GetWindowSum(int first, int second, int third) => first + second + third;
}
