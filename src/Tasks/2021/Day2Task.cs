namespace AdventCode.Tasks2021;

/// <summary>
/// Day2s Test Tasks
/// </summary>
public class Day2Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 2;
    private readonly ILogger<Day2Task> _logger;
    #region TestData
    protected override string TestData => @"forward 5
down 5
forward 8
up 3
down 8
forward 2";
    #endregion

    public Day2Task(IAdventWebClient client, ILogger<Day2Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        int horizontal = 0, depth = 0;
        data.ForEach(x =>
        {
            var code = x.Split(" ");
            var position = code[0];
            var direction = int.Parse(code[1]);
            switch (position)
            {
                case "forward":
                    horizontal += direction;
                    break;
                case "down":
                    depth += direction;
                    break;
                case "up":
                    depth -= direction;
                    break;
            }
        });
        return (horizontal * depth).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        int horizontal = 0, depth = 0, aim = 0;
        data.ForEach(x =>
        {
            var code = x.Split(" ");
            var position = code[0];
            var direction = int.Parse(code[1]);
            switch (position)
            {
                case "forward":
                    horizontal += direction;
                    depth += (aim * direction);
                    break;
                case "down":
                    aim += direction;
                    break;
                case "up":
                    aim -= direction;
                    break;
            }
        });
        return (horizontal * depth).ToString();
    }

}
