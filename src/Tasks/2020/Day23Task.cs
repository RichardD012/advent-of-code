namespace AdventCode.Tasks2020;


public class Day23Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 23;
    private readonly ILogger<Day23Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day23Task(IAdventWebClient client, ILogger<Day23Task> logger) : base(client)
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
