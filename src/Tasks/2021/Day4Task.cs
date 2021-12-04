using System.Threading.Tasks;
using AdventCode.Utils;
using Microsoft.Extensions.Logging;
using AdventCode.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Metrics;
using Sprache;
using System;

namespace AdventCode.Tasks2021;

public class Day4Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 4;
    private readonly ILogger<Day4Task> _logger;
    #region TestData
    protected override string TestData => @"7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1

22 13 17 11  0
 8  2 23  4 24
21  9 14 16  7
 6 10  3 18  5
 1 12 20 15 19

 3 15  0  2 22
 9 18 13 17  5
19  8  7 25 23
20 11 10 24  4
14 21 16 12  6

14 21 17 24  4
10 16 15  9 19
18  8 23 26 20
22 11 13  6  5
 2  0 12  3  7";
    #endregion

    public Day4Task(IAdventWebClient client, ILogger<Day4Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var input = data[0].Split(",").Select(x => int.Parse(x));
        var bingoBoards = GenerateBingoBoards(data.Skip(2).ToList());
        foreach (var entry in input)
        {
            foreach (var board in bingoBoards)
            {
                (var bingo, var result, var notWinningResult) = board.SetNumber(entry);
                if (bingo && result != null && notWinningResult != null)
                {
                    return (notWinningResult.Sum() * entry).ToString();
                }
            }
        }
        throw new InvalidAnswerException();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var input = data[0].Split(",").Select(x => int.Parse(x));
        var bingoBoards = GenerateBingoBoards(data.Skip(2).ToList());
        foreach (var entry in input)
        {
            foreach (var board in bingoBoards)
            {
                (var bingo, var result, var notWinningResult) = board.SetNumber(entry);
                if (bingo && result != null && notWinningResult != null)
                {
                    if (bingoBoards.Where(x => x != board).Any(x => x.Bingo == false) == false)
                    {
                        return (notWinningResult.Sum() * entry).ToString();
                    }
                }
            }
        }
        throw new InvalidAnswerException();
    }

    private static List<BingoBoard> GenerateBingoBoards(List<string> data)
    {
        var boards = new List<BingoBoard>();
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
            boards.Add(new BingoBoard(stringEntry));
        } while (index < data.Count);
        return boards;
    }

    public class BingoBoard
    {
        public bool Bingo { get; private set; }
        private Spot[,] Board { get; set; } = new Spot[5, 5];
        public BingoBoard(List<string> boardInput)
        {
            if (boardInput.Count != 5) throw new ArgumentOutOfRangeException(nameof(boardInput), "Wrong number of rows for board");
            for (var y = 0; y < boardInput.Count; y++)
            {
                var inputs = boardInput[y].Trim().Split(" ").Where(x => string.IsNullOrEmpty(x) == false).Select(x => int.Parse(x)).ToList();
                if (inputs.Count != 5) throw new ArgumentOutOfRangeException(nameof(inputs), "Wrong number of columns for board");
                for (var x = 0; x < inputs.Count; x++)
                {
                    Board[y, x] = new Spot(inputs[x]);
                }
            }
        }

        public (bool, int[]?, int[]?) SetNumber(int number)
        {
            for (var y = 0; y < 5; y++)
            {
                for (var x = 0; x < 5; x++)
                {
                    if (Board[y, x].Number == number)
                    {
                        Board[y, x].Hit = true;
                        return HasBingo();
                    }
                }
            }
            return (false, null, null);
        }

        public (bool, int[]?, int[]?) HasBingo()
        {
            for (var yPos = 0; yPos < 5; yPos++)
            {
                var tempRow = Enumerable.Range(0, 5).Select(x => Board[yPos, x]).ToArray();
                if (RowHasBingo(tempRow))
                {
                    Bingo = true;
                    var winning = tempRow.Select(x => x.Number).ToArray();
                    return (true, winning, GetUnwinningNumbers(winning));
                }
            }
            for (var xPos = 0; xPos < 5; xPos++)
            {
                var tempRow = Enumerable.Range(0, 5).Select(y => Board[y, xPos]).ToArray();
                if (RowHasBingo(tempRow))
                {
                    Bingo = true;
                    var winning = tempRow.Select(x => x.Number).ToArray();
                    return (true, winning, GetUnwinningNumbers(winning));
                }
            }
            return (false, null, null);
        }

        private int[]? GetUnwinningNumbers(int[] nums)
        {
            var result = new List<int>();
            for (var y = 0; y < 5; y++)
            {
                for (var x = 0; x < 5; x++)
                {
                    if (nums.Contains(Board[y, x].Number) == false && Board[y, x].Hit == false)
                        result.Add(Board[y, x].Number);

                }
            }
            return result.ToArray();
        }

        private static bool RowHasBingo(Spot[] vs)
        {
            return vs.All(x => x.Hit == true);
        }
    }

    private class Spot
    {
        public bool Hit { get; set; }
        public int Number { get; private set; }
        public Spot(int number)
        {
            Number = number;
            Hit = false;
        }
    }
}
