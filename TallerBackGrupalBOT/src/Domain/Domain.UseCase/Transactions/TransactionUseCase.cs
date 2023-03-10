using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using credinet.exception.middleware.models;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Gateway;
using Domain.Model.Entities.Transactions;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils;
using Helpers.ObjectsUtils.Extensions;
using Microsoft.Extensions.Options;

namespace Domain.UseCase.Transactions
{
    /// <summary>
    /// Caso de uso de entidad <see cref="Transaction"/>
    /// </summary>
    public class TransactionUseCase : ITransactionUseCase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IOptions<ConfiguradorAppSettings> _options;

        /// <summary>
        /// Crea una instancia del caso de uso <see cref="TransactionUseCase"/>
        /// </summary>
        /// <param name="accountRepository"></param>
        /// <param name="transactionRepository"></param>
        /// <param name="options"></param>
        public TransactionUseCase(IAccountRepository accountRepository,
            ITransactionRepository transactionRepository,
            IOptions<ConfiguradorAppSettings> options)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _options = options;
        }

        /// <summary>
        /// Retorna una entidad de tipo <see cref="Transaction"/> por su Id
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public async Task<Transaction> FindTransactionById(string transactionId)
        {
            Transaction transaction = await _transactionRepository.FindByIdAsync(transactionId);
            if (transaction is null)
            {
                throw new BusinessException(TipoExcepcionNegocio.EntidadNoEncontrada.GetDescription(),
                    (int)TipoExcepcionNegocio.EntidadNoEncontrada);
            }

            return transaction;
        }

        /// <summary>
        /// Retorna las entidades de tipo <see cref="Transaction"/> asociadas por el Id de entidad
        /// de tipo <see cref="Account"/>
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public async Task<List<Transaction>> FindTransactionsByAccountId(string accountId)
        {
            Account accountSeleccionada = await _accountRepository.FindByIdAsync(accountId);

            if (accountSeleccionada is null)
                throw new BusinessException(TipoExcepcionNegocio.CuentaNoEncontrada.GetDescription(),
                    (int)TipoExcepcionNegocio.CuentaNoEncontrada);

            return await _transactionRepository.FindByAccountIdAsync(accountId);
        }

        /// <summary>
        /// Método de tipo <see cref="Transaction"/> que realiza una consignación
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<Transaction> MakeDeposit(Transaction transaction)
        {
            var cuenta = await _accountRepository.FindByIdAsync(transaction.AccountId);

            ValidarEstadoCuenta(cuenta);

            transaction.AssignTransactionType(TransactionType.Deposit);
            transaction.AddMovementDate();
            transaction.AssignInitialBalance(cuenta.Balance);
            transaction.AssignFinalCreditBalance(transaction.Value);
            transaction.GenerateDescription();

            cuenta.UpdateBalance(cuenta.Balance + transaction.Value);

            ValidarNuevoSaldoDisponible(cuenta);

            await _accountRepository.UpdateAsync(transaction.AccountId, cuenta);
            return await _transactionRepository.CreateAsync(transaction);
        }

        /// <summary>
        /// Método de tipo <see cref="Transaction"/> que realiza un retiro
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<Transaction> MakeWithdrawal(Transaction transaction)
        {
            var cuenta = await _accountRepository.FindByIdAsync(transaction.AccountId);
            var valorRetiro = ValidarValorRetiro(transaction.Value, cuenta);

            ValidarEstadoCuenta(cuenta);

            transaction.AssignTransactionType(TransactionType.Withdrawal);
            transaction.AddMovementDate();
            transaction.AssignInitialBalance(cuenta.Balance);
            transaction.AssignDebitFinalBalance(valorRetiro);
            transaction.GenerateDescription();

            cuenta.UpdateBalance(cuenta.Balance - valorRetiro);

            ValidarNuevoSaldoDisponible(cuenta);

            await _accountRepository.UpdateAsync(transaction.AccountId, cuenta);
            return await _transactionRepository.CreateAsync(transaction);
        }

        /// <summary>
        /// Método de tipo <see cref="Transaction"/> que realiza una transferencia
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="idCuentaReceptor"></param>
        /// <returns></returns>
        public async Task<Transaction> MakeTransfer(Transaction transaction, string idCuentaReceptor)
        {
            var cuentaOrigen = await _accountRepository.FindByIdAsync(transaction.AccountId);
            var cuentaDestino = await _accountRepository.FindByIdAsync(idCuentaReceptor);

            if (cuentaDestino is null)
                throw new BusinessException($"La cuenta de destino numero {idCuentaReceptor} no existe",
                (int)TipoExcepcionNegocio.CuentaNoEncontrada);

            if (cuentaDestino.IsCancelled())
                throw new BusinessException(TipoExcepcionNegocio.EstadoCuentaCancelada.GetDescription(),
                    (int)TipoExcepcionNegocio.EstadoCuentaCancelada);

            ValidarEstadoCuenta(cuentaOrigen, transaction.AccountId);

            var valorRetiro = ValidarValorRetiro(transaction.Value, cuentaOrigen);

            transaction.AssignTransactionType(TransactionType.Transfer);
            transaction.AddMovementDate();
            transaction.AssignInitialBalance(cuentaOrigen.Balance);
            transaction.AssignDebitFinalBalance(valorRetiro);
            transaction.GenerateDescription(idCuentaReceptor);

            var transacciónReceptor = CrearTransacciónReceptor(transaction, cuentaDestino);

            cuentaOrigen.UpdateBalance(cuentaOrigen.Balance - valorRetiro);
            cuentaDestino.UpdateBalance(cuentaDestino.Balance + valorRetiro);

            ValidarNuevoSaldoDisponible(cuentaOrigen);
            ValidarNuevoSaldoDisponible(cuentaDestino);

            await _accountRepository.UpdateAsync(transaction.AccountId, cuentaOrigen);
            await _accountRepository.UpdateAsync(idCuentaReceptor, cuentaDestino);

            await _transactionRepository.CreateAsync(transacciónReceptor);
            return await _transactionRepository.CreateAsync(transaction);
        }

        private void ValidarNuevoSaldoDisponible(Account account)
        {
            if (account.Exempt) account.AvailableBalance = account.Balance;
            else account.CalculateAvailableBalance(_options.Value.GMF);
        }

        private static void ValidarEstadoCuenta(Account account, string idCuentaValidar = null)
        {
            if (account is null)
                throw new BusinessException(
                    idCuentaValidar is null ? TipoExcepcionNegocio.CuentaNoEncontrada.GetDescription()
                    : $"La cuenta de origen con Id {idCuentaValidar} no existe",
                (int)TipoExcepcionNegocio.CuentaNoEncontrada);

            if (account.IsCancelled())
                throw new BusinessException(TipoExcepcionNegocio.EstadoCuentaCancelada.GetDescription(),
                    (int)TipoExcepcionNegocio.EstadoCuentaCancelada);

            if (account.IsInactive())
                throw new BusinessException(TipoExcepcionNegocio.EstadoCuentaInactiva.GetDescription(),
                    (int)TipoExcepcionNegocio.EstadoCuentaInactiva);
        }

        private decimal ValidarValorRetiro(decimal valor, Account account)
        {
            var valorConGMF = valor * (1.0M + _options.Value.GMF);
            var saldoConSobregiro = account.Balance + _options.Value.ValorSobregiro;

            if (account.Exempt)
            {
                if (account.AccountType == AccountType.Savings && valor > account.Balance)
                {
                    throw new BusinessException(TipoExcepcionNegocio.ValorRetiroNoPermitido.GetDescription(),
                        (int)TipoExcepcionNegocio.ValorRetiroNoPermitido);
                }

                if (account.AccountType == AccountType.Regular && valor > saldoConSobregiro)
                {
                    throw new BusinessException(TipoExcepcionNegocio.ValorRetiroNoPermitido.GetDescription(),
                        (int)TipoExcepcionNegocio.ValorRetiroNoPermitido);
                }

                return valor;
            }

            if (account.AccountType == AccountType.Savings && valorConGMF > account.Balance)
            {
                throw new BusinessException(TipoExcepcionNegocio.ValorRetiroNoPermitido.GetDescription(),
                    (int)TipoExcepcionNegocio.ValorRetiroNoPermitido);
            }

            if (account.AccountType == AccountType.Regular && valorConGMF > saldoConSobregiro)
            {
                throw new BusinessException(TipoExcepcionNegocio.ValorRetiroNoPermitido.GetDescription(),
                    (int)TipoExcepcionNegocio.ValorRetiroNoPermitido);
            }

            return valorConGMF;
        }

        private Transaction CrearTransacciónReceptor(Transaction transaction, Account accountReceptor)
            => new(accountReceptor.Id,
                transaction.MovementDate,
                TransactionType.Transfer,
                transaction.Value,
                accountReceptor.Balance,
                AsignarSaldoFinalReceptor(accountReceptor.Balance, transaction.Value),
                GenerarDescripciónTransferenciaReceptor(transaction.Value, transaction.AccountId, accountReceptor.Id)
            );

        private decimal AsignarSaldoFinalReceptor(decimal saldoInicial, decimal valor) => saldoInicial + valor;

        private String GenerarDescripciónTransferenciaReceptor(decimal valor, string idCuentaOrigen,
            string idCuentaDestino) =>
            $"Se Recibió Transfer por ${valor} desde la {idCuentaOrigen} a la cuenta {idCuentaDestino}";
    }
}