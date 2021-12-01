using System;
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

    public int TaskDay => 1;

    public Day1Task(IAdventWebClient client, ILogger<Day1Task> logger) : base(client)
    {
        _logger = logger;
    }

    public async Task<string?> GetFirstTaskAnswer()
    {
        var dataInput = await GetData(TaskDay);
        var inputLines = dataInput.Split("\n");
        int numberIncreases = 0, numberDecreases = 0;
        for (var i = 0; i < inputLines.Length; i++)
        {
            if (i == 0) continue;
            var current = int.TryParse(inputLines[i], out var lineNubmer) ? lineNubmer : throw new ArgumentException("Provided value was not a number");
            var previous = int.TryParse(inputLines[i - 1], out var prevLineNumber) ? prevLineNumber : throw new ArgumentException("Provided previous value was not a number");
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

    public async Task<string?> GetSecondTaskAnswer()
    {
        var dataInput = await GetData(TaskDay);
        var inputLines = dataInput.Split("\n");
        int numberIncreases = 0, numberDecreases = 0;
        for (var i = 0; i < inputLines.Length - 2; i++)
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

    private static int GetWindowSum(string first, string second, string third) => (int.TryParse(first, out var firstInt) ? firstInt : throw new ArgumentException("First string was not a number")) +
            (int.TryParse(second, out var secondInt) ? secondInt : throw new ArgumentException("Second string was not a number")) +
            (int.TryParse(third, out var thirdInt) ? thirdInt : throw new ArgumentException("Third string was not a number"));
}
