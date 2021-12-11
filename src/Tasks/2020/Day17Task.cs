namespace AdventCode.Tasks2020;


public class Day17Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 17;
    private readonly ILogger<Day17Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day17Task(IAdventWebClient client, ILogger<Day17Task> logger) : base(client)
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
