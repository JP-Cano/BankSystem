using System.Collections.Generic;
using System.Threading.Tasks;
using credinet.exception.middleware.models;
using Domain.Model.Entities.Gateway;
using Domain.Model.Entities.Users;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils.Extensions;

namespace Domain.UseCase.Users
{
    /// <summary>
    /// Caso de uso de entidad <see cref="User"/>
    /// </summary>
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Crea una instancia del caso de uso <see cref="UserUseCase"/>
        /// </summary>
        /// <param name="userRepository"></param>
        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Devuelve todas las entidades de tipo <see cref="User"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> FindAll()
        {
            return await _userRepository.FindAllAsync();
        }

        /// <summary>
        /// Devuelve una entidad de tipo <see cref="User"/> por su Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<User> FindById(string userId)
        {
            User user = await _userRepository.FindByIdAsync(userId);
            if (user is null)
            {
                throw new BusinessException(BusinessTypeException.EntityNotFound.GetDescription(),
                    (int)BusinessTypeException.EntityNotFound);
            }

            return user;
        }

        /// <summary>
        /// Crea una entidad de tipo <see cref="User"/>
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<User> Create(User user)
        {
            return await _userRepository.CreateAsync(user);
        }
    }
}