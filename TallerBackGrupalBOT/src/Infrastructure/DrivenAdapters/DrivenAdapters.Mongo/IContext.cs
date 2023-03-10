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
        public IMongoCollection<UsuarioEntity> Usuarios { get; }

        /// <summary>
        /// Colección de tipo <see cref="Transaction"/>
        /// </summary>
        public IMongoCollection<TransacciónEntity> Transacciones { get; }

        /// <summary>
        /// Colección de tipo <see cref="Clientes"/>
        /// </summary>
        public IMongoCollection<ClienteEntity> Clientes { get; }

        /// <summary>
        /// Colección de tipo <see cref="Cuentas"/>
        /// </summary>
        public IMongoCollection<CuentaEntity> Cuentas { get; }
    }
}