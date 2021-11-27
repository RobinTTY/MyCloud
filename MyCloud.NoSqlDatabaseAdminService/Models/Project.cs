namespace MyCloud.NoSqlDatabaseAdminService.Models;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }

    public Project(string name, string description = "")
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
        Name = name;
        Description = description;
    }

    public Project(Guid id, string name, string description = "")
    {
        Id = id;
        CreatedDate = DateTime.Now;
        Name = name;
        Description = description;
    }
}