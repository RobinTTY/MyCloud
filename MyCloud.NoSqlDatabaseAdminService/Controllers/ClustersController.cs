using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MyCloud.NoSqlDatabaseAdminService.Core;
using MyCloud.NoSqlDatabaseAdminService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MyCloud.NoSqlDatabaseAdminService.Controllers
{
    /// <summary>
    /// Provides access to <see cref="Cluster"/> resources.
    /// </summary>
    [ApiController]
    [Route("api/projects/{id}/clusters")]
    [ApiVersion("1.0")]
    [ApiKeyAuth]
    [SwaggerTag("Clusters distribute your data across multiple nodes.")]
    public class ClustersController : ControllerBase
    {
        private readonly Database _context;
        private readonly ILogger<ProjectsController> _logger;

        /// <summary>
        /// Creates a new instance of <see cref="ClustersController"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public ClustersController(Database context, ILogger<ProjectsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Get all clusters of a project.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Clusters retrieved</response>
        /// <response code="400">The request was unacceptable, due to a malformed id.</response>
        /// <response code="401">No valid API key provided</response>
        /// <response code="500">The server encountered an error while processing your request</response>
        [HttpGet(Name = "GetClusters")]
        [ProducesResponseType(typeof(List<Cluster>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Cluster>> Get(Guid id)
        {
            return _context.Clusters.Where(cluster => cluster.ProjectId == id).ToList();
        }

        /// <summary>
        /// Get a cluster of a project.
        /// </summary>
        /// <param name="id">The id of the project the cluster is assigned to.</param>
        /// <param name="clusterId">The id of the cluster to get.</param>
        /// <returns></returns>
        [HttpGet("{clusterId:guid}", Name = "GetClusterById")]
        public ActionResult<Cluster> GetById(Guid id, Guid clusterId)
        {
            var cluster = _context.Clusters.FirstOrDefault(cluster => cluster.Id == clusterId && cluster.ProjectId == id);
            if (cluster != null)
                return cluster;
            return NotFound();
        }

        //[HttpPost(Name = "PostProject")]
        //public ActionResult<Project> Post([FromBody] Cluster cluster)
        //{
        //    _context.Projects.Add(cluster);
        //    return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        //}

        //[HttpDelete("{id:guid}", Name = "DeleteProject")]
        //public ActionResult DeleteById(Guid id)
        //{
        //    var project = _context.Projects.FirstOrDefault(project => project.Id == id);
        //    if (project != null)
        //    {
        //        _context.Projects.Remove(project);
        //        // HTTP 204 No Content: The server successfully processed the request, but is not returning any content
        //        // https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#:~:text=A%20successful%20response%20SHOULD%20be%20200
        //        return NoContent();
        //    }

        //    return NotFound();
        //}

        //[HttpPatch("{id:guid}", Name = "PatchProject")]
        //public ActionResult<Project> Patch(Guid id, [FromBody] JsonPatchDocument<Project> patchParameters)
        //{
        //    var project = _context.Projects.FirstOrDefault(project => project.Id == id);
        //    if (project != null)
        //    {
        //        patchParameters.Operations.RemoveAll(op => op.path is not ("/name" or "/description"));
        //        patchParameters.ApplyTo(project, ModelState);
        //        return !ModelState.IsValid ? ValidationProblem(ModelState) : Ok(project);
        //    }

        //    return NotFound();
        //}
    }
}
