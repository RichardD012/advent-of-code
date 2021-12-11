namespace AdventCode.Tasks2020;


public class Day21Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 21;
    private readonly ILogger<Day21Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day21Task(IAdventWebClient client, ILogger<Day21Task> logger) : base(client)
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
