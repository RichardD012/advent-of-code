namespace AdventCode.Tasks2020;

public class Day6Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 6;
    private readonly ILogger<Day6Task> _logger;
    #region TestData
    protected override string TestData => @"abc

a
b
c

ab
ac

a
a
a
a

b";
    #endregion

    public Day6Task(IAdventWebClient client, ILogger<Day6Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var answers = GenerateAnswerBoard<Dictionary<string, int>>(data, (stringEntry, boards) =>
        {
            var seenCount = new Dictionary<string, bool>();
            foreach (var line in stringEntry)
            {
                foreach (var lineChar in line)
                {
                    seenCount[lineChar.ToString()] = true;
                }
            }
            foreach (var answers in seenCount.Keys)
            {
                boards[answers] = boards.GetValueOrDefault(answers) + 1;
            }
            return boards;
        });
        return answers.Values.Sum().ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        return GenerateAnswerBoard<int>(data, (stringEntry, returnCount) =>
        {
            var groupAnswers = new Dictionary<string, int>();
            //each person in the group
            foreach (var line in stringEntry)
            {
                //each persons answers
                foreach (var lineChar in line)
                {
                    groupAnswers[lineChar.ToString()] = groupAnswers.GetValueOrDefault(lineChar.ToString()) + 1;
                }
            }
            foreach (var answer in groupAnswers.Keys)
            {
                if (groupAnswers[answer] == stringEntry.Count)
                {
                    returnCount++;
                }
            }
            return returnCount;
        }).ToString();
    }

    private static TReturn GenerateAnswerBoard<TReturn>(List<string> data, Func<List<string>, TReturn, TReturn> updateFunction) where TReturn : new()
    {
        var returnCount = new TReturn();
        var index = 0;
        do
        {
            var stringEntry = new List<string>();
            string entry;
            do
            {
                entry = data[index];
                if (string.IsNullOrEmpty(entry) == false)
                {
                    stringEntry.Add(entry);
                }
                index++;
            } while (string.IsNullOrEmpty(entry) == false && index < data.Count);
            returnCount = updateFunction(stringEntry, returnCount);
        } while (index < data.Count);
        return returnCount;
    }
}
