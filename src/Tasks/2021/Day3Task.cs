using System.Threading.Tasks;
using AdventCode.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;

namespace AdventCode.AdventCode.Tasks2021;


public class Day3Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 3;
    private readonly ILogger<Day3Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion;
    public Day3Task(IAdventWebClient client, ILogger<Day3Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        throw new TaskIncompleteException();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        throw new TaskIncompleteException();
    }
}
