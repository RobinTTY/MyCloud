using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MyCloud.NoSqlDatabaseAdminService.Models
{
    /// <summary>
    /// A cluster of database servers, containing x nodes.
    /// </summary>
    public class Cluster
    {
        /// <summary>
        /// The unique id of the cluster.
        /// </summary>
        /// <example>b4c5634d-2576-4996-9244-69a9aa429ffe</example>
        public Guid Id { get; set; }
        /// <summary>
        /// The project id this cluster is associated with.
        /// </summary>
        /// <example>b4c5634d-2576-4996-9244-69a9aa429ffe</example>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// The name of the cluster.
        /// </summary>
        /// <example>My Database Cluster</example>
        public string? Name { get; set; }
        /// <summary>
        /// The connection string which is used to connect to the cluster.
        /// </summary>
        /// <example>cluster://node1.example.com:32020,node2.example.com:32020</example>
        public string? ConnectionString { get; set; }
        /// <summary>
        /// Timestamp of when the cluster was created.
        /// </summary>
        /// <example>2021-11-30T07:58:43.2697702+01:00</example>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The current software version deployed on the nodes of the cluster.
        /// </summary>
        public SoftwareVersion? Version { get; set; }
        /// <summary>
        /// Contains cluster configuration options.
        /// </summary>
        [Required]
        public ClusterConfiguration? Configuration { get; set; }
        /// <summary>
        /// The region configuration of the server which determines where to deploy individual nodes.
        /// </summary>
        [Required]
        public List<RegionConfiguration>? RegionConfiguration { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Cluster"/>.
        /// </summary>
        public Cluster() { }

        /// <summary>
        /// Creates a new instance of <see cref="Cluster"/>.
        /// </summary>
        /// <param name="cluster"></param>
        public Cluster(Cluster cluster)
        {
            Id = Guid.NewGuid();
            ProjectId = cluster.ProjectId;
            Name = cluster.Name;
            CreatedDate = DateTime.Now;
            Version = cluster.Version ?? new SoftwareVersion(1, 6, 0);
            Configuration = cluster.Configuration;
            RegionConfiguration = cluster.RegionConfiguration;
            if (Configuration != null) ConnectionString = GetClusterConnectionString(Configuration.NumberOfNodes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numberOfNodes"></param>
        /// <returns></returns>
        public string GetClusterConnectionString(uint numberOfNodes)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("cluster://");
            for (var i = 0; i < numberOfNodes; i++)
            {
                stringBuilder.Append($"node{i}-{GetRandomString().ToLower()}.example.com:32020,");
            }

            return stringBuilder.ToString().Trim(',');
        }

        /// <summary>
        /// Generates a pseudo random string;
        /// </summary>
        /// <returns></returns>
        public string GetRandomString()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }

    /// <summary>
    /// Configuration options for a <see cref="Cluster"/>.
    /// </summary>
    public class ClusterConfiguration
    {
        /// <summary>
        /// The number of nodes to deploy.
        /// </summary>
        /// <example>7</example>
        public uint NumberOfNodes { get; set; } = 1;

        /// <summary>
        /// The disk size of each node.
        /// </summary>
        /// <example>1000</example>
        public uint DiskSizeGb { get; set; } = 1000;

        /// <summary>
        /// Enables auto scaling of the cluster which automatically determines the optimal
        /// CPU, memory and disk size configuration.
        /// </summary>
        /// <example>true</example>
        public bool AutoScalingEnabled { get; init; } = true;

        /// <summary>
        /// Enables the generation of automatic backups.
        /// </summary>
        /// <example>true</example>
        public bool BackupEnabled { get; init; } = true;
    }

    /// <summary>
    /// Configuration which determines where to deploy individual nodes.
    /// </summary>
    public class RegionConfiguration
    {
        /// <summary>
        /// Physical location to deploy a node of the cluster to.
        /// </summary>
        /// <example>eu-north-1</example>
        [Required]
        public string? Region { get; set; } = "eu-west-1";
        /// <summary>
        /// Priority of the region. The highest priority is 10, the lowest is 0.
        /// Regions with higher priority will be preferred when deploying nodes.
        /// </summary>
        /// <example>7</example>
        public uint Priority { get; set; } = 10;
    }

    /// <summary>
    /// Identifies the version of a software product.
    /// </summary>
    public class SoftwareVersion
    {
        /// <summary>
        /// The major version number of the software.
        /// </summary>
        /// <example>1</example>
        public uint Major { get; set; }
        /// <summary>
        /// The minor version number of the software.
        /// </summary>
        /// <example>2</example>
        public uint Minor { get; set; }
        /// <summary>
        /// The patch version number of the software.
        /// </summary>
        /// <example>0</example>
        public uint Patch { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="SoftwareVersion"/>.
        /// </summary>
        /// <param name="major">The major version number of the software.</param>
        /// <param name="minor">The minor version number of the software.</param>
        /// <param name="patch">The patch version number of the software.</param>
        public SoftwareVersion(uint major, uint minor, uint patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }
    }
}
