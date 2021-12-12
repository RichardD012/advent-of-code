using System.Reflection.Emit;

namespace AdventCode.Tasks2021;


public class Day12Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 12;
    private readonly ILogger<Day12Task> _logger;
    #region TestData
    protected override string TestData => @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";
    /** //Other data
        protected override string TestData => @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";
    **/
    #endregion

    public Day12Task(IAdventWebClient client, ILogger<Day12Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var startCave = GenerateCaves(data);
        var pathCount = GeneratePathCounts(startCave, new Dictionary<string, int>(), 1);
        return pathCount.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var startCave = GenerateCaves(data);
        var pathCount = GeneratePathCounts(startCave, new Dictionary<string, int>(), 2);
        return pathCount.ToString();
    }

    private static int GeneratePathCounts(Cave startCave, Dictionary<string, int> excludeList, int maxSmallCave)
    {
        //if we reached the end, we're done
        if (startCave.IsEndCave)
        {
            return 1;
        }
        //if this isn't a small cave, incrrement the counter for visiting it
        if (startCave.IsLargeCave == false)
        {
            excludeList[startCave.Label] = excludeList.GetValueOrDefault(startCave.Label) + 1;
        }
        int paths = 0;
        foreach (var entry in startCave.Neighbors)
        {
            //can never revisit the start of the caves
            if (entry.IsStartCave)
            {
                continue;
            }
            //If we've already been to a small cave and it meets the single smallcave threshold - if it were *each* small cave can be visited twice we'd only check that instance's count
            if (excludeList.ContainsKey(entry.Label) && excludeList.Values.Any(x => x >= maxSmallCave))
            {
                continue;
            }
            paths += GeneratePathCounts(entry, excludeList.ToDictionary(x => x.Key, x => x.Value), maxSmallCave);
        }
        return paths;
    }

    private static Cave GenerateCaves(List<string> data)
    {
        var caves = new Dictionary<string, Cave>();
        foreach (var entry in data)
        {
            var path = entry.Split("-");
            var startNode = path[0];
            var endNode = path[1];
            Cave startCave, endCave;
            if (caves.ContainsKey(startNode) == false)
            {
                startCave = new Cave(startNode);
                caves[startNode] = startCave;
            }
            else
            {
                startCave = caves[startNode];
            }

            if (caves.ContainsKey(endNode) == false)
            {
                endCave = new Cave(endNode);
                caves[endNode] = endCave;
            }
            else
            {
                endCave = caves[endNode];
            }
            startCave.AddNodeIfNotPresent(endCave);
            endCave.AddNodeIfNotPresent(startCave);
        }
        return caves["start"];
    }

    private class Cave
    {
        public string Label { get; set; }
        public List<Cave> Neighbors { get; set; } = new List<Cave>();
        public bool IsLargeCave => "start".EqualsIgnoreCase(Label) == false && "end".EqualsIgnoreCase(Label) == false && Label.All(c => char.IsUpper(c));

        public bool IsStartCave => "start".EqualsIgnoreCase(Label);
        public bool IsEndCave => "end".EqualsIgnoreCase(Label);

        public Cave(string label) => Label = label;

        internal void AddNodeIfNotPresent(Cave neighbor)
        {
            if (Neighbors.Any(x => x.Label.EqualsIgnoreCase(neighbor.Label)) == false)
            {
                Neighbors.Add(neighbor);
            }
        }
    }
}
