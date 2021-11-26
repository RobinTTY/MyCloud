using Microsoft.AspNetCore.Mvc;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class ProjectsController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<ProjectsController> _logger;

    public ProjectsController(ApiContext context, ILogger<ProjectsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet(Name = "GetProjects")]
    public ActionResult<List<Project>> Get()
    {
        return _context.Projects;
    }

    [HttpGet("{id}", Name = "GetProjectById")]
    public ActionResult<Project> GetById(Guid id)
    {
        var project = _context.Projects.FirstOrDefault(project => project.Id == id);
        if (project != null)
            return project;
        return NotFound();
    }

    [HttpPost(Name = "PostProjects")]
    public async Task<ActionResult<Project>> Post([FromQuery]string projectName)
    {
        var project = new Project(projectName);
        _context.Projects.Add(project);
        return CreatedAtAction(nameof(GetById), new { id = project.Id } , project);
    }


}