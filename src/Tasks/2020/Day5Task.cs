namespace AdventCode.Tasks2020;

public class Day5Task : BaseCodeTask, IAdventCodeTask
{
    private const int NumRows = 128;
    private const int NumColumns = 8;
    public override int TaskDay => 5;
    private readonly ILogger<Day5Task> _logger;
    #region TestData
    protected override string TestData => @"FBFBBFFRLR
    BFFFBBFRRR
    FFFBBBFRRR
    BBFFBBFRLL";
    #endregion

    public Day5Task(IAdventWebClient client, ILogger<Day5Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var maxId = 0;
        foreach (var row in data)
        {
            var rows = Enumerable.Range(0, NumRows).ToArray();
            for (var i = 0; i < 7; i++)
            {
                rows = GetPartition(row[i], rows);
            }
            var columns = Enumerable.Range(0, NumColumns).ToArray();
            for (var i = 7; i < 10; i++)
            {
                columns = GetPartition(row[i], columns);
            }
            if (columns.Length != 1 && rows.Length != 1)
            {
                throw new InvalidAnswerException();
            }
            var ticketId = CalculateTicketId(rows[0], columns[0]);
            if (ticketId > maxId)
            {
                maxId = ticketId;
            }
        }
        return maxId.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var foundIds = new List<int>();
        var data = await GetDataAsListAsync<string>();
        var seats = new bool[NumRows, NumColumns];
        foreach (var row in data)
        {
            var rows = Enumerable.Range(0, NumRows).ToArray();
            for (var i = 0; i < 7; i++)
            {
                rows = GetPartition(row[i], rows);
            }
            var columns = Enumerable.Range(0, NumColumns).ToArray();
            for (var i = 7; i < 10; i++)
            {
                columns = GetPartition(row[i], columns);
            }
            if (columns.Length != 1 && rows.Length != 1)
            {
                throw new InvalidAnswerException();
            }
            seats[rows[0], columns[0]] = true;
            var ticketId = CalculateTicketId(rows[0], columns[0]);
            foundIds.Add(ticketId);
        }
        for (var row = 0; row < NumRows; row++)
        {
            for (var col = 0; col < NumColumns; col++)
            {
                if (seats[row, col] == false)
                {
                    var thisId = CalculateTicketId(row, col);
                    if (foundIds.Contains(thisId + 1) && foundIds.Contains(thisId - 1))
                    {
                        return thisId.ToString();
                    }
                }
            }
        }
        throw new InvalidAnswerException();
    }

    private int CalculateTicketId(int row, int column) => (row * 8) + column;

    private static int[] GetPartition(char Direction, int[] Partition)
    {
        if (Direction != 'F' && Direction != 'L' && Direction != 'B' && Direction != 'R') throw new ArgumentOutOfRangeException(nameof(Direction), "Invalid Direction Provided");
        if (Direction == 'F' || Direction == 'L')
        {
            return Partition.Take((int)Math.Floor(Partition.Length / 2.0)).ToArray();
        }
        return Partition.Skip((int)Math.Ceiling(Partition.Length / 2.0)).ToArray();
    }
}
