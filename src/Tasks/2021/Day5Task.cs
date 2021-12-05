using Microsoft.AspNetCore.Hosting;

namespace AdventCode.Tasks2021;

public class Day5Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 5;
    private readonly ILogger<Day5Task> _logger;
    #region TestData
    protected override string TestData => @"0,9 -> 5,9
8,0 -> 0,8
9,4 -> 3,4
2,2 -> 2,1
7,0 -> 7,4
6,4 -> 2,0
0,9 -> 2,9
3,4 -> 1,4
0,0 -> 8,8
5,5 -> 8,2";
    #endregion

    public Day5Task(IAdventWebClient client, ILogger<Day5Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = (await GetDataAsListAsync<string>()).Select(GenerateVent);
        var numCols = data.Max(x => x.MaxX) + 1;
        var numRows = data.Max(y => y.MaxY) + 1;
        var grid = new int[numRows, numCols];
        foreach (var vent in data)
        {
            if (vent.X1 == vent.X2 || vent.Y1 == vent.Y2)
            {
                grid = PopulateVent(grid, vent);
            }
        }

        return DetermineDangerousPoints(grid).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = (await GetDataAsListAsync<string>()).Select(GenerateVent);
        var numCols = data.Max(x => x.MaxX) + 1;
        var numRows = data.Max(y => y.MaxY) + 1;
        var grid = new int[numRows, numCols];
        foreach (var vent in data)
        {
            grid = PopulateVent(grid, vent);
        }
        return DetermineDangerousPoints(grid).ToString();
    }

    private static int[,] PopulateVent(int[,] grid, Vent vent)
    {
        var xRemaining = Math.Abs(vent.X2 - vent.X1);
        var yRemaining = Math.Abs(vent.Y2 - vent.Y1);
        if (xRemaining != 0)
        {
            xRemaining++; //include the end point otherwise you only will "mark" the first xCoordinate
        }
        if (yRemaining != 0)
        {
            yRemaining++; //include the end point otherwise you only will "mark" the first yCoordinate
        }
        var xPos = vent.X1;
        var yPos = vent.Y1;
        do
        {
            grid[yPos, xPos]++;
            if (xRemaining > 0)
            {
                xPos += vent.X1 > vent.X2 ? -1 : 1;
            }
            if (yRemaining > 0)
            {
                yPos += vent.Y1 > vent.Y2 ? -1 : 1;
            }
            xRemaining--;
            yRemaining--;
        } while (xRemaining > 0 || yRemaining > 0);
        return grid;
    }

    private static int DetermineDangerousPoints(int[,] grid)
    {
        var dangerousPoints = 0;
        for (var row = 0; row < grid.GetLength(0); row++)
        {
            for (var col = 0; col < grid.GetLength(1); col++)
            {
                dangerousPoints += grid[row, col] > 1 ? 1 : 0;
            }
        }
        return dangerousPoints;
    }

    private Vent GenerateVent(string input)
    {
        var inputParts = input.Split(" -> ");
        var first = inputParts[0].Split(",");
        var second = inputParts[1].Split(",");
        return new Vent(int.Parse(first[0]), int.Parse(first[1]), int.Parse(second[0]), int.Parse(second[1]));
    }

    private class Vent
    {
        public int X1 { get; private set; }
        public int Y1 { get; private set; }
        public int X2 { get; private set; }
        public int Y2 { get; private set; }

        public int MaxX => X2 > X1 ? X2 : X1;
        public int MaxY => Y2 > Y1 ? Y2 : Y1;

        public Vent(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public override string ToString()
        {
            return $"{X1},{Y1} -> {X2},{Y2}";
        }
    }
}
