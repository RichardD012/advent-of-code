using System;
using System.Xml.Schema;

namespace AdventCode.Tasks2021;


public class Day16Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 16;
    private readonly ILogger<Day16Task> _logger;
    private readonly Dictionary<string, string> Encoding = new()
    {
        ["0"] = "0000",
        ["1"] = "0001",
        ["2"] = "0010",
        ["3"] = "0011",
        ["4"] = "0100",
        ["5"] = "0101",
        ["6"] = "0110",
        ["7"] = "0111",
        ["8"] = "1000",
        ["9"] = "1001",
        ["A"] = "1010",
        ["B"] = "1011",
        ["C"] = "1100",
        ["D"] = "1101",
        ["E"] = "1110",
        ["F"] = "1111"
    };

    private readonly Dictionary<int, Func<List<long>, long>> Operations = new()
    {
        [0] = (values) => values.Sum(), //sum
        [1] = (values) => values.Aggregate((long)1, (acc, val) => acc * val), //product
        [2] = (values) => values.Min(), //min
        [3] = (values) => values.Max(), //max
        [5] = (values) => values[0] > values[1] ? 1 : 0, //greather than
        [6] = (values) => values[0] < values[1] ? 1 : 0, //less than
        [7] = (values) => values[0] == values[1] ? 1 : 0
    };
    #region TestData
    protected override string TestData => @"880086C3E88112";
    #endregion

    public Day16Task(IAdventWebClient client, ILogger<Day16Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsync();
        var binaryString = GenerateBinaryString(data);
        var (_, version, _) = ParsePacket(0, binaryString);
        return version.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsync();
        var binaryString = GenerateBinaryString(data);
        var (_, _, value) = ParsePacket(0, binaryString);
        return value.ToString();
    }

    private string GenerateBinaryString(string data) => string.Join("", data.Select(x => Encoding[x.ToString()]));

    private (int, long, long) ParsePacket(int startVersion, string packetString)
    {
        var typeID = Convert.ToInt32(packetString.Substring(3, 3), 2);
        if (typeID == 4)
        {
            var (literalIndex, literalVersion, literalValue) = ParseLiteral(packetString);
            return (literalIndex, literalVersion + startVersion, literalValue);
        }
        else
        {
            var (opIndex, opVersion, opValue) = ParseOperation(packetString, Operations[typeID]);
            return (opIndex, opVersion + startVersion, opValue);
        }
        throw new TaskIncompleteException();
    }

    private (int, long, long) ParseOperation(string binary, Func<List<long>, long> operation)
    {
        var version = Convert.ToInt64(binary[..3], 2);

        var values = new List<long>();
        int index = -1;
        if (binary.Select(x => x).Any(x => x == '1') == false)
        {
            return (binary.Length, 0, 0);
        }
        if (binary[6] == '1')
        {
            var numPackets = Convert.ToInt32(binary.Substring(7, 11), 2);
            index = 18;
            foreach (var _ in Enumerable.Range(1, numPackets))
            {
                var (newIndex, newVersion, value) = ParsePacket(0, binary[index..]);
                index += newIndex;
                version += newVersion;
                values.Add(value);
            }
        }
        else if (binary[6] == '0')
        {
            var bitLength = Convert.ToInt32(binary.Substring(7, 15), 2);
            index = 22;
            while (index < 22 + bitLength)
            {
                var (newIndex, newVersion, value) = ParsePacket(0, binary[index..]);
                index += newIndex;
                version += newVersion;
                values.Add(value);
            }
        }
        return (index, version, operation(values));
    }

    private static (int, long, long) ParseLiteral(string binary)
    {
        var index = 6;
        var resultBinary = "";
        var version = Convert.ToInt64(binary[..3], 2);
        while (true)
        {
            resultBinary += binary.Substring(index + 1, 4);
            if (binary[index] == '0')
            {
                return (index + 5, version, Convert.ToInt64(resultBinary, 2));
            }
            index += 5;
        }
        throw new InvalidAnswerException();
    }
}
