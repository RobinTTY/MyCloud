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
            Projects.Add(new Project("VideoStreamPro"));
        }
    }
}
