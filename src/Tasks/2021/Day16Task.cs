namespace AdventCode.Tasks2021;


public class Day16Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 16;
    private readonly ILogger<Day16Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day16Task(IAdventWebClient client, ILogger<Day16Task> logger) : base(client)
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
