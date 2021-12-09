namespace AdventCode.Tasks2021;


public class Day9Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 9;
    private readonly ILogger<Day9Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day9Task(IAdventWebClient client, ILogger<Day9Task> logger) : base(client)
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
