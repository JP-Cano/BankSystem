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
    private readonly IMongoCollection<TransactionEntity> _mongoTransacciónCollection;
    private readonly IMongoCollection<AccountEntity> _mongoCuentaCollection;
    private readonly IMapper _mapper;

    /// <summary>
    /// Crea una instancia de repositorio <see cref="TransactionRepositoryAdapter"/>
    /// </summary>
    /// <param name="mongoDb"></param>
    /// <param name="mapper"></param>
    public TransactionRepositoryAdapter(IContext mongoDb, IMapper mapper)
    {
        _mongoTransacciónCollection = mongoDb.Transactions;
        _mongoCuentaCollection = mongoDb.Accounts;
        _mapper = mapper;
    }

    /// <summary>
    /// <see cref="ITransactionRepository.FindByIdAsync"/>
    /// </summary>
    /// <param name="transactionId"></param>
    /// <returns></returns>
    public async Task<Transaction> FindByIdAsync(string transactionId)
    {
        IAsyncCursor<TransactionEntity> transacciónCursor =
            await _mongoTransacciónCollection.FindAsync(transacción => transacción.Id == transactionId);

        TransactionEntity transactionSeleccionada = await transacciónCursor.FirstOrDefaultAsync();

        return transactionSeleccionada is null ? null : _mapper.Map<Transaction>(transactionSeleccionada);
    }

    /// <summary>
    /// <see cref="ITransactionRepository.FindByAccountIdAsync"/>
    /// </summary>
    /// <param name="accountId"></param>
    /// <returns></returns>
    public async Task<List<Transaction>> FindByAccountIdAsync(string accountId)
    {
        IAsyncCursor<AccountEntity> cuentaCursor =
            await _mongoCuentaCollection.FindAsync(cuenta => cuenta.Id == accountId);

        AccountEntity accountEntity = await cuentaCursor.FirstOrDefaultAsync();

        IAsyncCursor<TransactionEntity> transacciónCursor =
            await _mongoTransacciónCollection.FindAsync(transacción => transacción.AccountId == accountEntity.Id);

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
        var transacciónEntity = _mapper.Map<TransactionEntity>(transaction);
        await _mongoTransacciónCollection.InsertOneAsync(transacciónEntity);

        return _mapper.Map<Transaction>(transacciónEntity);
    }
}