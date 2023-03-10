using AutoMapper;
using Domain.Model.Entities.Gateway;
using DrivenAdapters.Mongo.entities;
using DrivenAdapters.Mongo.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model.Entities.Transactions;

namespace DrivenAdapters.Mongo.Adapters;

/// <summary>
/// Adaptador de entidad <see cref="Transaction"/>
/// </summary>
public class TransactionRepositoryAdapter : ITransactionRepository
{
    private readonly IMongoCollection<TransacciónEntity> _mongoTransacciónCollection;
    private readonly IMongoCollection<CuentaEntity> _mongoCuentaCollection;
    private readonly IMapper _mapper;

    /// <summary>
    /// Crea una instancia de repositorio <see cref="TransactionRepositoryAdapter"/>
    /// </summary>
    /// <param name="mongoDb"></param>
    /// <param name="mapper"></param>
    public TransactionRepositoryAdapter(IContext mongoDb, IMapper mapper)
    {
        _mongoTransacciónCollection = mongoDb.Transacciones;
        _mongoCuentaCollection = mongoDb.Cuentas;
        _mapper = mapper;
    }

    /// <summary>
    /// <see cref="ITransactionRepository.FindByIdAsync"/>
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    public async Task<Transaction> FindByIdAsync(string transactionId)
    {
        IAsyncCursor<TransacciónEntity> transacciónCursor =
            await _mongoTransacciónCollection.FindAsync(transacción => transacción.Id == transactionId);

        TransacciónEntity transacciónSeleccionada = await transacciónCursor.FirstOrDefaultAsync();

        return transacciónSeleccionada is null ? null : _mapper.Map<Transaction>(transacciónSeleccionada);
    }

    /// <summary>
    /// <see cref="ITransactionRepository.FindByAccountIdAsync"/>
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<List<Transaction>> FindByAccountIdAsync(string accountId)
    {
        IAsyncCursor<CuentaEntity> cuentaCursor =
            await _mongoCuentaCollection.FindAsync(cuenta => cuenta.Id == accountId);

        CuentaEntity cuentaEntity = await cuentaCursor.FirstOrDefaultAsync();

        IAsyncCursor<TransacciónEntity> transacciónCursor =
            await _mongoTransacciónCollection.FindAsync(transacción => transacción.IdCuenta == cuentaEntity.Id);

        return transacciónCursor
            .ToList()
            .Select(transacción => _mapper.Map<Transaction>(transacción))
            .ToList();
    }

    /// <summary>
    /// <see cref="ITransactionRepository.CreateAsync"/>
    /// </summary>
    /// <param name="transaction"></param>
    /// <returns></returns>
    public async Task<Transaction> CreateAsync(Transaction transaction)
    {
        var transacciónEntity = _mapper.Map<TransacciónEntity>(transaction);
        await _mongoTransacciónCollection.InsertOneAsync(transacciónEntity);

        return _mapper.Map<Transaction>(transacciónEntity);
    }
}