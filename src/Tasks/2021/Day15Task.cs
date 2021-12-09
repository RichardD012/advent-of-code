namespace AdventCode.Tasks2021;


public class Day15Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 15;
    private readonly ILogger<Day15Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day15Task(IAdventWebClient client, ILogger<Day15Task> logger) : base(client)
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
