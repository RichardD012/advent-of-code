namespace AdventCode.Tasks2021;

public class Day5Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 5;
    private readonly ILogger<Day5Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day5Task(IAdventWebClient client, ILogger<Day5Task> logger) : base(client)
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
        _ = await GetDataAsListAsync<string>();
        throw new TaskIncompleteException();
    }
}
