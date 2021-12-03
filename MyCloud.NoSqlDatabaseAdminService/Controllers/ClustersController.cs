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
    [Route("api/projects/{projectId}/clusters")]
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
        /// <param name="projectId">The id of the project the clusters are assigned to.</param>
        /// <returns></returns>
        /// <response code="200">Clusters retrieved</response>
        /// <response code="400">The request was unacceptable, due to a malformed id.</response>
        /// <response code="401">No valid API key provided</response>
        /// <response code="500">The server encountered an error while processing your request</response>
        [HttpGet(Name = "GetClusters")]
        [ProducesResponseType(typeof(List<Cluster>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<List<Cluster>> Get(Guid projectId)
        {
            return _context.Clusters.Where(cluster => cluster.ProjectId == projectId).ToList();
        }

        /// <summary>
        /// Get a cluster of a project.
        /// </summary>
        /// <param name="projectId">The id of the project the cluster is assigned to.</param>
        /// <param name="clusterId">The id of the cluster to get.</param>
        /// <returns></returns>
        /// <response code="200">Cluster retrieved</response>
        /// <response code="400">The request was unacceptable, due to a malformed id.</response>
        /// <response code="401">No valid API key provided</response>
        /// <response code="404">The cluster with the given id could not be found.</response>
        /// <response code="500">The server encountered an error while processing your request</response>
        [HttpGet("{clusterId}", Name = "GetClusterById")]
        [ProducesResponseType(typeof(Cluster), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Cluster> GetById(Guid projectId, Guid clusterId)
        {
            var cluster = _context.Clusters.FirstOrDefault(cluster => cluster.Id == clusterId && cluster.ProjectId == projectId);
            if (cluster != null)
                return cluster;
            return NotFound();
        }

        /// <summary>
        /// Create a cluster in a project.
        /// </summary>
        /// <param name="projectId">The id of the project to create the cluster for.</param>
        /// <param name="addCluster">The cluster to create.</param>
        /// <returns></returns>
        /// <response code="200">Cluster created</response>
        /// <response code="400">The request was unacceptable, due to a malformed id.</response>        // TODO: only id or also body?
        /// <response code="401">No valid API key provided</response>
        /// <response code="500">The server encountered an error while processing your request</response>
        [HttpPost(Name = "PostCluster")]
        [ProducesResponseType(typeof(Cluster), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Cluster> Post(Guid projectId, [FromBody] Cluster addCluster)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            addCluster.ProjectId = projectId;
            var verifyNewGuid = new Cluster(addCluster);
            _context.Clusters.Add(verifyNewGuid);
            return CreatedAtAction(nameof(GetById), new { projectId, clusterId = verifyNewGuid.Id }, verifyNewGuid);
        }

        /// <summary>
        /// Remove a cluster from a project.
        /// </summary>
        /// <param name="projectId">The project id the cluster belongs to.</param>
        /// <param name="clusterId">The id of the cluster to remove.</param>
        /// <returns></returns>
        /// <response code="204">Cluster deleted</response>
        /// <response code="400">The request was unacceptable, due to a malformed id.</response>
        /// <response code="401">No valid API key provided</response>
        /// <response code="404">The cluster with the given id could not be found.</response>
        /// <response code="500">The server encountered an error while processing your request</response>
        [HttpDelete("{clusterId}", Name = "DeleteCluster")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteById(Guid projectId, Guid clusterId)
        {
            var cluster = _context.Clusters.FirstOrDefault(cluster => cluster.Id == clusterId && cluster.ProjectId == projectId);
            if (cluster != null)
            {
                _context.Clusters.Remove(cluster);
                // HTTP 204 No Content: The server successfully processed the request, but is not returning any content
                // https://www.w3.org/Protocols/rfc2616/rfc2616-sec9.html#:~:text=A%20successful%20response%20SHOULD%20be%20200
                return NoContent();
            }

            return NotFound();
        }

        /// <summary>
        /// Updates a cluster record in a project.
        /// </summary>
        /// <param name="projectId">The project id the cluster belongs to.</param>
        /// <param name="clusterId"></param>
        /// <param name="patchParameters">The updated record.</param>
        /// <returns></returns>
        /// <response code="200">Cluster updated</response>
        /// <response code="400">The request was unacceptable, due to a malformed id.</response>
        /// <response code="401">No valid API key provided</response>
        /// <response code="404">The cluster with the given id could not be found.</response>
        /// <response code="500">The server encountered an error while processing your request</response>
        [HttpPatch("{clusterId}", Name = "PatchCluster")]
        [ProducesResponseType(typeof(Cluster), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Cluster> Patch(Guid projectId, [FromRoute] Guid clusterId, [FromBody] JsonPatchDocument<Cluster> patchParameters)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var cluster = _context.Clusters.FirstOrDefault(cluster => cluster.Id == clusterId && cluster.ProjectId == projectId);
            if (cluster != null)
            {
                // TODO: update operations
                patchParameters.Operations.RemoveAll(op => !(op.path is "/name" || op.path.StartsWith("/version") || op.path.StartsWith("/configuration") || op.path.StartsWith("/regionConfiguration")));
                patchParameters.ApplyTo(cluster, ModelState);
                return !ModelState.IsValid ? ValidationProblem(ModelState) : Ok(cluster);
            }

            return !ModelState.IsValid ? ValidationProblem(ModelState) : NotFound();
        }
    }
}
