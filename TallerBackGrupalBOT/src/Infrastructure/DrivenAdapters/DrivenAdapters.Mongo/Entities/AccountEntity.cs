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
    public class AccountEntity : EntityBase
    {
        /// <summary>
        /// Id de entidad <see cref="Client"/>
        /// </summary>
        [JsonProperty("client_id")]
        [BsonElement("client_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }

        /// <summary>
        /// Numero de Cuenta
        /// </summary>
        [BsonElement("account_number")]
        public string AccountNumber { get; set; }

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
        [BsonElement("balance")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Balance { get; set; }

        /// <summary>
        /// Balance Disponible
        /// </summary>
        [BsonElement("available_balance")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal AvailableBalance { get; set; }

        /// <summary>
        /// Excenta GMF
        /// </summary>
        [BsonElement("exempt")]
        public bool Exempt { get; private set; }

        /// <summary>
        /// Historial de Modificaciones de la cuenta
        /// </summary>
        [BsonElement("modification_history")]
        public List<Modification> ModificationHistory { get; private set; }
    }
}