namespace AdventCode.Tasks2020;

public class Day3Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 3;
    private readonly ILogger<Day3Task> _logger;
    #region TestData
    protected override string TestData => @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#";
    #endregion

    public Day3Task(IAdventWebClient client, ILogger<Day3Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var input = await GetDataAsListAsync<string>();
        var treeCount = 0;
        int positionX = 0;
        for (int positionY = 0; positionY < input.Count; positionY++)
        {
            if (input[positionY][positionX].ToString().EqualsIgnoreCase("#"))
            {
                treeCount++;
            }
            positionX += 3;
            if (positionX >= input[positionY].Length)
            {
                positionX -= input[positionY].Length;
            }
        }
        return treeCount.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var input = await GetDataAsListAsync<string>();
        var treeCount = 1;
        var slopes = new List<(int, int)>() { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
        foreach (var slope in slopes)
        {
            int positionX = 0;
            var localCount = 0;
            for (int positionY = 0; positionY < input.Count; positionY += slope.Item2)
            {
                if (input[positionY][positionX].ToString().EqualsIgnoreCase("#"))
                {
                    localCount++;
                }
                positionX += slope.Item1;
                if (positionX >= input[positionY].Length)
                {
                    positionX -= input[positionY].Length;
                }
            }
            treeCount *= localCount;
        }

        return treeCount.ToString();
    }
}
