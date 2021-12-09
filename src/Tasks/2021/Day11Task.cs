namespace AdventCode.Tasks2021;


public class Day11Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 11;
    private readonly ILogger<Day11Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day11Task(IAdventWebClient client, ILogger<Day11Task> logger) : base(client)
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
