using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AdventCode.Utils
{
    public interface IAdventWebClient
    {
        public Task<string?> GetDayInput(int day, CancellationToken ct = default);
        public Task<List<T>> GetDayInputList<T>(int day, CancellationToken ct = default);
    }

    public class AdventWebClient : IAdventWebClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AdventWebClient> _logger;
        private static readonly Dictionary<string, WebResponse> ResponseDictionary = new();

        public AdventWebClient(HttpClient httpClient, ILogger<AdventWebClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<T>> GetDayInputList<T>(int day, CancellationToken ct = default)
        {
            var response = await GetDayInput(day, ct);
            if (response == null)
            {
                return new List<T>();
            }
            if (typeof(T) == typeof(int))
            {
                return response.Split("\n").Select(x => int.TryParse(x, out var lineValue) ? lineValue : throw new ArgumentException("Provided value was not a number")).ToList() as List<T> ?? new List<T>();
            }
            if (typeof(T) == typeof(string))
            {
                return response.Split("\n").ToList() as List<T> ?? new List<T>();
            }
            _logger.LogError("Unhandled type parsing for list {ListType}", typeof(T));
            return new List<T>();
        }

        public async Task<string?> GetDayInput(int day, CancellationToken ct = default)
        {
            var response = await GetUrl<WebResponse>($"2021/day/{day}/input", ct);
            return response.ResponseString?.Trim();
        }

        private async Task<T> GetUrl<T>(string url, CancellationToken ct = default) where T : WebResponse, new()
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
                response.RequestUrl = url;
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

    public class WebResponse
    {
        public string? RequestUrl { get; set; }
        public byte[]? ResponseData { get; set; }
        public string? ResponseString { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public HttpResponseHeaders? ResponseHeaders { get; set; }
        public bool IsSuccess { get; set; }
        public Exception? Exception { get; set; }


    }
}