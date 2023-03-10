using System;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Transactions;

namespace EntryPoints.ReactiveWeb.Entities.Commands;

/// <summary>
/// Comando para crear una entidad de tipo <see cref="Transaction"/>
/// </summary>
public class CrearTransacción
{
    /// <summary>
    /// Id de entidad <see cref="Account"/>
    /// </summary>
    public string IdCuenta { get; set; }

    /// <summary>
    /// Date en que se hizo el movimiento
    /// </summary>
    public DateTime FechaMovimiento { get; set; }

    /// <summary>
    /// Tipo de transacción
    /// </summary>
    public TransactionType TransactionType { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public decimal Valor { get; set; }

    /// <summary>
    /// Balance inicial
    /// </summary>
    public decimal SaldoInicial { get; set; }

    /// <summary>
    /// Balance final
    /// </summary>
    public decimal SaldoFinal { get; set; }

    /// <summary>
    /// Description
    /// </summary>
    public string Descripción { get; set; }
}