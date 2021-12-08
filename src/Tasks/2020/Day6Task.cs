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
        var answers = GenerateAnswerBoard<Dictionary<string, int>>(data, (groupEntry, boards) =>
        {
            //Answer dictionary for a given group
            var seenCount = new Dictionary<string, bool>();
            //a persons answers in a group
            foreach (var line in groupEntry)
            {
                //each individual answer
                foreach (var lineChar in line)
                {
                    seenCount[lineChar.ToString()] = true; //simply mark this answer as seen
                }
            }
            //for the existing board result, add each answer that was seen by that group
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
        return GenerateAnswerBoard<int>(data, (groupEntry, returnCount) =>
        {
            var groupAnswers = new Dictionary<string, int>();
            //each person in the group
            foreach (var line in groupEntry)
            {
                //each persons answers
                foreach (var lineChar in line)
                {
                    groupAnswers[lineChar.ToString()] = groupAnswers.GetValueOrDefault(lineChar.ToString()) + 1;
                }
            }
            foreach (var answer in groupAnswers.Keys)
            {
                if (groupAnswers[answer] == groupEntry.Count) // compare the answers for this letter/question to the number of people. They need to match
                {
                    returnCount++;
                }
            }
            return returnCount;
        }).ToString();
    }

    /// <summary>
    /// Function to process each group in the answers
    /// </summary>
    /// <param name="data">Data is the input data from the Problem 6 input data</param>
    /// <param name="updateFunction">Function to perform on each answer group. The input is the list of answers for that group and the existing result set.</param>
    /// <typeparam name="TReturn">Type of data the calling function is expecting.</typeparam>
    /// <returns>Updated return set</returns>
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
