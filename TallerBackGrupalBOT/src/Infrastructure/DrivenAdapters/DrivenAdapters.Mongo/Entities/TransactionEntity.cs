using DrivenAdapters.Mongo.Entities.Base;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Transactions;

namespace DrivenAdapters.Mongo.entities;

/// <summary>
/// DTO de entidad <see cref="Transaction"/>
/// </summary>
public class TransactionEntity : EntityBase
{
    /// <summary>
    /// Id de entidad <see cref="Account"/>
    /// </summary>
    [JsonProperty("account_id")]
    [BsonElement("account_id")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AccountId { get; set; }

    /// <summary>
    /// Date De movimiento
    /// </summary>
    [BsonElement("movement_date")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime MovementDate { get; set; }

    /// <summary>
    /// Enum <see cref="TransactionType"/>
    /// </summary>
    [JsonConverter(typeof(TransactionType))]
    [BsonRepresentation(BsonType.String)]
    public TransactionType TransactionType { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    [BsonElement("value")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Value { get; set; }

    /// <summary>
    /// Balance inicial
    /// </summary>
    [BsonElement("initial_balance")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal InitialBalance { get; set; }

    /// <summary>
    /// Balance final
    /// </summary>
    [BsonElement("final_balance")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal FinalBalance { get; set; }

    /// <summary>
    /// Descripcion
    /// </summary>
    [BsonElement("description")] public string Description { get; set; }
}