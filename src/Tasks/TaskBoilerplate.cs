using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;

namespace AdventCode.Tasks;

/// <summary>
/// Boilerplate Task for Future Days
/// </summary>
public class DayXTask : BaseCodeTask, IAdventCodeTask
{
    public int TaskDay => 0;
    private readonly ILogger<DayXTask> _logger;
    public DayXTask(IAdventWebClient client, ILogger<DayXTask> logger) : base(client)
    {
        _logger = logger;
    }

    public Task<string?> GetFirstTaskAnswer()
    {
        throw new TaskIncompleteException();
    }

    public Task<string?> GetSecondTaskAnswer()
    {
        throw new TaskIncompleteException();
    }
}
