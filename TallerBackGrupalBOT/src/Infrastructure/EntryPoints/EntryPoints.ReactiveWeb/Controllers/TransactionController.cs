using AutoMapper;
using Domain.UseCase.Common;
using EntryPoints.ReactiveWeb.Base;
using EntryPoints.ReactiveWeb.Entities.Commands;
using EntryPoints.ReactiveWeb.Entities.Handlers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Transactions;
using Domain.UseCase.Transactions;

namespace EntryPoints.ReactiveWeb.Controllers;

/// <summary>
/// Controller de entidad <see cref="Transaction"/>
/// </summary>
[Produces("application/json")]
[ApiVersion("1.0")]
[Route("api/[controller]/[action]")]
public class TransactionController : AppControllerBase<TransactionController>
{
    private readonly ITransactionUseCase _transactionUseCase;
    private readonly IMapper _mapper;

    /// <summary>
    /// Crea una instancia de <see cref="TransactionController"/>
    /// </summary>
    /// <param name="eventsService"></param>
    /// <param name="transactionUseCase"></param>
    /// <param name="mapper"></param>
    public TransactionController(IManageEventsUseCase eventsService, ITransactionUseCase transactionUseCase,
        IMapper mapper) : base(eventsService)
    {
        _transactionUseCase = transactionUseCase;
        _mapper = mapper;
    }

    /// <summary>
    /// Endpoint que retorna una entidad de tipo <see cref="Transaction"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<IActionResult> FindTransactionById([FromRoute] string id) =>
        HandleRequest(async () =>
        {
            Transaction transaction = await _transactionUseCase.FindTransactionById(id);
            return _mapper.Map<TransactionHandler>(transaction);
        }, "");

    /// <summary>
    /// Endpoint que retorna una entidad de tipo <see cref="Transaction"/> por el Id de la entidad
    /// <see cref="Account"/>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public Task<IActionResult> FindTransactionsByAccountId([FromRoute] string id) =>
        HandleRequest(async () =>
        {
            IEnumerable<Transaction> transacciones = await _transactionUseCase.FindTransactionsByAccountId(id);
            return _mapper.Map<IEnumerable<TransactionHandler>>(transacciones);
        }, "");

    /// <summary>
    /// Endpoint para realizar una consignación
    /// </summary>
    /// <param name="createTransaction"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IActionResult> MakeDeposit([FromBody] CreateTransaction createTransaction) =>
        HandleRequest(async () =>
        {
            Transaction transaction =
                await _transactionUseCase.MakeDeposit(_mapper.Map<Transaction>(createTransaction));

            return _mapper.Map<TransactionHandler>(transaction);
        }, "");

    /// <summary>
    /// Endpoint para realizar un retiro
    /// </summary>
    /// <param name="createTransaction"></param>
    /// <returns></returns>
    [HttpPost]
    public Task<IActionResult> MakeWithdrawal([FromBody] CreateTransaction createTransaction) =>
        HandleRequest(async () =>
        {
            Transaction transaction =
                await _transactionUseCase.MakeWithdrawal(_mapper.Map<Transaction>(createTransaction));

            return _mapper.Map<TransactionHandler>(transaction);
        }, "");

    /// <summary>
    /// Endpoint para realizar una transferencia
    /// </summary>
    /// <param name="createTransaction"></param>
    /// <param name="idCuentaReceptor"></param>
    /// <returns></returns>
    [HttpPost("{idCuentaReceptor}")]
    public Task<IActionResult> MakeTransfer([FromBody] CreateTransaction createTransaction,
        [FromRoute] string idCuentaReceptor) =>
        HandleRequest(async () =>
        {
            Transaction transaction =
                await _transactionUseCase.MakeTransfer(_mapper.Map<Transaction>(createTransaction), idCuentaReceptor);

            return _mapper.Map<TransactionHandler>(transaction);
        }, "");
}