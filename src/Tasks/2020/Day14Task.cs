namespace AdventCode.Tasks2020;


public class Day14Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 14;
    private readonly ILogger<Day14Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day14Task(IAdventWebClient client, ILogger<Day14Task> logger) : base(client)
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
