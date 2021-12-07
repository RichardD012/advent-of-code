namespace AdventCode.Tasks2020;


public class Day8Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 8;
    private readonly ILogger<Day8Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day8Task(IAdventWebClient client, ILogger<Day8Task> logger) : base(client)
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
