namespace AdventCode.Tasks2021;

public class Day9Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 9;
    private readonly ILogger<Day9Task> _logger;
    #region TestData
    protected override string TestData => @"2199943210
3987894921
9856789892
8767896789
9899965678";
    #endregion

    public Day9Task(IAdventWebClient client, ILogger<Day9Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var lowestPoints = GenerateLowestPoints(data);
        return lowestPoints.Select(x => int.Parse(data[x.Item1][x.Item2].ToString()) + 1).Sum().ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var lowestPoints = GenerateLowestPoints(data);
        var basinSizes = new List<int>();
        foreach (var point in lowestPoints)
        {
            basinSizes.Add(CalculateBasinSize(data, new bool[data.Count, data[0].Length], point.Item1, point.Item2));
        }
        return basinSizes.OrderByDescending(x => x).Take(3).Aggregate((a, x) => a * x).ToString();
    }

    private static List<(int, int)> GenerateLowestPoints(List<string> data)
    {
        var lowestPoints = new List<(int, int)>();
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[y].Length; x++)
            {
                var otherPoints = new List<int>();
                var thisPoint = int.Parse(data[y][x].ToString());
                if (y > 0)
                {
                    otherPoints.Add(int.Parse(data[y - 1][x].ToString()));
                }
                if (x > 0)
                {
                    otherPoints.Add(int.Parse(data[y][x - 1].ToString()));
                }
                if (x < data[y].Length - 1)
                {
                    otherPoints.Add(int.Parse(data[y][x + 1].ToString()));
                }
                if (y < data.Count - 1)
                {
                    otherPoints.Add(int.Parse(data[y + 1][x].ToString()));
                }
                if (otherPoints.Any(x => x <= thisPoint) == false)
                {
                    lowestPoints.Add((y, x));
                }
            }
        }
        return lowestPoints;
    }

    private static int CalculateBasinSize(List<string> data, bool[,] seen, int y, int x)
    {
        if (y < 0 || x < 0 || y >= data.Count || x >= data[y].Length)
        {
            return 0;
        }
        if (int.Parse(data[y][x].ToString()) == 9)
        {
            return 0;
        }
        if (seen[y, x] == true)
        {
            return 0;
        }
        seen[y, x] = true;
        return 1 + CalculateBasinSize(data, seen, y - 1, x) + CalculateBasinSize(data, seen, y, x - 1) + CalculateBasinSize(data, seen, y, x + 1) + CalculateBasinSize(data, seen, y + 1, x);
    }
}
