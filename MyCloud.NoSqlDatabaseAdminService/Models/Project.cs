namespace MyCloud.NoSqlDatabaseAdminService.Models;

/// <summary>
/// A project organizes resources within a common scope.
/// </summary>
public class Project
{
    /// <summary>
    /// The unique id of the project.
    /// </summary>
    /// <example>97a1dc68-0809-4b7b-8b5f-97020925c19b</example>
    public Guid Id { get; set; }
    /// <summary>
    /// The name of the project.
    /// </summary>
    /// <example>My example project.</example>
    public string Name { get; set; }
    /// <summary>
    /// The project description.
    /// </summary>
    /// <example>My project description.</example>
    public string Description { get; set; }
    /// <summary>
    /// Timestamp of when the project was created.
    /// </summary>
    /// <example>2021-11-30T08:52:43.8249333+01:00</example>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Creates a new instance of <see cref="Project"/>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    public Project(string name, string description = "")
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.Now;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Creates a new instance of <see cref="Project"/>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    public Project(Guid id, string name, string description = "")
    {
        Id = id;
        CreatedDate = DateTime.Now;
        Name = name;
        Description = description;
    }
}