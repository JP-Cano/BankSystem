using Domain.Model.Entities.Clientes;
using DrivenAdapters.Mongo.Entities.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Clients;

namespace DrivenAdapters.Mongo.Entities
{
    /// <summary>
    /// DTO de entidad <see cref="Account"/>
    /// </summary>
    public class CuentaEntity : EntityBase
    {
        /// <summary>
        /// Id de entidad <see cref="Client"/>
        /// </summary>
        [JsonProperty("idCliente")]
        [BsonElement("idCliente")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdCliente { get; set; }

        /// <summary>
        /// Numero de Cuenta
        /// </summary>
        [BsonElement("numeroCuenta")]
        public string NumeroCuenta { get; set; }

        /// <summary>
        /// Enum <see cref="AccountType"/>
        /// </summary>
        [JsonConverter(typeof(AccountType))]
        [BsonRepresentation(BsonType.String)]
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Enum <see cref="AccountStatus"/>
        /// </summary>
        [JsonConverter(typeof(AccountStatus))]
        [BsonRepresentation(BsonType.String)]
        public AccountStatus AccountStatus { get; set; }

        /// <summary>
        /// Balance
        /// </summary>
        [BsonElement("saldo")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Saldo { get; set; }

        /// <summary>
        /// Balance Disponible
        /// </summary>
        [BsonElement("saldo_disponible")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal SaldoDisponible { get; set; }

        /// <summary>
        /// Excenta GMF
        /// </summary>
        [BsonElement("exenta")]
        public bool Exenta { get; private set; }

        /// <summary>
        /// Historial de Modificaciones de la cuenta
        /// </summary>
        [BsonElement("historial_modificaciones")]
        public List<Modification> HistorialModificaciones { get; private set; }
    }
}