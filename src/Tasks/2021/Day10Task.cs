namespace AdventCode.Tasks2021;


public class Day10Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 10;
    private readonly ILogger<Day10Task> _logger;
    #region TestData
    protected override string TestData => @"[({(<(())[]>[[{[]{<()<>>
[(()[<>])]({[<{<<[]>>(
{([(<{}[<>[]}>{[]{[(<()>
(((({<>}<{<{<>}{[]{[]{}
[[<[([]))<([[{}[[()]]]
[{[{({}]{}}([{[{{{}}([]
{<[[]]>}<{[{[{[]{()[[[]
[<(<(<(<{}))><([]([]()
<{([([[(<>()){}]>(<<{{
<{([{{}}[<[[[<>{}]]]>[]]";
    #endregion

    public Day10Task(IAdventWebClient client, ILogger<Day10Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        ulong illegalCount = 0;
        foreach (var line in data)
        {
            var stack = new Stack<char>();
            foreach (var syntaxChar in line)
            {
                if (IsStartChar(syntaxChar))
                {
                    stack.Push(syntaxChar);
                }
                else if (IsEndChar(syntaxChar))
                {
                    var topOfStack = stack.Pop();
                    if (IsValidEndCharacter(topOfStack, syntaxChar) == false)
                    {
                        illegalCount += GetIllegalValueScore(syntaxChar);
                        break;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(syntaxChar), $"Provided character {syntaxChar} was invalid");
                }
            }
        }
        return illegalCount.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var incompleteScores = new List<ulong>();
        foreach (var line in data)
        {
            ulong incompleteScore = 0;
            var stack = new Stack<char>();
            var lineInvalid = false;
            foreach (var syntaxChar in line)
            {
                if (IsStartChar(syntaxChar))
                {
                    stack.Push(syntaxChar);
                }
                else if (IsEndChar(syntaxChar))
                {
                    var topOfStack = stack.Pop();
                    if (IsValidEndCharacter(topOfStack, syntaxChar) == false)
                    {
                        lineInvalid = true;
                        break;
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(syntaxChar), $"Provided character {syntaxChar} was invalid");
                }
            }
            if (lineInvalid == false)
            {
                while (stack.Count != 0)
                {
                    incompleteScore = (incompleteScore * 5) + GetMissingValueScore(GetCorrespondingEndCharacter(stack.Pop()));
                }
                incompleteScores.Add(incompleteScore);
            }
        }
        incompleteScores = incompleteScores.OrderBy(x => x).ToList();
        var middleIndex = Math.Floor(incompleteScores.Count / 2.0);
        return incompleteScores.ElementAt((int)middleIndex).ToString();
    }

    private static ulong GetIllegalValueScore(char syntaxChar) => syntaxChar switch
    {
        ')' => 3,
        ']' => 57,
        '}' => 1197,
        '>' => 25137,
        _ => throw new InvalidDataException($"Invalid syntax character to score: {syntaxChar}"),
    };

    private static ulong GetMissingValueScore(char syntaxChar) => syntaxChar switch
    {
        ')' => 1,
        ']' => 2,
        '}' => 3,
        '>' => 4,
        _ => throw new InvalidDataException($"Invalid syntax character to score: {syntaxChar}"),
    };

    private static char GetCorrespondingEndCharacter(char syntaxChar) => syntaxChar switch
    {
        '(' => ')',
        '[' => ']',
        '{' => '}',
        '<' => '>',
        _ => throw new InvalidDataException($"Invalid syntax character to score: {syntaxChar}"),
    };

    private static bool IsValidEndCharacter(char start, char end) => start switch
    {
        '(' => end == ')',
        '[' => end == ']',
        '{' => end == '}',
        '<' => end == '>',
        _ => throw new InvalidDataException($"Invalid starting character provided: {start}"),
    };

    private static bool IsStartChar(char @char) => @char is '(' or '{' or '[' or '<';
    private static bool IsEndChar(char @char) => @char is ')' or '}' or ']' or '>';

}

