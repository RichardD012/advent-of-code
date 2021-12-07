namespace AdventCode.Tasks2020;

public class Day7Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 7;
    private readonly ILogger<Day7Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day7Task(IAdventWebClient client, ILogger<Day7Task> logger) : base(client)
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
