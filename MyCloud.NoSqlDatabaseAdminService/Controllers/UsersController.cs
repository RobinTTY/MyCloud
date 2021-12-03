using System.Diagnostics;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers;

/// <summary>
/// Provides access to <see cref="User"/> resources.
/// </summary>
[ApiController]
[Route("api/projects/{projectId}/users")]
[ApiVersion("1.0")]
[ApiKeyAuth]
[SwaggerTag("User accounts provide access to clusters.")]
public class UsersController : ControllerBase
{
    private readonly Database _context;
    private readonly ILogger<ProjectsController> _logger;

    /// <summary>
    /// Creates new instance of <see cref="UsersController"/>.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    // TODO: Add id to user
    public UsersController(Database context, ILogger<ProjectsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all users of a project.
    /// </summary>
    /// <param name="projectId">The id of the project the users are assigned to.</param>
    /// <returns></returns>
    [HttpGet(Name = "GetUsers")]
    public ActionResult<List<User>> Get(Guid projectId)
    {
        return _context.Users.Where(user => user.ProjectId == projectId).ToList();
    }

    /// <summary>
    /// Get a user of a project.
    /// </summary>
    /// <param name="projectId">The id of the project the users are assigned to.</param>
    /// <param name="userId">The username of the user to get.</param>
    /// <returns></returns>
    [HttpGet("{userId}", Name = "GetUserByName")]
    public ActionResult<User> GetById(Guid projectId, Guid userId)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == userId && user.ProjectId == projectId);
        if (user != null)
            return user;
        return NotFound();
    }

    /// <summary>
    /// Create a user in a project.
    /// </summary>
    /// <param name="projectId">The id of the project to create the user for.</param>
    /// <param name="addUser">The user to create.</param>
    /// <returns></returns>
    [HttpPost(Name = "PostUser")]
    public ActionResult<User> Post(Guid projectId, [FromBody] User addUser)
    {
        if (_context.Users.Exists(user => user.Username == addUser.Username))
        {
            ModelState.SetModelValue("Username", new ValueProviderResult("error"));
            return Conflict(new { type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8", title = $"An existing user with the username '{addUser.Username}' already exists.", status = StatusCodes.Status409Conflict, traceId = Guid.NewGuid() });

        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        // This could probably be done better with ModelState?!
        addUser.ProjectId = projectId;
        var verifyNewGuid = new User(addUser);
        _context.Users.Add(verifyNewGuid);
        return CreatedAtAction(nameof(GetById), new { projectId, userId = verifyNewGuid.Id }, verifyNewGuid);
    }

    /// <summary>
    /// Remove a user from a project.
    /// </summary>
    /// <param name="projectId">The project id the user belongs to.</param>
    /// <param name="userId">The name of the user to remove.</param>
    /// <returns></returns>
    [HttpDelete("{userId}", Name = "DeleteUser")]
    public ActionResult DeleteById(Guid projectId, Guid userId)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == userId && user.ProjectId == projectId);
        if (user != null)
        {
            _context.Users.Remove(user);
            // HTTP 204 No Content: The server successfully processed the request, but is not returning any content
            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#:~:text=A%20successful%20response%20SHOULD%20be%20200
            return NoContent();
        }

        return NotFound();
    }

    /// <summary>
    /// Updates a user record in a project.
    /// </summary>
    /// <param name="projectId">The project id the user belongs to.</param>
    /// <param name="userId">The name of the user to update.</param>
    /// <param name="patchParameters">The updated record.</param>
    /// <returns></returns>
    [HttpPatch("{userId}", Name = "PatchUser")]
    public ActionResult<User> Patch(Guid projectId, Guid userId, [FromBody] JsonPatchDocument<User> patchParameters)
    {
        var user = _context.Users.FirstOrDefault(user => user.Id == userId && user.ProjectId == projectId);
        if (user != null)
        {
            patchParameters.Operations.RemoveAll(op => op.path is "/projectId");
            patchParameters.ApplyTo(user, ModelState);
            return !ModelState.IsValid ? ValidationProblem(ModelState) : Ok(user);
        }

        return NotFound();
    }
}