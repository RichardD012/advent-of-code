using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;
using AdventCode.Tasks;

namespace AdventCode.TasksYYYY;

/// <summary>
/// Boilerplate Task for Future Days
/// </summary>
public class DayXTask : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 0;
    private readonly ILogger<DayXTask> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public DayXTask(IAdventWebClient client, ILogger<DayXTask> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        _ = await GetDataAsListAsync<string>();
        throw new TaskIncompleteException();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        _ = await GetDataAsListAsync<string>();
        throw new TaskIncompleteException();
    }
}
