namespace MyCloud.NoSqlDatabaseAdminService.Models
{
    public class DatabaseUser
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Array of this user's roles.  A role allows the user to perform particular actions on the specified database.
        /// </summary>
        public List<Role> Roles { get; set; }
        /// <summary>
        /// Unique <see cref="Guid"/> that identifies the project to which the database user belongs.
        /// </summary>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// List of databases/clusters that this user can access.
        /// </summary>
        public string Scopes { get; set; }
    }

    public class Role
    {
        public string DatabaseName { get; set; }
        public string RoleName { get; set; }
    }
}
