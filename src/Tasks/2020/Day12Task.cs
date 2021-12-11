namespace AdventCode.Tasks2020;


public class Day12Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 12;
    private readonly ILogger<Day12Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day12Task(IAdventWebClient client, ILogger<Day12Task> logger) : base(client)
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
