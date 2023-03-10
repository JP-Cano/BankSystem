namespace Domain.Model.Entities.Users
{
    /// <summary>
    /// Clase <see cref="User"/>
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Full name
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Rol
        /// </summary>
        public Roles Rol { get; private set; }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Create an instance of class <see cref="User"/> with all attributes
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fullName"></param>
        /// <param name="rol"></param>
        public User(string id, string fullName, Roles rol)
        {
            Id = id;
            FullName = fullName;
            Rol = rol;
        }

        /// <summary>
        /// Check if rol is admin
        /// </summary>
        /// <param name="rol"></param>
        /// <returns></returns>
        public bool IsAdmin(Roles rol)
        {
            return rol == Roles.Admin;
        }
    }
}