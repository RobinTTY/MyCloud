using MyCloud.NoSqlDatabaseAdminService.Models;

namespace MyCloud.NoSqlDatabaseAdminService.Core
{
    public interface IApiContext
    {
        void PopulateWithDemoData();
    }

    public class ApiContext : IApiContext
    {
        public List<Project> Projects { get; set; }
        public List<User> Users { get; set; }
        public List<Invoice> Invoices { get; set; }
        
        public ApiContext()
        {
            Projects = new List<Project>();
            Users = new List<User>();
            Invoices = new List<Invoice>();
        }


        public void PopulateWithDemoData()
        {
            var projectGuid = Guid.Parse("97a1dc68-0809-4b7b-8b5f-97020925c19b");
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
                    new("MyDb", "read"),
                    new("MyDb", "write"),
                    new("MySecondDb", "read")
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
                Id = new Guid("1baff4d6-adfd-47c0-8b05-9144da4ef9b5"),
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
                Id = new Guid("d514ec00-4a34-4035-86d0-fea096608ac1"),
                ProjectId = projectGuid,
                AmountBilledCents = 8371,
                AmountPaidCents = 8371,
                BillingPeriodStart = new DateTime(2021, 10, 1, 0, 0, 0),
                BillingPeriodEnd = new DateTime(2021, 10, 31, 0, 0, 0),
                Created = new DateTime(2021, 10, 1, 0, 0, 19),
                Updated = new DateTime(2021, 10, 21, 14, 11, 32),
                Status = InvoiceStatus.Paid
            });
        }
    }
}
