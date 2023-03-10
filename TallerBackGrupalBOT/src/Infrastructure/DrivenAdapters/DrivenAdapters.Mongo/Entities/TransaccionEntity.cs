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
public class TransacciónEntity : EntityBase
{
    /// <summary>
    /// Id de entidad <see cref="Account"/>
    /// </summary>
    [JsonProperty("idCuenta")]
    [BsonElement("idCuenta")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdCuenta { get; set; }

    /// <summary>
    /// Date De movimiento
    /// </summary>
    [BsonElement("fecha_movimiento")]
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime FechaMovimiento { get; set; }

    /// <summary>
    /// Enum <see cref="TransactionType"/>
    /// </summary>
    [JsonConverter(typeof(TransactionType))]
    [BsonRepresentation(BsonType.String)]
    public TransactionType TransactionType { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    [BsonElement("valor")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Valor { get; set; }

    /// <summary>
    /// Balance inicial
    /// </summary>
    [BsonElement("saldo_inicial")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal SaldoInicial { get; set; }

    /// <summary>
    /// Balance final
    /// </summary>
    [BsonElement("saldo_final")]
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal SaldoFinal { get; set; }

    /// <summary>
    /// Descripcion
    /// </summary>
    [BsonElement("descripcion")] public string Descripción { get; set; }
}