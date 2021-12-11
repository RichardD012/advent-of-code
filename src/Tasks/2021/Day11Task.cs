namespace AdventCode.Tasks2021;


public class Day11Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 11;
    private readonly ILogger<Day11Task> _logger;
    #region TestData
    protected override string TestData => @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";
    #endregion

    public Day11Task(IAdventWebClient client, ILogger<Day11Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var grid = GenerateGrid(data);
        var flashes = 0;
        for (var i = 0; i < 100; i++)
        {
            var (stepFlashes, _) = IterateGridForStep(grid);
            flashes += stepFlashes;
        }
        return flashes.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var grid = GenerateGrid(data);
        var step = 0;
        var allFlashed = false;
        while (allFlashed == false)
        {
            var (_, seen) = IterateGridForStep(grid);
            step++;
            var flashed = true;
            for (var y = 0; y < grid.GetLength(0); y++)
            {
                for (var x = 0; x < grid.GetLength(1); x++)
                {
                    flashed = flashed && seen[y, x];
                }
            }
            allFlashed = flashed;
        }
        return step.ToString();
    }
    private static (int, bool[,]) IterateGridForStep(int[,] grid)
    {
        var seen = new bool[grid.GetLength(0), grid.GetLength(1)];
        int count = 0;
        for (var y = 0; y < grid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.GetLength(1); x++)
            {
                count += IterateFlash(grid, seen, y, x);
            }
        }
        return (count, seen);
    }

    private static int[,] GenerateGrid(List<string> data)
    {
        //assume all of the grid is same size
        var returnGrid = new int[data.Count, data[0].Length];
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[y].Length; x++)
            {
                returnGrid[y, x] = int.Parse(data[y][x].ToString());
            }
        }
        return returnGrid;
    }

    private static int IterateFlash(int[,] grid, bool[,] seen, int y, int x)
    {
        if (y < 0 || x < 0 || y >= grid.GetLength(0) || x >= grid.GetLength(1))
        {
            return 0;
        }

        if (seen[y, x] == true)
        {
            return 0;
        }
        if (grid[y, x] == 9)
        {
            grid[y, x] = 0;
            seen[y, x] = true;
            return 1 + IterateFlash(grid, seen, y - 1, x) + IterateFlash(grid, seen, y, x - 1) + IterateFlash(grid, seen, y, x + 1) + IterateFlash(grid, seen, y + 1, x) +
                IterateFlash(grid, seen, y - 1, x - 1) + IterateFlash(grid, seen, y - 1, x + 1) + IterateFlash(grid, seen, y + 1, x - 1) + IterateFlash(grid, seen, y + 1, x + 1);
        }
        grid[y, x]++;
        return 0;
    }
}
