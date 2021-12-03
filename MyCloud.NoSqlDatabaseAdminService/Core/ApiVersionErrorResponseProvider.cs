using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace MyCloud.NoSqlDatabaseAdminService.Core;

/// <summary>
/// 
/// </summary>
public class ApiVersionErrorResponseProvider : DefaultErrorResponseProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public override IActionResult CreateResponse(ErrorResponseContext context)
    {
        //You can initialize your own class here. Below is just a sample.
        context.Request.Headers.TryGetValue("api-version", out var apiVersion);
        var errorResponse = new
        {
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            Title = "Unsupported Api Version",
            Description = $"The HTTP resource that matches the request URI '{context.Request.Path}' does not support the API version '{apiVersion}'.",
            Status = "400",
            TraceId = Guid.NewGuid()
        };
        var response = new ObjectResult(errorResponse)
        {
            StatusCode = (int)HttpStatusCode.BadRequest
        };

        return response;
    }
}

