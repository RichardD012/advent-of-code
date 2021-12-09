namespace AdventCode.Tasks2021;


public class Day22Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 22;
    private readonly ILogger<Day22Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day22Task(IAdventWebClient client, ILogger<Day22Task> logger) : base(client)
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
