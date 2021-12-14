using System;
using System.Diagnostics.Eventing.Reader;

namespace AdventCode.Tasks2021;


public class Day14Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 14;
    private readonly ILogger<Day14Task> _logger;
    #region TestData
    protected override string TestData => @"NNCB

CH -> B
HH -> N
CB -> H
NH -> C
HB -> C
HC -> B
HN -> C
NN -> C
BH -> H
NC -> B
NB -> B
BN -> B
BB -> N
BC -> B
CC -> N
CN -> C";
    #endregion

    public Day14Task(IAdventWebClient client, ILogger<Day14Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var polymerTemplate = GenerateBruteForceTemplate(data, 10);
        return polymerTemplate.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var polymerTemplate = GenerateOptimumTemplate(data, 40);
        return polymerTemplate.ToString();
    }

    private static long GenerateOptimumTemplate(List<string> data, int iterations)
    {
        var polymerTemplate = data[0];
        var ruleDictionary = new Dictionary<string, string>();
        foreach (var instruction in data.Skip(2))
        {
            var rules = instruction.Split(" -> ");
            ruleDictionary[rules[0]] = rules[1];
        }
        var letterDictionary = new Dictionary<char, long>();
        var countDictionary = new Dictionary<string, long>();
        for (var letter = 0; letter < polymerTemplate.Length; letter++)
        {
            if (letter != polymerTemplate.Length - 1)
                countDictionary[polymerTemplate.Substring(letter, 2)] = countDictionary.GetValueOrDefault(polymerTemplate.Substring(letter, 2)) + 1;
            letterDictionary[polymerTemplate[letter]] = letterDictionary.GetValueOrDefault(polymerTemplate[letter]) + 1;
        }
        for (var i = 0; i < iterations; i++)
        {
            var newCountDictionary = new Dictionary<string, long>();
            foreach (var entry in ruleDictionary.Keys)
            {
                if (countDictionary.ContainsKey(entry))
                {
                    var rule = ruleDictionary[entry];
                    var key1 = entry[0] + rule;
                    var key2 = rule + entry[1];
                    newCountDictionary[key1] = newCountDictionary.GetValueOrDefault(key1) + countDictionary[entry];
                    newCountDictionary[key2] = newCountDictionary.GetValueOrDefault(key2) + countDictionary[entry];
                    letterDictionary[rule[0]] = letterDictionary.GetValueOrDefault(rule[0]) + countDictionary[entry];
                }
            }
            countDictionary = newCountDictionary;
        }
        return letterDictionary.Values.Max() - letterDictionary.Values.Min();
    }

    private static long GenerateBruteForceTemplate(List<string> data, int iterations)
    {
        var polymerTemplate = data[0];
        var ruleDictionary = new Dictionary<string, string>();
        foreach (var instruction in data.Skip(2))
        {
            var rules = instruction.Split(" -> ");
            ruleDictionary[rules[0]] = rules[1];
        }
        for (var i = 0; i < iterations; i++)
        {
            for (var letter = 0; letter < polymerTemplate.Length - 1; letter++)
            {
                var currentGroup = polymerTemplate.Substring(letter, 2);
                if (ruleDictionary.ContainsKey(currentGroup))
                {
                    polymerTemplate = string.Concat(polymerTemplate.AsSpan(0, letter + 1), ruleDictionary[currentGroup], polymerTemplate.AsSpan(letter + 1));
                    letter++;
                }
            }
        }
        var count = polymerTemplate.GroupBy(c => c).Select(c => new { Char = c.Key, Count = c.LongCount() });
        return count.Max(x => x.Count) - count.Min(x => x.Count);
    }
}