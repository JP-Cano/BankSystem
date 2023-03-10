using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Users;

namespace Domain.UseCase.Users
{
    /// <summary>
    /// Interfaz de caso de uso de entidad <see cref="User"/>
    /// </summary>
    public interface IUserUseCase
    {
        /// <summary>
        /// Método para obtener todos los usuarios
        /// </summary>
        /// <returns></returns>
        Task<List<User>> FindAll();

        /// <summary>
        /// Método para obtener un user por Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> FindById(string userId);

        /// <summary>
        /// Método para crear un user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> Create(User user);
    }
}