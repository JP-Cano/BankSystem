using System;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Transactions;

namespace EntryPoints.ReactiveWeb.Entities.Handlers;

/// <summary>
/// Handler DTO de entidad <see cref="Transaction"/>
/// </summary>
public class TransactionHandler
{
    /// <summary>
    /// Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Id de entidad <see cref="Account"/>
    /// </summary>
    public string AccountId { get; set; }

    /// <summary>
    /// Date en que se hizo el movimiento
    /// </summary>
    public DateTime MovementDate { get; set; }

    /// <summary>
    /// Tipo de transacción
    /// </summary>
    public TransactionType TransactionType { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    /// Balance inicial
    /// </summary>
    public decimal InitialBalance { get; set; }

    /// <summary>
    /// Balance final
    /// </summary>
    public decimal FinalBalance { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; set; }
}