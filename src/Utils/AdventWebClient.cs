using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace AdventCode.Utils;

public interface IAdventWebClient
{
    public Task<string?> GetDayInputAsync(int day, CancellationToken ct = default);
    public Task<List<T>> GetDayInputListAsync<T>(int day, CancellationToken ct = default);
}

public class AdventWebClient : IAdventWebClient
{
    private const string InputFileDirectory = "input";
    private readonly HttpClient _httpClient;
    private readonly ILogger<AdventWebClient> _logger;
    private static readonly Dictionary<string, WebResponse> ResponseDictionary = new();

    public AdventWebClient(HttpClient httpClient, ILogger<AdventWebClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<T>> GetDayInputListAsync<T>(int day, CancellationToken ct = default)
    {
        var response = await GetDayInputAsync(day, ct);
        return response.ToDataList<T>();
    }

    public async Task<string?> GetDayInputAsync(int day, CancellationToken ct = default)
    {
        var response = await GetUrlAsync<WebResponse>($"{AdventUtils.GetCurrentYear()}/day/{day}/input", ct);
        return response.ResponseString?.Trim();
    }



    private async Task<T> GetUrlAsync<T>(string url, CancellationToken ct = default) where T : WebResponse, new()
    {
        var response = new T();
        try
        {
            if (ResponseDictionary.ContainsKey(url))
            {
                if (ResponseDictionary[url] is T returnResp)
                {
                    return returnResp;
                }
            }
            var parsedFileName = $"{url.Replace($"{AdventUtils.GetCurrentYear()}/", "").Replace("/", "-")}.txt";
            response.RequestUrl = url;
            var outputFile = $"{InputFileDirectory}/{AdventUtils.GetCurrentYear()}/{parsedFileName}";
            if (File.Exists(outputFile))
            {
                response.StatusCode = HttpStatusCode.OK;
                response.ResponseData = await File.ReadAllBytesAsync(outputFile, ct);
                response.ResponseString = await File.ReadAllTextAsync(outputFile, ct);
                ResponseDictionary[url] = response;
                return response;
            }
            var httpResponse = await _httpClient.GetAsync(url, ct);
            response.StatusCode = httpResponse.StatusCode;
            response.ResponseHeaders = httpResponse.Headers;
            response.IsSuccess = httpResponse.IsSuccessStatusCode;
            if (response.IsSuccess || response.StatusCode == HttpStatusCode.ServiceUnavailable || response.StatusCode == HttpStatusCode.NotFound)
            {
                try
                {
                    var byteArray = await httpResponse.Content.ReadAsByteArrayAsync(ct);
                    var respString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                    response.ResponseData = byteArray;
                    response.ResponseString = respString;
                    ResponseDictionary[url] = response;
                    if (response.IsSuccess)
                    {
                        if (Directory.Exists(InputFileDirectory) == false)
                        {
                            Directory.CreateDirectory(InputFileDirectory);
                        }
                        if (Directory.Exists($"{InputFileDirectory}/{AdventUtils.GetCurrentYear()}") == false)
                        {
                            Directory.CreateDirectory($"{InputFileDirectory}/{AdventUtils.GetCurrentYear()}");
                        }
                        await File.WriteAllTextAsync(outputFile, respString, Encoding.UTF8, ct);
                    }

                }
                catch (Exception)
                {
                    if (response.IsSuccess)//we don't care about the service unavailable errors
                        throw;
                }
                if ("Please don't repeatedly request this endpoint before it unlocks! The calendar countdown is synchronized with the server time; the link will be enabled on the calendar the instant this puzzle becomes available.".EqualsIgnoreCase(response.ResponseString?.Trim()))
                    throw new NoDataException();

            }
        }
        catch (NoDataException)
        {
            throw;
        }
        catch (Exception e)
        {
            response.Exception = e;
            response.IsSuccess = false;
        }
        return response;
    }

}