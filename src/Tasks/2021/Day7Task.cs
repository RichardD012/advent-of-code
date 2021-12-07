namespace AdventCode.Tasks2021;

public class Day7Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 7;
    private readonly ILogger<Day7Task> _logger;
    #region TestData
    protected override string TestData => @"16,1,2,0,4,2,7,1,2,14";
    #endregion

    public Day7Task(IAdventWebClient client, ILogger<Day7Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var testData = (await GetDataAsync()).Split(",").Select(x => int.Parse(x));
        var min = testData.Min();
        var max = testData.Max();
        int? leastMoves = null;
        for (var i = min; i <= max; i++)
        {
            int moves = 0;
            foreach (var entry in testData)
            {
                moves += Math.Abs(entry - i);
            }
            if (leastMoves == null || moves < leastMoves)
            {
                leastMoves = moves;
            }
        }
        return leastMoves.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var testData = (await GetDataAsync()).Split(",").Select(x => int.Parse(x));
        var min = testData.Min();
        var max = testData.Max();
        int? leastMoves = null;
        for (var i = min; i <= max; i++)
        {
            int moves = 0;
            foreach (var entry in testData)
            {
                var steps = Math.Abs(entry - i);
                moves += CalculateMoves(steps);
            }
            if (leastMoves == null || moves < leastMoves)
            {
                leastMoves = moves;
            }
        }
        return leastMoves.ToString();
    }
    //There should probably be a better formula for this?
    private static int CalculateMoves(int steps)
    {
        int returnInt = 0;
        for (int i = 1; i <= steps; i++)
        {
            returnInt += i;
        }
        return returnInt;
    }
}
