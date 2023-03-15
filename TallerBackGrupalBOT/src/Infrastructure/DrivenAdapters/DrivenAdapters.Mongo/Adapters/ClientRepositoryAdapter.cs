using AutoMapper;
using Domain.Model.Entities.Clientes;
using Domain.Model.Entities.Gateway;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model.Entities.Clients;

namespace DrivenAdapters.Mongo.Adapters
{
    /// <summary>
    /// <see cref="IClientRepository"/>
    /// </summary>
    public class ClientRepositoryAdapter : IClientRepository
    {
        private readonly FilterDefinitionBuilder<ClientEntity> filtro = Builders<ClientEntity>.Filter;

        private readonly IMongoCollection<ClientEntity> _collection;

        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor de <see cref="IClientRepository"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public ClientRepositoryAdapter(IContext context, IMapper mapper)
        {
            _collection = context.Clients;
            _mapper = mapper;
        }

        /// <summary>
        /// <see cref="IClientRepository.UpdateAsync"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Client> UpdateAsync(string clientId, Client client)
        {
            await _collection.ReplaceOneAsync(
                filtro.Eq(x => x.Id, clientId),
                _mapper.Map<ClientEntity>(client));

            return client;
        }

        /// <summary>
        /// <see cref="IClientRepository.CreateAsync"/>
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public async Task<Client> CreateAsync(string userId, Client client)
        {
            var nuevoCliente = _mapper.Map<ClientEntity>(client);
            await _collection.InsertOneAsync(nuevoCliente);

            return _mapper.Map<Client>(nuevoCliente);
        }

        /// <summary>
        /// <see cref="IClientRepository.FindByIdAsync"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<Client> FindByIdAsync(string clientId)
        {
            var filtroMetodo = Builders<ClientEntity>.Filter.Eq(x => x.Id, clientId);
            var cursor = await _collection.Find(filtroMetodo).FirstOrDefaultAsync();
            return _mapper.Map<Client>(cursor);
        }

        /// <summary>
        /// <see cref="IClientRepository.FindByIdNumber"/>
        /// </summary>
        /// <param name="idNumber"></param>
        /// <returns></returns>
        public async Task<Client> FindByIdNumber(string idNumber)
        {
            var cursor = await _collection.FindAsync<ClientEntity>(
                filtro.Eq(x => x.IdNumber, idNumber));
            return _mapper.Map<Client>(cursor.FirstOrDefault());
        }

        /// <summary>
        /// <see cref="IClientRepository.FindAllAsync"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Client>> FindAllAsync()
        {
            var cursor = await _collection.FindAsync<ClientEntity>(_ => true);
            return cursor.ToList()
                .Select(x => _mapper.Map<Client>(x))
                .ToList();
        }
    }
}