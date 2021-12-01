using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventCode.Utils
{
    public interface IAdventWebClient
    {
        public Task<string?> GetDayInput(int day, CancellationToken ct = default);
    }

    public class AdventWebClient : IAdventWebClient
    {
        private readonly HttpClient _httpClient;

        public AdventWebClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                response.RequestUrl = url;
                var httpResponse = await _httpClient.GetAsync(url, ct);
                response.StatusCode = httpResponse.StatusCode;
                response.ResponseHeaders = httpResponse.Headers;
                response.IsSuccess = httpResponse.IsSuccessStatusCode;
                if (response.IsSuccess || response.StatusCode == HttpStatusCode.ServiceUnavailable)
                {
                    try
                    {
                        var byteArray = await httpResponse.Content.ReadAsByteArrayAsync();
                        var respString = Encoding.UTF8.GetString(byteArray, 0, byteArray.Length);
                        response.ResponseData = byteArray;
                        response.ResponseString = respString;
                    }
                    catch (Exception)
                    {
                        if (response.IsSuccess)//we don't care about the service unavailable errors
                            throw;
                    }

                }
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