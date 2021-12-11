namespace AdventCode.Tasks2020;


public class Day18Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 18;
    private readonly ILogger<Day18Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day18Task(IAdventWebClient client, ILogger<Day18Task> logger) : base(client)
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
