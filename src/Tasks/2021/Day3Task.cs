using System;
using System.Linq;
using System.Threading.Tasks;
using AdventCode.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;

namespace AdventCode.Tasks2021;


public class Day3Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 3;
    private readonly ILogger<Day3Task> _logger;
    #region TestData
    protected override string TestData => @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010";
    #endregion;
    public Day3Task(IAdventWebClient client, ILogger<Day3Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var input = await GetDataAsListAsync<string>();
        var gammaString = string.Empty;
        var epsilonString = string.Empty;
        for (int positionX = 0; positionX < input[0].Length; positionX++)
        {
            var ones = 0;
            for (int positionY = 0; positionY < input.Count; positionY++)
            {
                if (input[positionY][positionX].EqualsIgnoreCase("1"))
                {
                    ones++;
                }
            }
            var zeros = input.Count - ones;
            gammaString += ones > zeros ? "1" : "0";
            epsilonString += ones > zeros ? "0" : "1";
        }
        return (Convert.ToInt32(gammaString, 2) * Convert.ToInt32(epsilonString, 2)).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var count = new int[data[0].Length];
        for (int positionX = 0; positionX < data[0].Length; positionX++)
        {
            for (int positionY = 0; positionY < data.Count; positionY++)
            {
                if (data[positionY][positionX].EqualsIgnoreCase("1"))
                {
                    count[positionX]++;
                }
            }
            //resize data based on the currentPositionX;
            var ones = count[positionX];
            var zeros = data.Count - count[positionX];
            if (ones >= zeros)
            {
                data = data.Where(x => x[positionX].EqualsIgnoreCase("1")).ToList();
            }
            else
            {
                data = data.Where(x => x[positionX].EqualsIgnoreCase("0")).ToList();
            }
            if (data.Count == 1)
            {
                break;
            }
        }
        var oxygen = data[0];
        data = await GetDataAsListAsync<string>();
        count = new int[data[0].Length];
        for (int positionX = 0; positionX < data[0].Length; positionX++)
        {
            for (int positionY = 0; positionY < data.Count; positionY++)
            {
                if (data[positionY][positionX].EqualsIgnoreCase("1"))
                {
                    count[positionX]++;
                }
            }
            //resize data based on the currentPositionX;
            var ones = count[positionX];
            var zeros = data.Count - count[positionX];
            if (ones >= zeros)
            {
                data = data.Where(x => x[positionX].EqualsIgnoreCase("0")).ToList();
            }
            else
            {
                data = data.Where(x => x[positionX].EqualsIgnoreCase("1")).ToList();
            }
            if (data.Count == 1)
            {
                break;
            }
        }
        var scrubber = data[0];
        return (Convert.ToInt32(oxygen, 2) * Convert.ToInt32(scrubber, 2)).ToString();
    }
}
