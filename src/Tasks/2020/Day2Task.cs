using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;
using AdventCode.Tasks;

namespace AdventCode.Tasks2020;

/// <summary>
/// Boilerplate Task for Future Days
/// </summary>
public class Day2Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 2;
    private readonly ILogger<Day2Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion;
    public Day2Task(IAdventWebClient client, ILogger<Day2Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override Task<string?> GetFirstTaskAnswer()
    {
        throw new TaskIncompleteException();
    }

    public override Task<string?> GetSecondTaskAnswer()
    {
        throw new TaskIncompleteException();
    }
}
