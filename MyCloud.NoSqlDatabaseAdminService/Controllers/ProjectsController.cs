using Microsoft.AspNetCore.Mvc;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class ProjectsController : ControllerBase
{
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(ILogger<ProjectsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetProjects")]
    public string Get()
    {
        return "wow";
    }
}