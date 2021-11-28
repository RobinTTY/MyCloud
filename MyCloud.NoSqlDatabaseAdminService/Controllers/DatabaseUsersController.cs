using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers;

[ApiController]
[Route("api/projects/{id}/databaseUsers")]
[ApiVersion("1.0")]
public class DatabaseUsersController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<ProjectsController> _logger;

    public DatabaseUsersController(ApiContext context, ILogger<ProjectsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet(Name = "GetUsers")]
    public ActionResult<List<DatabaseUser>> Get(Guid id)
    {
        return _context.Users.Where(user => user.ProjectId == id).ToList();
    }

    [HttpGet("{username}", Name = "GetUserByName")]
    public ActionResult<DatabaseUser> GetById(string username)
    {
        var user = _context.Users.FirstOrDefault(user => user.Username == username);
        if (user != null)
            return user;
        return NotFound();
    }

    [HttpPost(Name = "PostUser")]
    public ActionResult<DatabaseUser> Post(Guid id, [FromBody] DatabaseUser addUser)
    {
        if (_context.Users.Exists(user => user.Username == addUser.Username && addUser.ProjectId == id))
            // https://stackoverflow.com/questions/3825990/http-response-code-for-post-when-resource-already-exists
            return Conflict();
        _context.Users.Add(addUser);
        return CreatedAtAction(nameof(GetById), new { id = id, username = addUser.Username }, addUser);
    }

    [HttpDelete("{username}", Name = "DeleteUser")]
    public ActionResult DeleteById(Guid id, string username)
    {
        var user = _context.Users.FirstOrDefault(user => user.Username == username && user.ProjectId == id);
        if (user != null)
        {
            _context.Users.Remove(user);
            // HTTP 204 No Content: The server successfully processed the request, but is not returning any content
            // https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#:~:text=A%20successful%20response%20SHOULD%20be%20200
            return NoContent();
        }

        return NotFound();
    }

    [HttpPatch("{username}", Name = "PatchUser")]
    public ActionResult<DatabaseUser> Patch(Guid id, string username, [FromBody] JsonPatchDocument<DatabaseUser> patchParameters)
    {
        var user = _context.Users.FirstOrDefault(user => user.Username == username && user.ProjectId == id);
        if (user != null)
        {
            patchParameters.Operations.RemoveAll(op => op.path is not "/projectId");
            patchParameters.ApplyTo(user);
            return Ok(user);
        }

        return NotFound();
    }
}