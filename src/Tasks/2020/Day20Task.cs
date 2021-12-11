namespace AdventCode.Tasks2020;


public class Day20Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 20;
    private readonly ILogger<Day20Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day20Task(IAdventWebClient client, ILogger<Day20Task> logger) : base(client)
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
