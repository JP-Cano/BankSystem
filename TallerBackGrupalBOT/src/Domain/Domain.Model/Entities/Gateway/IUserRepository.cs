using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Users;

namespace Domain.Model.Entities.Gateway
{
    /// <summary>
    /// User repository interface
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Return users
        /// </summary>
        /// <returns></returns>
        Task<List<User>> FindAllAsync();

        /// <summary>
        /// Return user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> FindByIdAsync(string userId);

        /// <summary>
        /// Create an user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateAsync(User user);
    }
}