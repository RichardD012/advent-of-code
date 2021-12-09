namespace AdventCode.Tasks2021;


public class Day19Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 19;
    private readonly ILogger<Day19Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day19Task(IAdventWebClient client, ILogger<Day19Task> logger) : base(client)
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
