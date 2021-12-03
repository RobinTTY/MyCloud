using MyCloud.NoSqlDatabaseAdminService.Models;

namespace MyCloud.NoSqlDatabaseAdminService.Core
{
    /// <summary>
    /// In-memory "database" for demo data.
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Contains the existing <see cref="Project"/>s.
        /// </summary>
        public List<Project> Projects { get; set; }
        /// <summary>
        /// Contains the existing <see cref="User"/>s.
        /// </summary>
        public List<User> Users { get; set; }
        /// <summary>
        /// Contains the existing <see cref="Invoice"/>s.
        /// </summary>
        public List<Invoice> Invoices { get; set; }
        /// <summary>
        /// Contains the existing <see cref="Cluster"/>s.
        /// </summary>
        public List<Cluster> Clusters { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Database"/>.
        /// </summary>
        public Database()
        {
            Projects = new List<Project>();
            Users = new List<User>();
            Invoices = new List<Invoice>();
            Clusters = new List<Cluster>();
        }

        /// <summary>
        /// Populates the database with demo data.
        /// </summary>
        public void PopulateWithDemoData()
        {
            var projectGuid = new Guid("97a1dc68-0809-4b7b-8b5f-97020925c19b");
            var secondaryProjectGuid = new Guid("df164112-aa29-4764-8f93-95233399d024");
            var mainUserGuid = new Guid("6a630182-c38e-4311-9995-c0178f59e254");
            var secondaryUserGuid = new Guid("03080c12-59bc-4b62-b377-7f1ca5290d88");
            var mainClusterGuid = new Guid("b4c5634d-2576-4996-9244-69a9aa429ffe");
            var secondaryClusterGuid = new Guid("2e64769e-62db-4beb-9bfc-0dfef91ade78");
            var invoice1Guid = new Guid("1baff4d6-adfd-47c0-8b05-9144da4ef9b5");
            var invoice2Guid = new Guid("d514ec00-4a34-4035-86d0-fea096608ac1");
            var invoice3Guid = new Guid("620ba3e6-6667-40be-bad7-50a12b83bc57");

            // Projects
            Projects.Add(new Project(projectGuid, "VideoStreamPro", "The new great streaming portal."));
            Projects.Add(new Project(secondaryProjectGuid, "Project Legacy", "Not available"));
            // Users
            Users.Add(new User
            {
                Id = mainUserGuid,
                Username = "Robin",
                Password = "MySuperSecretPassword",
                ProjectId = projectGuid,
                Roles = new List<Role>
                {
                    new(mainClusterGuid, "read"),
                    new(mainClusterGuid, "write"),
                    new(secondaryClusterGuid, "read")
                },
                Scopes = new List<Guid>
                {
                    mainClusterGuid,
                    secondaryClusterGuid
                }
            });
            Users.Add(new User
            {
                Id = secondaryUserGuid,
                Username = "Tom",
                Password = "password123",
                ProjectId = projectGuid,
                Roles = new List<Role>
                {
                    new(mainClusterGuid, "read"),
                    new(mainClusterGuid, "write"),
                    new(secondaryClusterGuid, "dbOwner")
                },
                Scopes = new List<Guid>
                {
                    mainClusterGuid,
                    secondaryClusterGuid
                }
            });
            // Invoices
            Invoices.Add(new Invoice
            {
                Id = invoice1Guid,
                ProjectId = projectGuid,
                AmountBilledCents = 7146,
                AmountPaidCents = 0,
                BillingPeriodStart = new DateTime(2021, 10, 1, 0, 0, 0),
                BillingPeriodEnd = new DateTime(2021, 10, 31, 0, 0, 0),
                Created = new DateTime(2021, 11, 1, 0, 0, 32),
                Updated = new DateTime(2021, 11, 1, 0, 0, 32),
                Status = InvoiceStatus.Invoiced
            });
            Invoices.Add(new Invoice
            {
                Id = invoice2Guid,
                ProjectId = projectGuid,
                AmountBilledCents = 8371,
                AmountPaidCents = 8371,
                BillingPeriodStart = new DateTime(2021, 9, 1, 0, 0, 0),
                BillingPeriodEnd = new DateTime(2021, 9, 30, 0, 0, 0),
                Created = new DateTime(2021, 10, 1, 0, 0, 19),
                Updated = new DateTime(2021, 10, 21, 14, 11, 32),
                Status = InvoiceStatus.Paid
            });
            Invoices.Add(new Invoice
            {
                Id = invoice3Guid,
                ProjectId = secondaryProjectGuid,
                AmountBilledCents = 18046,
                AmountPaidCents = 18046,
                BillingPeriodStart = new DateTime(2020, 2, 1, 0, 0, 0),
                BillingPeriodEnd = new DateTime(2020, 2, 28, 0, 0, 0),
                Created = new DateTime(2020, 3, 1, 0, 0, 32),
                Updated = new DateTime(2020, 3, 23, 17, 6, 49),
                Status = InvoiceStatus.Paid
            });
            // Clusters
            Clusters.Add(new Cluster
            {
                Id = mainClusterGuid,
                ProjectId = projectGuid,
                Name = "MyDbCluster",
                Configuration = new ClusterConfiguration
                {
                    AutoScalingEnabled = true,
                    BackupEnabled = true,
                    DiskSizeGb = 2000,
                    NumberOfNodes = 3
                },
                RegionConfiguration = new List<RegionConfiguration>
                {
                    new()
                    {
                        Region = "eu-central-1",
                        Priority = 10
                    },
                    new()
                    {
                        Region = "us-west-2",
                        Priority = 9
                    },
                    new()
                    {
                        Region = "ap-northeast-1",
                        Priority = 9
                    }
                },
                ConnectionString = "cluster://node1.example.com:32020,node2.example.com:32020,node3.example.com:32020",
                CreatedDate = DateTime.Now,
                Version = new SoftwareVersion(1,4,2)
            });
            Clusters.Add(new Cluster
            {
                Id = new Guid("6bdfeed3-86f0-4ad5-9e6d-2d42dd1b6e90"),
                ProjectId = projectGuid,
                Name = "My unwanted Db cluster",
                Configuration = new ClusterConfiguration
                {
                    AutoScalingEnabled = true,
                    BackupEnabled = false,
                    DiskSizeGb = 6000,
                    NumberOfNodes = 4
                },
                RegionConfiguration = new List<RegionConfiguration>
                {
                    new()
                    {
                        Region = "ap-central-1",
                        Priority = 10
                    },
                    new()
                    {
                        Region = "us-east-1",
                        Priority = 9
                    }
                },
                ConnectionString = "cluster://node1.example.com:32020,node2.example.com:32020,node3.example.com:32020,node4.example.com:32020",
                CreatedDate = DateTime.Now,
                Version = new SoftwareVersion(1, 5, 0)
            });
        }
    }
}
