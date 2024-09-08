namespace GoldenCastle.Govhack2024.Middleware;

public class HttpLoggingHandler : DelegatingHandler
{
    private readonly ILogger<HttpLoggingHandler> _logger;

    public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger)
    {
        _logger = logger;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Guid id = Guid.NewGuid();
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        _logger.LogDebug("[{Id}] Request: {Request}", id, request);
        _logger.LogDebug("[{Id}] Response: {Response}", id, response);
        _logger.LogDebug("[{Id}] Response: {Response.Content}", id, await response.Content.ReadAsStringAsync(cancellationToken));
        return response;
    }
}