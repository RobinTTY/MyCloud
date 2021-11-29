using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers;

[ApiController]
[Route("api/projects")]
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

    [HttpGet("{id:guid}", Name = "GetProjectById")]
    public ActionResult<Project> GetById(Guid id)
    {
        var project = _context.Projects.FirstOrDefault(project => project.Id == id);
        if (project != null)
            return project;
        return NotFound();
    }

    [HttpPost(Name = "PostProject")]
    public ActionResult<Project> Post([FromBody] ProjectPostParameters postParameters)
    {
        var project = new Project(postParameters.Name, postParameters.Description);
        _context.Projects.Add(project);
        return CreatedAtAction(nameof(GetById), new { id = project.Id } , project);
    }

    [HttpDelete("{id:guid}", Name = "DeleteProject")]
    public ActionResult DeleteById(Guid id)
    {
        var project = _context.Projects.FirstOrDefault(project => project.Id == id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            // HTTP 204 No Content: The server successfully processed the request, but is not returning any content
            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#:~:text=A%20successful%20response%20SHOULD%20be%20200
            return NoContent();
        }

        return NotFound();
    }

    [HttpPatch("{id:guid}", Name = "PatchProject")]
    public ActionResult<Project> Patch(Guid id, [FromBody] JsonPatchDocument<Project> patchParameters)
    {
        var project = _context.Projects.FirstOrDefault(project => project.Id == id);
        if (project != null)
        {
            patchParameters.Operations.RemoveAll(op => op.path is not ("/name" or "/description"));
            patchParameters.ApplyTo(project, ModelState);
            return !ModelState.IsValid ? ValidationProblem(ModelState) : Ok(project);
        }

        return NotFound();
    }

    public class ProjectPostParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}