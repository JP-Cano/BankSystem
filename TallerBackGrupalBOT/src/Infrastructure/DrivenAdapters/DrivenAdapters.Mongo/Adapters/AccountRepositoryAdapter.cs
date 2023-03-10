using AutoMapper;
using Domain.Model.Entities.Gateway;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;

namespace DrivenAdapters.Mongo.Adapters
{
    /// <summary>
    /// <see cref="IAccountRepository"/>
    /// </summary>
    public class AccountRepositoryAdapter : IAccountRepository
    {
        private readonly IMongoCollection<CuentaEntity> _collectionCuenta;

        private readonly FilterDefinitionBuilder<CuentaEntity> filtro = Builders<CuentaEntity>.Filter;

        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor <see cref="IAccountRepository"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        public AccountRepositoryAdapter(IContext context, IMapper mapper)
        {
            _collectionCuenta = context.Cuentas;
            _mapper = mapper;
        }

        /// <summary>
        /// <see cref="IAccountRepository.UpdateAsync"/>
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> UpdateAsync(string accountId, Account account)
        {
            await _collectionCuenta.ReplaceOneAsync(
                filtro.Eq(x => x.Id, accountId),
                _mapper.Map<CuentaEntity>(account));
            return account;
        }

        /// <summary>
        /// <see cref="IAccountRepository.CreateAsync"/>
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<Account> CreateAsync(Account account)
        {
            var nuevaCuenta = _mapper.Map<CuentaEntity>(account);
            await _collectionCuenta.InsertOneAsync(nuevaCuenta);
            return _mapper.Map<Account>(nuevaCuenta);
        }

        /// <summary>
        /// <see cref="IAccountRepository.FindByClientAsync"/>
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public async Task<List<Account>> FindByClientAsync(string clientId)
        {
            IAsyncCursor<CuentaEntity> cursorCuentas = await _collectionCuenta.FindAsync(Builders<CuentaEntity>.Filter.Empty);

            List<Account> cuentasCliente = cursorCuentas.ToEnumerable().Select(cuentaEntity => _mapper.Map<Account>(cuentaEntity)).ToList();
            List<Account> cuentasClienteFiltradas = cuentasCliente.Where(cuenta => cuenta.ClientId == clientId).ToList();

            return cuentasCliente is null ? null : cuentasClienteFiltradas;
        }

        /// <summary>
        /// <see cref="IAccountRepository.FindByIdAsync"/>
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<Account> FindByIdAsync(string accountId)
        {
            var filter = Builders<CuentaEntity>.Filter.Eq(usuario => usuario.Id, accountId);
            var result = await _collectionCuenta.Find(filter).FirstOrDefaultAsync();
            return result is null ? null : _mapper.Map<Account>(result);
        }

        /// <summary>
        /// <see cref="IAccountRepository.FindAllAsync"/>
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> FindAllAsync()
        {
            IAsyncCursor<CuentaEntity> cursorCuentas = await _collectionCuenta.FindAsync(Builders<CuentaEntity>.Filter.Empty);

            List<Account> cuentas = cursorCuentas.ToEnumerable().Select(cuentaEntity => _mapper.Map<Account>(cuentaEntity)).ToList();
            if (cuentas is null)
            {
                return null;
            }
            return cuentas;
        }
    }
}