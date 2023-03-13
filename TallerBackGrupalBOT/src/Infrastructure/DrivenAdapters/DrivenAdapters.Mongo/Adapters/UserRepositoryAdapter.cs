using AutoMapper;
using Domain.Model.Entities.Gateway;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model.Entities.Users;

namespace DrivenAdapters.Mongo.Adapters;

/// <summary>
/// Adaptador de entidad <see cref="User"/>
/// </summary>
public class UserRepositoryAdapter : IUserRepository
{
    private readonly IMongoCollection<UserEntity> _mongoUsuarioCollection;
    private readonly IMapper _mapper;

    /// <summary>
    /// Crea una instancia de repositorio <see cref="UserRepositoryAdapter"/>
    /// </summary>
    /// <param name="mongoDb"></param>
    /// <param name="mapper"></param>
    public UserRepositoryAdapter(IContext mongoDb, IMapper mapper)
    {
        _mongoUsuarioCollection = mongoDb.Usuarios;
        _mapper = mapper;
    }

    /// <summary>
    /// <see cref="IUserRepository.FindAllAsync"/>
    /// </summary>
    /// <returns></returns>
    public async Task<List<User>> FindAllAsync()
    {
        IAsyncCursor<UserEntity> usuarioCursor = await _mongoUsuarioCollection
            .FindAsync(Builders<UserEntity>.Filter.Empty);

        return usuarioCursor
            .ToList()
            .Select(usuario => _mapper.Map<User>(usuario))
            .ToList();
    }

    /// <summary>
    /// <see cref="IUserRepository.FindByIdAsync"/>
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task<User> FindByIdAsync(string userId)
    {
        IAsyncCursor<UserEntity> cursor = await _mongoUsuarioCollection.FindAsync(c => c.Id == userId);
        UserEntity userEntity = await cursor.FirstOrDefaultAsync();

        return userEntity == null ? null : _mapper.Map<User>(userEntity);
    }

    /// <summary>
    /// <see cref="IUserRepository.CreateAsync"/>
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<User> CreateAsync(User user)
    {
        var usuarioEntity = _mapper.Map<UserEntity>(user);
        await _mongoUsuarioCollection.InsertOneAsync(usuarioEntity);

        return _mapper.Map<User>(usuarioEntity);
    }
}