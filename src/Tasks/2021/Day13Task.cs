using Google.Cloud.Vision.V1;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AdventCode.Tasks2021;

public class Day13Task : BaseCodeTask, IAdventCodeTask
{
    public override int TaskDay => 13;
    private readonly ILogger<Day13Task> _logger;
    #region TestData
    protected override string TestData => @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";
    #endregion

    public Day13Task(IAdventWebClient client, ILogger<Day13Task> logger) : base(client)
    {
        _logger = logger;
    }

    public override async Task<string?> GetFirstTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var (coordinates, folds) = ParseData(data);
        var newData = FoldData(coordinates, folds[0]);
        var instances = 0;
        for (var y = 0; y < newData.GetLength(0); y++)
        {
            for (var x = 0; x < newData.GetLength(1); x++)
            {
                instances += newData[y, x] != 0 ? 1 : 0;
            }
        }
        return instances.ToString();
    }

    public override async Task<string?> GetSecondTaskAnswerAsync()
    {
        var data = await GetDataAsListAsync<string>();
        var (coordinates, folds) = ParseData(data);
        foreach (var fold in folds)
        {
            coordinates = FoldData(coordinates, fold);
        }
        var code = await ExtractLetters(coordinates);
        return code;
    }
    private async Task<string> ExtractLetters(int[,] coordinates)
    {
        var letterWidth = 4;
        var spacing = 1;
        var pixelWidth = 20;
        var margin = 2;
        var letters = coordinates.GetLength(1) / (letterWidth + spacing);
        using (var ms = new MemoryStream())
        {
            using (var resultImage = new Image<Rgba32>(pixelWidth * ((letters - 1) * (letterWidth + spacing) + letterWidth) + (margin * 2 * pixelWidth), pixelWidth * coordinates.GetLength(0) + (margin * 2 * pixelWidth)))
            {
                resultImage.Mutate(imageContext =>
                {
                    imageContext.BackgroundColor(Rgba32.ParseHex("#FFFFFF"));
                    for (var y = 0; y < coordinates.GetLength(0); y++)
                    {
                        for (var x = 0; x < coordinates.GetLength(1); x++)
                        {
                            if (coordinates[y, x] != 0)
                            {
                                imageContext.Fill(Rgba32.ParseHex("#000000"), new Rectangle(x * pixelWidth + (margin * pixelWidth), y * pixelWidth + (margin * pixelWidth), pixelWidth, pixelWidth));
                            }
                        }
                    }
                });
                resultImage.SaveAsPng($"Day13.png");
            }
        }
        var bytes = File.ReadAllBytes("Day13.png");
        try
        {
            var imageContent = Convert.ToBase64String(bytes);
            var client = ImageAnnotatorClient.Create();
            var image = Google.Cloud.Vision.V1.Image.FromBytes(bytes);
            var response = await client.DetectTextAsync(image);
            var returnString = string.Empty;
            foreach (var annotation in response)
            {
                if (annotation.Description != null)
                {
                    returnString = annotation.Description;
                    break;
                }
            }
            File.Delete("Day13.png");
            return returnString;
        }
        catch (Exception)
        {
            throw new Exception($"Error processing data using Google Vision API - image saved as Day13.png");
        }
    }

    private static int[,] FoldData(int[,] coordinates, Fold fold)
    {
        int[,] responseCoordinates;
        if (fold.Direction == FoldDirection.Y)
        {
            responseCoordinates = new int[fold.Axis, coordinates.GetLength(1)];
            responseCoordinates = SetCoordinates(responseCoordinates, coordinates);
            for (var y = 0; y < responseCoordinates.GetLength(0); y++)
            {
                for (var x = 0; x < responseCoordinates.GetLength(1); x++)
                {
                    responseCoordinates[y, x] += coordinates[coordinates.GetLength(0) - y - 1, x];
                }
            }
        }
        else
        {
            responseCoordinates = new int[coordinates.GetLength(0), fold.Axis];
            responseCoordinates = SetCoordinates(responseCoordinates, coordinates);
            for (var y = 0; y < responseCoordinates.GetLength(0); y++)
            {
                for (var x = 0; x < responseCoordinates.GetLength(1); x++)
                {
                    responseCoordinates[y, x] += coordinates[y, coordinates.GetLength(1) - x - 1];
                }
            }
        }
        return responseCoordinates;
    }

    private static int[,] SetCoordinates(int[,] newData, int[,] coordinates)
    {
        for (var y = 0; y < newData.GetLength(0); y++)
        {
            for (var x = 0; x < newData.GetLength(1); x++)
            {
                newData[y, x] = coordinates[y, x];
            }
        }
        return newData;
    }

    private (int[,], List<Fold>) ParseData(List<string> data)
    {
        var coordinates = new List<(int, int)>();
        var folds = new List<Fold>();
        for (var i = 0; i < data.Count; i++)
        {
            var entry = data[i];
            if (string.IsNullOrEmpty(entry))
            {
                continue;
            }
            if (entry.Contains("fold along"))
            {
                var direction = entry.Contains("y=") ? FoldDirection.Y : FoldDirection.X;
                var axis = int.Parse(entry[(entry.IndexOf("=") + 1)..]);
                folds.Add(new Fold(direction, axis));
            }
            if (entry.Contains(','))
            {
                var coordinate = entry.Split(",");
                coordinates.Add((int.Parse(coordinate[0]), int.Parse(coordinate[1])));
            }
        }
        var returnCoordinates = new int[coordinates.Max(x => x.Item2) + 1, coordinates.Max(x => x.Item1) + 1];
        foreach (var entry in coordinates)
        {
            returnCoordinates[entry.Item2, entry.Item1]++;
        }
        return (returnCoordinates, folds);
    }

    public class Fold
    {
        public FoldDirection Direction { get; set; }
        public int Axis { get; set; }

        public Fold(FoldDirection direction, int axis)
        {
            Direction = direction;
            Axis = axis;
        }
    }

    public enum FoldDirection
    {
        X,
        Y
    }

}
