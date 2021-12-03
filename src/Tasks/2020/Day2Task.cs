using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;
using AdventCode.Tasks;
using System.Linq;

namespace AdventCode.Tasks2020;

public class Day2Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 2;
    private readonly ILogger<Day2Task> _logger;
    #region TestData
    protected override string TestData => @"1-3 a: abcde
1-3 b: cdefg
2-9 c: ccccccccc";
    #endregion;
    public Day2Task(IAdventWebClient client, ILogger<Day2Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var list = await GetDataAsListAsync<string>();
        return list.Select(x => GenerateInputRule(x)).Count(x => x.IsValid()).ToString();
    }

    private static InputRule GenerateInputRule(string input)
    {
        var dataInput = input.Split(":");
        var rules = dataInput[0].Split(" ");
        var range = rules[0].Split("-");
        return new InputRule(int.Parse(range[0]), int.Parse(range[1]), rules[1], dataInput[1]);
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var list = await GetDataAsListAsync<string>();
        return list.Select(x => GenerateInputRule(x)).Count(x => x.IsValid2()).ToString();
    }

    private class InputRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string Letter { get; set; }
        public string Input { get; set; }
        public InputRule(int min, int max, string letter, string input)
        {
            Min = min;
            Max = max;
            Letter = letter.Trim();
            Input = input.Trim();
        }

        public bool IsValid()
        {
            var count = 0;
            foreach (var c in Input)
            {
                if (c.ToString().EqualsIgnoreCase(Letter))
                {
                    count++;
                }
            }
            return count >= Min && count <= Max;
        }
        public bool IsValid2()
        {

            return (Input[Min - 1].ToString().EqualsIgnoreCase(Letter) && Input[Max - 1].ToString().EqualsIgnoreCase(Letter) == false) ||
                (Input[Min - 1].ToString().EqualsIgnoreCase(Letter) == false && Input[Max - 1].ToString().EqualsIgnoreCase(Letter));
        }

    }
}
