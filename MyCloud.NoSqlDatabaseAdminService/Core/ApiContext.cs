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
        public List<DatabaseUser> Users { get; set; }

        public ApiContext()
        {
            Projects = new List<Project>();
            Users = new List<DatabaseUser>();
        }


        public void PopulateWithDemoData()
        {
            var projectGuid = Guid.Parse("00000000-0000-0000-0000-000000000000");
            Projects.Add(new Project(projectGuid, "VideoStreamPro", "The new great streaming portal."));
            Users.Add(new DatabaseUser
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
        }
    }
}
