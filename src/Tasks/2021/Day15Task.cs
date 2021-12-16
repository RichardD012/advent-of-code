using System.Collections.Generic;
using System.Reflection;

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
        var plot = GeneratePlot(data, 1);
        var path = Dijkstra(plot, (0, 0), (plot.GetLength(1) - 1, plot.GetLength(0) - 1));
        return path?.Skip(1).Sum(x => plot[x.Item2, x.Item1]).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var plot = GeneratePlot(data, 5);
        var path = AStar(plot, (0, 0), (plot.GetLength(1) - 1, plot.GetLength(0) - 1));
        return path?.Skip(1).Sum(x => plot[x.Item2, x.Item1]).ToString();
    }
    private static List<(int, int)>? Dijkstra(int[,] plot, (int, int) start, (int, int) goal)
    {
        var previous = new Dictionary<(int, int), (int, int)>();
        var distance = new Dictionary<(int, int), int>();
        var vertexSet = new List<(int, int)>();
        for (var y = 0; y < plot.GetLength(0); y++)
        {
            for (var x = 0; x < plot.GetLength(1); x++)
            {
                var newVertex = (x, y);
                distance[newVertex] = int.MaxValue;
                vertexSet.Add(newVertex);
            }
        }
        var source = vertexSet.FirstOrDefault(x => x.Item1 == start.Item1 && x.Item2 == start.Item2);
        distance[source] = 0;
        while (vertexSet.Any())
        {
            var current = GetMinVertexDistance(vertexSet, distance);
            if (current.Item1 == goal.Item1 && current.Item2 == goal.Item2)
            {
                return GeneratePath(previous, current);
            }
            var result = vertexSet.Remove(current);
            foreach (var neighbor in GetUnvisitedNeighbors(plot, vertexSet, current))
            {
                var currentDistance = distance[current] + plot[neighbor.Item2, neighbor.Item1];
                if (currentDistance < distance[neighbor])
                {
                    distance[neighbor] = currentDistance;
                    previous[neighbor] = current;
                }
            }
        }
        return null;
    }

    private static (int, int) GetMinVertexDistance(List<(int, int)> vertexSet, Dictionary<(int, int), int> distance)
    {
        int? minDistance = null;
        (int, int) minVertex = (-1, -1);
        foreach (var entry in vertexSet)
        {
            if (distance.ContainsKey(entry))
            {
                if (minDistance == null || distance[entry] < minDistance)
                {
                    minDistance = distance[entry];
                    minVertex = entry;
                }
            }
        }
        return minVertex;
    }

    //AStar Pathfinding Algorithm: https://en.wikipedia.org/wiki/A*_search_algorithm
    private static List<(int, int)>? AStar(int[,] plot, (int, int) start, (int, int) goal)
    {
        var priorityQueue = new PriorityQueue<(int, int), int>();
        var prev = new Dictionary<(int, int), (int, int)>();

        var gScore = new Dictionary<(int, int), int>
        {
            [start] = 0
        };

        var fScore = new Dictionary<(int, int), int>
        {
            [start] = 0
        };
        priorityQueue.Enqueue(start, fScore[start]);

        while (priorityQueue.TryDequeue(out (int, int) current, out int _))
        {
            if (current.Item1 == goal.Item1 && current.Item2 == goal.Item2)
            {
                return GeneratePath(prev, current);
            }
            foreach (var entry in GetNeighbors(plot, current))
            {
                var tentGScore = gScore[current] + plot[entry.Item2, entry.Item1];
                if (tentGScore < gScore.GetValueOrDefault(entry, int.MaxValue))
                {
                    prev[entry] = current;
                    gScore[entry] = tentGScore;
                    fScore[entry] = tentGScore + Distance(current, (plot.GetLength(1) - 1, plot.GetLength(0) - 1));
                    priorityQueue.Enqueue(entry, fScore[entry]);
                }
            }
        }
        return null;
    }
    private static List<(int, int)> GeneratePath(Dictionary<(int, int), (int, int)> cameFrom, (int, int) current)
    {
        var res = new List<(int, int)>
        {
            current
        };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            res.Add(current);
        }
        res.Reverse();
        return res;
    }
    //Manhattan distance heuristics - this works because the plotted distance from a point will always be bigger than one closer to the source
    private static int Distance((int, int) cur, (int, int) other)
    {
        int x = Math.Abs(cur.Item1 - other.Item1);
        int y = Math.Abs(cur.Item2 - other.Item2);
        return x + y;
    }

    private static List<(int, int)> GetUnvisitedNeighbors(int[,] plot, List<(int, int)> remainingVerticies, (int, int) current)
    {
        var returnList = new List<(int, int)>();

        if (current.Item2 > 0 && remainingVerticies.Contains((current.Item1, current.Item2 - 1)))
            returnList.Add((current.Item1, current.Item2 - 1));

        if (current.Item2 < plot.GetLength(0) - 1 && remainingVerticies.Contains((current.Item1, current.Item2 + 1)))
            returnList.Add((current.Item1, current.Item2 + 1));

        if (current.Item1 > 0 && remainingVerticies.Contains((current.Item1 - 1, current.Item2)))
            returnList.Add((current.Item1 - 1, current.Item2));
        if (current.Item1 < plot.GetLength(1) - 1 && remainingVerticies.Contains((current.Item1 + 1, current.Item2)))
            returnList.Add((current.Item1 + 1, current.Item2));
        return returnList;
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

    private static int[,] GeneratePlot(List<string> data, int grid)
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
