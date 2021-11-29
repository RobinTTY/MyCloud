namespace MyCloud.NoSqlDatabaseAdminService.Models;

public class Project
{
    /// <summary>
    /// The unique id of the project.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the project.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// The project description.
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Timestamp of when the project was created.
    /// </summary>
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