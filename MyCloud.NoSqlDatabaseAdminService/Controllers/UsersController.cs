﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers;

[ApiController]
[Route("api/projects/{id}/users")]
[ApiVersion("1.0")]
public class UsersController : ControllerBase
{
    private readonly ApiContext _context;
    private readonly ILogger<ProjectsController> _logger;

    public UsersController(ApiContext context, ILogger<ProjectsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Get all the users of a project.
    /// </summary>
    /// <param name="id">The id of the project the users are assigned to.</param>
    /// <returns></returns>
    [HttpGet(Name = "GetUsers")]
    public ActionResult<List<User>> Get(Guid id)
    {
        return _context.Users.Where(user => user.ProjectId == id).ToList();
    }

    /// <summary>
    /// Get a specific user of a project.
    /// </summary>
    /// <param name="username">The username of the user to get.</param>
    /// <returns></returns>
    [HttpGet("{username}", Name = "GetUserByName")]
    public ActionResult<User> GetById(Guid id, string username)
    {
        var user = _context.Users.FirstOrDefault(user => user.Username == username && user.ProjectId == id);
        if (user != null)
            return user;
        return NotFound();
    }

    /// <summary>
    /// Create a user in the project.
    /// </summary>
    /// <param name="id">The id of the project to create the user for.</param>
    /// <param name="addUser">The user to create.</param>
    /// <returns></returns>
    [HttpPost(Name = "PostUser")]
    public ActionResult<User> Post(Guid id, [FromBody] User addUser)
    {
        if (_context.Users.Exists(user => user.Username == addUser.Username && addUser.ProjectId == id))
            // https://stackoverflow.com/questions/3825990/http-response-code-for-post-when-resource-already-exists
            return Conflict();
        _context.Users.Add(addUser);
        return CreatedAtAction(nameof(GetById), new { id = id, username = addUser.Username }, addUser);
    }

    /// <summary>
    /// Removes a user from the project.
    /// </summary>
    /// <param name="id">The project id the user belongs to.</param>
    /// <param name="username">The name of the user to remove.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Updates a user record.
    /// </summary>
    /// <param name="id">The project id the user belongs to.</param>
    /// <param name="username">The name of the user to update.</param>
    /// <param name="patchParameters">The updated record.</param>
    /// <returns></returns>
    [HttpPatch("{username}", Name = "PatchUser")]
    public ActionResult<User> Patch(Guid id, string username, [FromBody] JsonPatchDocument<User> patchParameters, [FromServices] IOptions<ApiBehaviorOptions> apiBehaviorOptions)
    {
        var user = _context.Users.FirstOrDefault(user => user.Username == username && user.ProjectId == id);
        if (user != null)
        {
            patchParameters.Operations.RemoveAll(op => op.path is "/projectId");
            patchParameters.ApplyTo(user, ModelState);
            return !ModelState.IsValid ? ValidationProblem(ModelState) : Ok(user);
        }

        return NotFound();
    }
}