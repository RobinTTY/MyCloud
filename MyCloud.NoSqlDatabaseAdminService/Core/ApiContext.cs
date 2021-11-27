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

        public ApiContext()
        {
            Projects = new List<Project>();
        }


        public void PopulateWithDemoData()
        {
            Projects.Add(new Project(Guid.Parse("00000000-0000-0000-0000-000000000000"), "VideoStreamPro", "The new great streaming portal."));
        }
    }
}
