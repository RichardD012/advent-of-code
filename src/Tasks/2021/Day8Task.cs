namespace AdventCode.Tasks2021;

public class Day8Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 8;
    private readonly ILogger<Day8Task> _logger;
    #region TestData
    protected override string TestData => @"be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe
edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc
fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg
fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb
aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea
fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb
dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe
bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef
egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb
gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce";
    #endregion

    public Day8Task(IAdventWebClient client, ILogger<Day8Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var responses = await GetDataAsListAsync<string>();
        int count = 0;
        foreach (var entry in responses)
        {
            var encoding = entry.Split("|");
            var answers = encoding[1].Trim().Split(" ");
            foreach (var answer in answers)
            {
                if (answer.Length == 2 || answer.Length == 3 || answer.Length == 4 || answer.Length == 7)
                {
                    count++;
                }
            }
        }
        return count.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var responses = await GetDataAsListAsync<string>();
        int count = 0;
        foreach (var entry in responses)
        {
            var encoding = entry.Split("|");
            var answers = encoding[1].Trim().Split(" ");
            var encodings = GetDigits(encoding[0].Split(" "));
            var stringRepresentation = string.Empty;
            foreach (var answer in answers)
            {
                var digitInt = DetermineDigit(answer, encodings);
                stringRepresentation = stringRepresentation + digitInt.ToString();
            }
            count += int.Parse(stringRepresentation);
        }
        return count.ToString();
    }

    private static int DetermineDigit(string answer, string[] encodings)
    {
        for (var i = 0; i < encodings.Length; i++)
        {
            if (encodings[i].ToCharArray().Except(answer.ToCharArray()).ToArray().Length == 0)
            {
                if (encodings[i].Length != answer.Length) continue;
                return i;
            }
        }
        throw new InvalidAnswerException();
    }

    private string[] GetDigits(string[] encodings)
    {
        var result = new string[10];
        foreach (var entry in encodings)
        {
            if (entry.Length == 2)
                result[1] = entry;
            if (entry.Length == 4)
                result[4] = entry;
            if (entry.Length == 3)
                result[7] = entry;
            if (entry.Length == 7)
                result[8] = entry;
        }
        //Determine 3 - 3 is the only 4 segment display that contains 1
        result[3] = encodings
            .Where(x => x.Length == 5)
            .First(x =>
            {
                var array = x.ToCharArray();
                return array.Contains(result[1][0]) && array.Contains(result[1][1]);
            });

        //Determine 6 - 6 is the only 6 segment that contains 1 part of the one (9 and 0 both contain both parts)
        result[6] = encodings
            .Where(x => x.Length == 6)
            .First(x =>
            {
                var array = x.ToCharArray();
                return (array.Contains(result[1][0]) && array.Contains(result[1][1]) == false) || (array.Contains(result[1][0]) == false && array.Contains(result[1][1]));
            });

        //Determine 5 - 5 and 2 are the only two 5 section numbers left. 5 is only 1 section off of 6, 2 is 2
        result[5] = encodings
            .Where(x => x.Length == 5 && x != result[3])
            .First(x =>
            {
                return result[6].ToCharArray().Except(x.ToCharArray()).ToArray().Length == 1;
            }
        );
        //Determine 2 - 2 is the only 5 section left
        result[2] = encodings.First(x => x.Length == 5 && x != result[3] && x != result[5]);

        //Determine 9 - 9 and 0 are the only 6 sections left - Taking 5 out of 9, you're left with 1 section, taking 5 out of 9 you're left with 2
        result[9] = encodings
            .Where(x => x.Length == 6 && x != result[6])
            .First(x =>
            {
                return x.ToCharArray().Except(result[5].ToCharArray()).ToArray().Length == 1;
            }
        );

        //Determine 0 - 0 is the only 6 section left
        result[0] = encodings.First(x => x.Length == 6 && x != result[9] && x != result[6]);
        return result;
    }


}
