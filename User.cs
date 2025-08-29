namespace ProductManagements
{
    /// <summary>
    /// User model class representing a user in the system
    /// </summary>
    public class User
    {
        /// <summary>
        /// Unique identifier for the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Username for login (must be unique)
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User's password (in real applications, this should be hashed)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User's telephone number (optional)
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// User's address (optional)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Constructor with required fields
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Constructor with all fields
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="telephone">Telephone number</param>
        /// <param name="address">Address</param>
        public User(string username, string password, string telephone, string address)
        {
            Username = username;
            Password = password;
            Telephone = telephone;
            Address = address;
        }

        /// <summary>
        /// Returns string representation of the user (for display purposes)
        /// </summary>
        /// <returns>Username</returns>
        public override string ToString()
        {
            return Username;
        }
    }
}