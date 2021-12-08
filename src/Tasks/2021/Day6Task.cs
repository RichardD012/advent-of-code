using System.Security.Cryptography.X509Certificates;

namespace AdventCode.Tasks2021;

public class Day6Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 6;
    private readonly ILogger<Day6Task> _logger;
    #region TestData
    protected override string TestData => @"3,4,3,1,2";
    #endregion

    public Day6Task(IAdventWebClient client, ILogger<Day6Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var testData = (await GetDataAsync()).Split(",").Select(x => int.Parse(x)).ToList();
        //Straightforward solution
        for (var i = 0; i < 80; i++)
        {
            var newFish = 0;
            for (var fish = 0; fish < testData.Count; fish++)
            {
                if (testData[fish] == 0)
                {
                    testData[fish] = 6;
                    newFish++;
                }
                else
                {
                    testData[fish]--;
                }
            }
            for (int n = 0; n < newFish; n++)
            {
                testData.Add(8);
            }
        }
        return testData.Count.ToString();
    }
    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        //straightforward solution doesn't work because array is too large. 
        //Instead of tracking each fish, track how many fish are on each day so you now only have an array of size 8

        var testData = (await GetDataAsync()).Split(",").Select(x => int.Parse(x)).GroupBy(x => x)
                .ToDictionary(x => x.Key, x => (long)x.Count());

        for (var i = 0; i < 256; i++)
        {
            //create a new Dictionary taking the previous days count and setting it to the next so 8-> 7-> 6. 
            //However on Day 6 *and* Day 8 you are setting Day 0 since this is the exponential growth
            testData = new()
            {
                [8] = testData.GetValueOrDefault(0),
                [7] = testData.GetValueOrDefault(8),
                [6] = testData.GetValueOrDefault(0) + testData.GetValueOrDefault(7),
                [5] = testData.GetValueOrDefault(6),
                [4] = testData.GetValueOrDefault(5),
                [3] = testData.GetValueOrDefault(4),
                [2] = testData.GetValueOrDefault(3),
                [1] = testData.GetValueOrDefault(2),
                [0] = testData.GetValueOrDefault(1),
            };
        }
        return testData.Values.Sum().ToString();
    }
}
