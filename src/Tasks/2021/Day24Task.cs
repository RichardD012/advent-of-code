namespace AdventCode.Tasks2021;


public class Day24Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 24;
    private readonly ILogger<Day24Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day24Task(IAdventWebClient client, ILogger<Day24Task> logger) : base(client)
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
