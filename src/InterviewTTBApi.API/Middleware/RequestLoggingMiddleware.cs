using System.Diagnostics;

namespace InterviewTTBApi.API.Middleware;

public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        logger.LogInformation("→ {Method} {Path}", context.Request.Method, context.Request.Path);

        await next(context);

        sw.Stop();
        logger.LogInformation("← {Status} {Method} {Path} ({Elapsed}ms)",
            context.Response.StatusCode,
            context.Request.Method,
            context.Request.Path,
            sw.ElapsedMilliseconds);
    }
}
