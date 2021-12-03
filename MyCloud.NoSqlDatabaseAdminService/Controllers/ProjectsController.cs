using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers;

/// <summary>
/// Provides access to <see cref="Project"/> resources.
/// </summary>
[ApiController]
[Route("api/projects")]
[ApiVersion("1.0")]
[ApiKeyAuth]
[SwaggerTag("Projects organize database resources within a common scope")]
public class ProjectsController : ControllerBase
{
    private readonly Database _context;
    private readonly ILogger<ProjectsController> _logger;

    /// <summary>
    /// Creates a new instance of <see cref="ProjectsController"/>.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public ProjectsController(Database context, ILogger<ProjectsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all projects.
    /// </summary>
    /// <param name="offset">Offset, starting with zero, which determines the current page.</param>
    /// <param name="limit">Number of items that are returned per page.</param>
    /// <returns></returns>
    [HttpGet(Name = "GetProjects")]
    public ActionResult<List<Project>> Get([FromQuery] int offset = 0, [FromQuery] int limit = 500)
    {
        return _context.Projects.Skip(offset * limit).Take(limit).ToList();
    }

    /// <summary>
    /// Get a project.
    /// </summary>
    /// <param name="projectId">The id of the project to get.</param>
    /// <returns>The <see cref="Project"/> with the given id.</returns>
    [HttpGet("{projectId}", Name = "GetProjectById")]
    public ActionResult<Project> GetById(Guid projectId)
    {
        var project = _context.Projects.FirstOrDefault(project => project.Id == projectId);
        if (project != null)
            return project;
        return NotFound();
    }

    /// <summary>
    /// Create a new project.
    /// </summary>
    /// <param name="postParameters">The project object to add.</param>
    /// <returns></returns>
    [HttpPost(Name = "PostProject")]
    public ActionResult<Project> Post([FromBody] ProjectPostParameters postParameters)
    {
        if (postParameters.Name == null || postParameters.Description == null)
            return BadRequest();
        var project = new Project(postParameters.Name, postParameters.Description);
        _context.Projects.Add(project);
        return CreatedAtAction(nameof(GetById), new { projectId = project.Id }, project);
    }

    /// <summary>
    /// Delete a project.
    /// </summary>
    /// <param name="projectId">The id of the project to delete.</param>
    /// <returns></returns>
    [HttpDelete("{projectId}", Name = "DeleteProject")]
    public ActionResult DeleteById(Guid projectId)
    {
        var project = _context.Projects.FirstOrDefault(project => project.Id == projectId);
        var unresolvedResourcesExist = _context.Users.Any(user => user.ProjectId == projectId) || _context.Clusters.Any(cluster => cluster.ProjectId == projectId) || _context.Invoices.Where(invoice => invoice.ProjectId == projectId).Any(invoice => invoice.Status != InvoiceStatus.Paid);
        if (unresolvedResourcesExist)
        {
            return Conflict(new
            {
                type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8",
                title = "The project still has resources assigned to it or you have unpaid invoices. Resolve these issues to delete the project.",
                status = StatusCodes.Status409Conflict,
                traceId = Guid.NewGuid()
            });
        }


        if (project != null)
        {
            _context.Projects.Remove(project);
            // HTTP 204 No Content: The server successfully processed the request, but is not returning any content
            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#:~:text=A%20successful%20response%20SHOULD%20be%20200
            return NoContent();
        }

        return NotFound();
    }

    /// <summary>
    /// Update a project.
    /// </summary>
    /// <param name="projectId">The id of the project to update.</param>
    /// <param name="patchParameters">A <a href="https://datatracker.ietf.org/doc/html/rfc6902">JSON patch document</a> containing the update operations.</param>
    /// <returns></returns>
    [HttpPatch("{projectId}", Name = "PatchProject")]
    public ActionResult<Project> Patch(Guid projectId, [FromBody] JsonPatchDocument<Project> patchParameters)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var project = _context.Projects.FirstOrDefault(project => project.Id == projectId);
        if (project != null)
        {
            patchParameters.Operations.RemoveAll(op => op.path is not ("/name" or "/description"));
            patchParameters.ApplyTo(project, ModelState);
            return !ModelState.IsValid ? ValidationProblem(ModelState) : Ok(project);
        }

        return NotFound();
    }

    /// <summary>
    /// Parameters needed for a post request of the projects endpoint.
    /// </summary>
    public class ProjectPostParameters
    {
        /// <summary>
        /// The name of the project.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// The project description.
        /// </summary>
        public string? Description { get; set; }
    }
}