namespace AdventCode.Tasks2021;


public class Day21Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 21;
    private readonly ILogger<Day21Task> _logger;
    #region TestData
    protected override string TestData => @"";
    #endregion

    public Day21Task(IAdventWebClient client, ILogger<Day21Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsync();
        // ⭐ we can convert 3 consecutive throws to a 3-throw with the new .Net 6 Chunk function:
        var threeRoll = DeterministicThrows().Chunk(3).Select(x => x.Sum());

        // take turns until the active player wins:
        var round = 0;
        var (active, other) = Parse(data);
        foreach (var steps in threeRoll)
        {
            round++;
            active = active.Move(steps);
            if (active.score >= 1000)
            {
                break;
            }
            (active, other) = (other, active);
        }

        return (other.score * 3 * round).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsync();

        // win counts tells us how many times the active and the other player wins
        // if they are starting from the given positions and scores.

        // this function needs to be cached, because we don't have time till eternity.
        var cache = new Dictionary<(Player, Player), (long, long)>();

        (long activeWins, long otherWins) winCounts((Player active, Player other) players)
        {
            if (players.other.score >= 21)
            {
                return (0, 1);
            }

            if (!cache.ContainsKey(players))
            {
                var (activeWins, otherWins) = (0L, 0L);
                foreach (var steps in DiracThrows())
                {
                    var wins = winCounts((players.other, players.active.Move(steps)));
                    // they are switching roles here ^
                    // hence the return value needs to be swapped as well
                    activeWins += wins.otherWins;
                    otherWins += wins.activeWins;
                }
                cache[players] = (activeWins, otherWins);
            }
            return cache[players];
        }

        var wins = winCounts(Parse(data));

        // which player wins more:
        return Math.Max(wins.activeWins, wins.otherWins).ToString();
    }

    IEnumerable<int> DeterministicThrows() =>
        from i in Enumerable.Range(1, int.MaxValue)
        select (i - 1) % 100 + 1;

    IEnumerable<int> DiracThrows() =>
        from i in new[] { 1, 2, 3 }
        from j in new[] { 1, 2, 3 }
        from k in new[] { 1, 2, 3 }
        select i + j + k;

    (Player active, Player other) Parse(string input)
    {
        var players = (
            from line in input.Split("\n")
            let parts = line.Split(": ")
            select new Player(0, int.Parse(parts[1]))
        ).ToArray();
        return (players[0], players[1]);
    }
}

record Player(int score, int pos)
{
    public Player Move(int steps)
    {
        var newPos = (this.pos - 1 + steps) % 10 + 1;
        return new Player(this.score + newPos, newPos);
    }
}
