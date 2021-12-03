using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyCloud.NoSqlDatabaseAdminService.Core;

/// <summary>
/// Implements a fake authorization layer.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
{
    /// <summary>
    /// Checks the fake api key.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <returns></returns>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var requiredApiKey = "4ce8eb35-23ee-4364-8209-0e45928948c2";

        if (!context.HttpContext.Request.Headers.TryGetValue("api-key", out var apiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if (apiKey != requiredApiKey)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        // TODO: content encoding doesn't work, investigate, fake for now
        context.HttpContext.Response.Headers.Add("Content-Encoding", "gzip, br");

        await next();
    }
}