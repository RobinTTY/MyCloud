﻿using MyCloud.NoSqlDatabaseAdminService.Models;

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
            var mainClusterGuid = new Guid("b4c5634d-2576-4996-9244-69a9aa429ffe");
            var secondaryClusterGuid = new Guid("2e64769e-62db-4beb-9bfc-0dfef91ade78");
            var invoice1Guid = new Guid("1baff4d6-adfd-47c0-8b05-9144da4ef9b5");
            var invoice2Guid = new Guid("d514ec00-4a34-4035-86d0-fea096608ac1");

            // Projects
            Projects.Add(new Project(projectGuid, "VideoStreamPro", "The new great streaming portal."));
            // Users
            Users.Add(new User
            {
                Username = "Robin",
                Password = "MySuperSecretPassword",
                ProjectId = projectGuid,
                Roles = new List<Role>
                {
                    new(mainClusterGuid, "read"),
                    new(mainClusterGuid, "write"),
                    new(secondaryClusterGuid, "read")
                },
                Scopes = new List<string>
                {
                    "MyDb",
                    "MySecondDb"
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
                BillingPeriodStart = new DateTime(2021, 10, 1, 0, 0, 0),
                BillingPeriodEnd = new DateTime(2021, 10, 31, 0, 0, 0),
                Created = new DateTime(2021, 10, 1, 0, 0, 19),
                Updated = new DateTime(2021, 10, 21, 14, 11, 32),
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
        }
    }
}