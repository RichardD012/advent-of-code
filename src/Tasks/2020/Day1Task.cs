using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;
using AdventCode.Tasks;

namespace AdventCode.Tasks2020;

/// <summary>
/// Boilerplate Task for Future Days
/// </summary>
public class Day1Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 1;
    private readonly ILogger<Day1Task> _logger;
    #region TestData
    protected override string TestData => @"1721
979
366
299
675
1456";
    #endregion;
    public Day1Task(IAdventWebClient client, ILogger<Day1Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<int>();
        for (var i = 0; i < data.Count - 1; i++)
        {
            for (var n = i + 1; n < data.Count; n++)
            {
                if (data[i] + data[n] == 2020)
                {
                    return (data[i] * data[n]).ToString();
                }
            }
        }
        throw new InvalidAnswerException();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<int>();
        for (var i = 0; i < data.Count - 2; i++)
        {
            for (var n = i + 1; n < data.Count - 1; n++)
            {
                for (var z = n + 1; z < data.Count; z++)
                {
                    if (data[i] + data[n] + data[z] == 2020)
                    {
                        return (data[i] * data[n] * data[z]).ToString();
                    }
                }
            }
        }
        throw new InvalidAnswerException();
    }
}
