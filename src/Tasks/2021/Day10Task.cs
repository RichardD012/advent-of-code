namespace AdventCode.Tasks2021;


public class Day10Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 10;
    private readonly ILogger<Day10Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day10Task(IAdventWebClient client, ILogger<Day10Task> logger) : base(client)
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
