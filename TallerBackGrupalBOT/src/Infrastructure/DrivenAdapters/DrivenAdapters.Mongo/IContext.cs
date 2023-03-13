using Domain.Model.Entities.Transactions;
using Domain.Model.Entities.Users;
using DrivenAdapters.Mongo.entities;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;

namespace DrivenAdapters.Mongo
{
    /// <summary>
    /// Interfaz Mongo context contract.
    /// </summary>
    public interface IContext
    {
        /// <summary>
        /// Colección de tipo <see cref="User"/>
        /// </summary>
        public IMongoCollection<UserEntity> Usuarios { get; }

        /// <summary>
        /// Colección de tipo <see cref="Transaction"/>
        /// </summary>
        public IMongoCollection<TransactionEntity> Transacciones { get; }

        /// <summary>
        /// Colección de tipo <see cref="Clientes"/>
        /// </summary>
        public IMongoCollection<ClientEntity> Clientes { get; }

        /// <summary>
        /// Colección de tipo <see cref="Cuentas"/>
        /// </summary>
        public IMongoCollection<AccountEntity> Cuentas { get; }
    }
}