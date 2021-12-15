using System.Collections.Generic;

namespace AdventCode.Tasks2021;


public class Day15Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 15;
    private readonly ILogger<Day15Task> _logger;
    #region TestData
    protected override string TestData => @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";
    #endregion

    public Day15Task(IAdventWebClient client, ILogger<Day15Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var plot = GeneratePlot(data, 1, false);
        var path = AStar(plot, (0, 0), (plot.GetLength(1) - 1, plot.GetLength(0) - 1));
        return path?.Skip(1).Sum(x => plot[x.Item2, x.Item1]).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var plot = GeneratePlot(data, 5, false);
        var path = AStar(plot, (0, 0), (plot.GetLength(1) - 1, plot.GetLength(0) - 1));
        return path?.Skip(1).Sum(x => plot[x.Item2, x.Item1]).ToString();
    }

    //AStar Pathfinding Algorithm: https://en.wikipedia.org/wiki/A*_search_algorithm
    private List<(int, int)>? AStar(int[,] plot, (int, int) start, (int, int) goal)
    {
        var openSet = new PriorityQueue<(int, int), int>();
        var cameFrom = new Dictionary<(int, int), (int, int)>();

        var gScore = new Dictionary<(int, int), int>
        {
            [start] = 0
        };

        var fScore = new Dictionary<(int, int), int>
        {
            [start] = 0
        };
        openSet.Enqueue(start, fScore[start]);

        while (openSet.TryDequeue(out (int, int) cur, out int _))
        {
            //_logger.LogInformation("parsing {x},{y}", cur.Item1, cur.Item2);
            if (cur.Item1 == goal.Item1 && cur.Item2 == goal.Item2)
            {
                return GeneratePath(cameFrom, cur);
            }
            foreach (var entry in GetNeighbors(plot, cur))
            {
                var tentGScore = gScore[cur] + plot[entry.Item2, entry.Item1];
                if (tentGScore < gScore.GetValueOrDefault(entry, int.MaxValue))
                {
                    cameFrom[entry] = cur;
                    gScore[entry] = tentGScore;
                    fScore[entry] = tentGScore + Distance(cur, (plot.GetLength(1) - 1, plot.GetLength(0) - 1));
                    openSet.Enqueue(entry, fScore[entry]);
                }
            }
        }
        return null;
    }
    private static List<(int, int)> GeneratePath(Dictionary<(int, int), (int, int)> cameFrom, (int, int) current)
    {
        List<(int, int)> res = new();
        res.Add(current);
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            res.Add(current);
        }
        res.Reverse();
        return res;
    }
    //Manhattan distance
    private static int Distance((int, int) cur, (int, int) other)
    {
        int x = Math.Abs(cur.Item1 - other.Item1);
        int y = Math.Abs(cur.Item2 - other.Item2);
        return x + y;
    }

    private static List<(int, int)> GetNeighbors(int[,] plot, (int, int) current)
    {
        var returnList = new List<(int, int)>();

        if (current.Item2 > 0)
            returnList.Add((current.Item1, current.Item2 - 1));

        if (current.Item2 < plot.GetLength(0) - 1)
            returnList.Add((current.Item1, current.Item2 + 1));

        if (current.Item1 > 0)
            returnList.Add((current.Item1 - 1, current.Item2));
        if (current.Item1 < plot.GetLength(1) - 1)
            returnList.Add((current.Item1 + 1, current.Item2));
        return returnList;
    }

    private static int[,] GeneratePlot(List<string> data, int grid, bool print)
    {
        var plot = new int[data.Count * grid, data[0].Length * grid];
        for (var y = 0; y < data.Count * grid; y++)
        {
            for (var x = 0; x < data.Count * grid; x++)
            {
                var actualY = y - ((y / data.Count) * data.Count);
                var actualX = x - ((x / data[0].Length) * data[0].Length);
                var parsedInt = int.Parse(data[actualY][actualX].ToString()) + (y / data.Count) + (x / data[0].Length);
                plot[y, x] = parsedInt > 9 ? parsedInt - 9 : parsedInt;
            }
        }
        plot[0, 0] = 0;
        return plot;
    }
}
