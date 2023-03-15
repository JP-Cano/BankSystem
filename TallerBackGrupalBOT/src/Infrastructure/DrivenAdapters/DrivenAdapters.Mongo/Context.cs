using Domain.Model.Entities.Clientes;
using DrivenAdapters.Mongo.entities;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clients;
using Domain.Model.Entities.Transactions;
using Domain.Model.Entities.Users;

namespace DrivenAdapters.Mongo
{
    /// <summary>
    /// Context is an implementation of <see cref="IContext"/>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Context : IContext
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// crea una nueva instancia de la clase <see cref="Context"/>
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="databaseName"></param>
        public Context(string connectionString, string databaseName)
        {
            MongoClient mongoClient = new MongoClient(connectionString);
            _database = mongoClient.GetDatabase(databaseName);
        }

        /// <summary>
        /// Tipo de contrato <see cref="User"/>
        /// </summary>
        public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>("Users");

        /// <summary>
        /// Tipo de contrato <see cref="Transaction"/>
        /// </summary>
        public IMongoCollection<TransactionEntity> Transactions =>
            _database.GetCollection<TransactionEntity>("Transactions");

        /// <summary>
        /// Colección en DB de <see cref="Client"/>
        /// </summary>
        public IMongoCollection<ClientEntity> Clients => _database.GetCollection<ClientEntity>("Clients");

        /// <summary>
        /// Colección en DB de <see cref="Account"/>
        /// </summary>
        public IMongoCollection<AccountEntity> Accounts => _database.GetCollection<AccountEntity>("Accounts");
    }
}