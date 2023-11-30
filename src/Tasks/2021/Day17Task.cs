namespace AdventCode.Tasks2021;


public class Day17Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 17;
    private readonly ILogger<Day17Task> _logger;
    #region TestData
    protected override string TestData => @"target area: x=20..30, y=-10..-5";
    #endregion

    public Day17Task(IAdventWebClient client, ILogger<Day17Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsync();
        var (_, yPos) = GenerateTargetArea(data);
        return (yPos.Item1 * (yPos.Item1 + 1) / 2).ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsync();
        var (xPos, yPos) = GenerateTargetArea(data);
        int x1 = xPos.Item1; int x2 = xPos.Item2;
        int y1 = yPos.Item1; int y2 = yPos.Item2;
        // assume target always +'ve x and -'ve y
        //bounds are found by the fact that we will overshoot the target in 1 step for certain x and y values
        //trivially shown for x, for y this is true because even if fired upwards it will always return to the y=0 point with a -'ve velocity 1 higher than its initial velocity (energy conservation?!)
        //also a better x-bound possible given that for low v it will never reach the target before going vertical
        (int minvx, int maxvx) = (0, x2);
        (int minvy, int maxvy) = (y1, Math.Abs(y1));

        int answer2 = 0;
        //could solve vx and vy seperately to find the step counts for which the projectile is in the target x or y range
        //then all valid solutions are overlaps of these for which there are fast algorithms
        //however brute force is easily enough for the input range
        for (int vx = minvx; vx <= maxvx; vx++)
        {
            for (int vy = minvy; vy <= maxvy; vy++)
            {
                int maxy = 0;
                int x = 0; int y = 0;
                for (int step = 1; x <= x2 && y >= y1; step++)
                {
                    //sum of arithmetic progression = n[2 * a - (n-1)]/2
                    x = step <= vx ? step * (2 * vx - (step - 1)) / 2 : x;
                    y = step * (2 * vy - (step - 1)) / 2;

                    maxy = y > maxy ? y : maxy;
                    if (x >= x1 && x <= x2 && y >= y1 && y <= y2)
                    {
                        answer2++;
                        break;
                    }
                }
            }
        }
        return answer2.ToString();
    }

    public ((int, int), (int, int)) GenerateTargetArea(string input)
    {
        var coordinates = input.Replace("target area: ", "").Split(", ");
        var xPos = coordinates[0].Replace("x=", "").Split("..");
        var yPos = coordinates[1].Replace("y=", "").Split("..");
        var x = (int.Parse(xPos[0]), int.Parse(xPos[1]));
        var y = (int.Parse(yPos[0]), int.Parse(yPos[1]));
        return (x, y);
    }
}
