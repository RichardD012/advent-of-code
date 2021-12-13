using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AdventCode.Utils;

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