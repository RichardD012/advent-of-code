namespace AdventCode.Tasks2021;


public class Day13Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 13;
    private readonly ILogger<Day13Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day13Task(IAdventWebClient client, ILogger<Day13Task> logger) : base(client)
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
