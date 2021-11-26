namespace MyCloud.NoSqlDatabaseAdminService.Models;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }

    public Project(string name)
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
        Name = name;
    }
}