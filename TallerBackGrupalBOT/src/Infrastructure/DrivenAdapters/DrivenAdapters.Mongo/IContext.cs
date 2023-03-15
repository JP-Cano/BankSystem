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
        public IMongoCollection<UserEntity> Users { get; }

        /// <summary>
        /// Colección de tipo <see cref="Transaction"/>
        /// </summary>
        public IMongoCollection<TransactionEntity> Transactions { get; }

        /// <summary>
        /// Colección de tipo <see cref="Clients"/>
        /// </summary>
        public IMongoCollection<ClientEntity> Clients { get; }

        /// <summary>
        /// Colección de tipo <see cref="Accounts"/>
        /// </summary>
        public IMongoCollection<AccountEntity> Accounts { get; }
    }
}