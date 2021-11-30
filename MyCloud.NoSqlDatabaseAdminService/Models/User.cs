﻿namespace MyCloud.NoSqlDatabaseAdminService.Models
{
    /// <summary>
    /// A database user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The name of the user.
        /// </summary>
        /// <example>Robin</example>
        public string? Username { get; set; }
        /// <summary>
        /// The user password used for authentication.
        /// </summary>
        /// <example>Secret</example>
        public string? Password { get; set; }
        /// <summary>
        /// Array of this user's roles.  A role allows the user to perform particular actions on the specified database.
        /// </summary>
        public List<Role>? Roles { get; set; }
        /// <summary>
        /// Unique <see cref="Guid"/> that identifies the project to which the database user belongs.
        /// </summary>
        /// <example>97a1dc68-0809-4b7b-8b5f-97020925c19b</example>
        public Guid ProjectId { get; set; }
        /// <summary>
        /// List of databases/clusters that this user can access.
        /// </summary>
        public List<string>? Scopes { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="User"/>.
        /// </summary>
        public User(){}

        /// <summary>
        /// Creates a new instance of <see cref="User"/>.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <param name="projectId"></param>
        /// <param name="scopes"></param>
        public User(string? username, List<Role>? roles, Guid projectId, List<string>? scopes)
        {
            Username = username;
            Roles = roles;
            ProjectId = projectId;
            Scopes = scopes;
        }
    }

    /// <summary>
    /// A role a user has for a specific database cluster.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// The id of the cluster the role applies to.
        /// </summary>
        public Guid ClusterId { get; set; }
        /// <summary>
        /// The role that should be applied to the user.
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="Role"/>
        /// </summary>
        /// <param name="clusterId"></param>
        /// <param name="roleName"></param>
        public Role(Guid clusterId, string roleName)
        {
            ClusterId = clusterId;
            RoleName = roleName;
        }
    }
}
