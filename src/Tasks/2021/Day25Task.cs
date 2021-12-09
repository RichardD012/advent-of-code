namespace AdventCode.Tasks2021;


public class Day25Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 25;
    private readonly ILogger<Day25Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day25Task(IAdventWebClient client, ILogger<Day25Task> logger) : base(client)
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
