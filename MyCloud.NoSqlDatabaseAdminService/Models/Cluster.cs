namespace MyCloud.NoSqlDatabaseAdminService.Models
{
    public class Cluster
    {
        /// <summary>
        /// The unique id of the cluster.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The project id this cluster is associated with.
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// The name of the cluster.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// The connection string which is used to connect to the cluster.
        /// </summary>
        public string? ConnectionString { get; set; }
        /// <summary>
        /// Timestamp of when the cluster was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// The current software version deployed on the nodes of the cluster.
        /// </summary>
        public Version? Version { get; set; }
        /// <summary>
        /// Contains cluster configuration options.
        /// </summary>
        public ClusterConfiguration? Configuration { get; set; }
        /// <summary>
        /// The region configuration of the server which determines where to deploy individual nodes.
        /// </summary>
        public List<RegionConfiguration>? RegionConfiguration { get; set; }
    }

    public class ClusterConfiguration
    {
        /// <summary>
        /// The number of nodes to deploy.
        /// </summary>
        public uint NumberOfNodes { get; set; }
        /// <summary>
        /// The disk size of each node.
        /// </summary>
        public uint DiskSizeGb { get; set; }
        /// <summary>
        /// Enables auto scaling of the cluster which automatically determines the optimal
        /// CPU, memory and disk size configuration.
        /// </summary>
        public bool AutoScalingEnabled { get; set; }
        /// <summary>
        /// Enables the generation of automatic backups.
        /// </summary>
        public bool BackupEnabled { get; set; }
    }

    public class RegionConfiguration
    {
        /// <summary>
        /// Physical location to deploy a node of the cluster to.
        /// </summary>
        public string? Region { get; set; }
        /// <summary>
        /// Priority of the region. The highest priority is 10, the lowest is 0.
        /// Regions with higher priority will be preferred when deploying nodes.
        /// </summary>
        public uint Priority { get; set; }
    }
}
