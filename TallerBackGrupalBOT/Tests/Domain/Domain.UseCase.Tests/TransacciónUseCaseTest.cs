using credinet.exception.middleware.models;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Gateway;
using Domain.Model.Entities.Transactions;
using Domain.Model.Tests;
using Domain.UseCase.Transactions;
using Helpers.Commons.Exceptions;
using Helpers.ObjectsUtils;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Domain.UseCase.Tests
{
    public class TransacciónUseCaseTest
    {
        private readonly ConfiguradorAppSettings _appSettings;
        private readonly Mock<IAccountRepository> _mockCuentaRepository;
        private readonly Mock<ITransactionRepository> _mockTransacciónRepository;
        private readonly Mock<IOptions<ConfiguradorAppSettings>> _mockOptions;
        private readonly TransactionUseCase _transactionUseCase;

        public TransacciónUseCaseTest()
        {
            _appSettings = new ConfiguradorAppSettings
            {
                GMF = 0.004M,
                ValorSobregiro = 3000000
            };

            _mockOptions = new Mock<IOptions<ConfiguradorAppSettings>>();

            _mockCuentaRepository = new();
            _mockTransacciónRepository = new();

            _transactionUseCase = new(_mockCuentaRepository.Object, _mockTransacciónRepository.Object,
                _mockOptions.Object);
        }

        [Fact]
        public async Task ObtenerTransacciónPorId_Exitoso()
        {
            _mockTransacciónRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTransactionTest);

            var transacción = await _transactionUseCase.FindTransactionById(It.IsAny<string>());

            Assert.NotNull(transacción);
            _mockTransacciónRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
        }

        [Fact]
        public async Task ObtenerTransaccionesPorIdCuenta_Exitoso()
        {
            _mockTransacciónRepository
                .Setup(repository => repository.FindByAccountIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTransactionListTest);

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new Account("0"));

            var transacciones = await _transactionUseCase.FindTransactionsByAccountId(It.IsAny<string>());

            Assert.NotNull(transacciones);
            Assert.Equal(GetTransactionListTest().Count, transacciones.Count);
            _mockTransacciónRepository.Verify(mock => mock.FindByAccountIdAsync((It.IsAny<string>())), Times.Once());
        }

        [Fact]
        public async Task RealizarConsignación_Exitoso()
        {
            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValue(100000)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetExemptSavingAccountTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(GetExemptSavingAccountTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(GetTransactionTest);

            var transacción = await _transactionUseCase.MakeDeposit(transacciónTest);

            Assert.NotNull(transacción);

            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Once());
        }

        [Fact]
        public async Task RealizarConsignación_EstadoCuentaCancelada_Retorna_Excepcion()
        {
            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetCancelledAccountTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(GetCancelledAccountTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(GetTransactionTest);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async () =>
                await _transactionUseCase.MakeDeposit(GetTransactionTest()));

            Assert.Equal((int)BusinessTypeException.AccountStateCancelled, businessException.code);
        }

        [Fact]
        public async Task RealizarRetiroCuentaExenta_Exitoso()
        {
            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValue(100000)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetExemptSavingAccountTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(GetExemptSavingAccountTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(GetTransactionTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var transacción = await _transactionUseCase.MakeWithdrawal(transacciónTest);

            Assert.NotNull(transacción);

            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Once());
        }

        [Fact]
        public async Task RealizarRetiroCuentaAhorros_Exenta_MontoARetirarMayorSaldo_LanzaExcepción()
        {
            var valorRetiroMayorASaldo = GetExemptSavingAccountTest().Balance + 1;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValue(valorRetiroMayorASaldo)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetExemptSavingAccountTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)BusinessTypeException.ForbiddenWithdrawalValue, exception.code);

            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Never());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Never());
        }

        [Fact]
        public async Task RealizarRetiroCuentaCorriente_Exenta_MontoARetiraMayorSaldoMasSobregiro_LanzaExcepción()
        {
            var valorRetiroMayorASaldoMasSobregiro =
                GetExemptRegularAccountTest().Balance + _appSettings.ValorSobregiro + 1;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValue(valorRetiroMayorASaldoMasSobregiro)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetExemptRegularAccountTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)BusinessTypeException.ForbiddenWithdrawalValue, exception.code);

            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Never());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Never());
        }

        [Fact]
        public async Task RealizarRetiroCuentaAhorros_NoExenta_MontoARetirarIgualSaldo_LanzaExcepción()
        {
            var valorRetiroIgualSaldo = GetNotExemptSavingAccountTest().Balance;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValue(valorRetiroIgualSaldo)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetNotExemptSavingAccountTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)BusinessTypeException.ForbiddenWithdrawalValue, exception.code);

            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Never());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Never());
        }

        [Fact]
        public async Task RealizarRetiroCuentaCorriente_NoExenta_MontoARetiraIgualSaldoMasSobregiro_LanzaExcepción()
        {
            var valorRetiroIgualASaldoMasSobregiro =
                GetNotExemptRegularAccountTest().Balance + _appSettings.ValorSobregiro + 1;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValue(valorRetiroIgualASaldoMasSobregiro)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetNotExemptRegularAccountTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)BusinessTypeException.ForbiddenWithdrawalValue, exception.code);

            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Never());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Never());
        }

        [Fact]
        public async Task RealizarTransferenciaCuentaExenta_Exitoso()
        {
            var idCuentaReceptor = "2";
            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValue(100000)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(GetExemptSavingAccountTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(GetExemptSavingAccountTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(GetTransactionTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var transacción = await _transactionUseCase.MakeTransfer(transacciónTest, idCuentaReceptor);

            Assert.NotNull(transacción);

            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Exactly(2));
            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Exactly(2));
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Exactly(2));
        }

        #region Private Methods

        private Transaction GetTransactionTest() =>
            new TransactionBuilderTest()
                .WithId("1")
                .WithAccountId("4300000000")
                .WithMovementDate(DateTime.Now)
                .WithTransactionType(TransactionType.Deposit)
                .WithValue(100000)
                .WithInitialBalance(1000000)
                .WithFinalBalance(1100000)
                .Build();

        private List<Transaction> GetTransactionListTest() => new()
        {
            new TransactionBuilderTest()
                .WithId("1")
                .WithAccountId("4600000000")
                .WithMovementDate(DateTime.Now)
                .WithTransactionType(TransactionType.Deposit)
                .WithValue(100000)
                .WithInitialBalance(1000000)
                .WithFinalBalance(1100000)
                .Build(),
            new TransactionBuilderTest()
                .WithId("2")
                .WithAccountId("2300000000")
                .WithMovementDate(DateTime.Now)
                .WithTransactionType(TransactionType.Withdrawal)
                .WithValue(100000)
                .WithInitialBalance(1000000)
                .WithFinalBalance(900000)
                .WithDescription("Se Realizo Withdrawal por $100000 desde la cuenta con ID 2")
                .Build(),
        };

        private Account GetNotExemptSavingAccountTest() => new AccountBuilderTest()
            .WithId("1")
            .WithClientId("123456789")
            .WithAccountNumber("4600000000")
            .WithAccountType(AccountType.Savings)
            .WithAccountStatus(AccountStatus.Active)
            .WithBalance(1000000)
            .WithAvailableBalance(996000)
            .WithExempt(false)
            .Build();

        private Account GetCancelledAccountTest()
        {
            Account account = new AccountBuilderTest()
                .WithId("1")
                .WithClientId("123456789")
                .WithAccountNumber("4600000000")
                .WithAccountType(AccountType.Savings)
                .WithBalance(0)
                .WithAvailableBalance(0)
                .WithExempt(false)
                .Build();

            account.CancelAccount();

            return account;
        }

        private Account GetExemptSavingAccountTest() => new AccountBuilderTest()
            .WithId("1")
            .WithClientId("123456789")
            .WithAccountNumber("4600000000")
            .WithAccountType(AccountType.Savings)
            .WithAccountStatus(AccountStatus.Active)
            .WithBalance(1000000)
            .WithAvailableBalance(996000)
            .WithExempt(true)
            .Build();

        private Account GetNotExemptRegularAccountTest() => new AccountBuilderTest()
            .WithId("1")
            .WithClientId("123456789")
            .WithAccountNumber("4600000000")
            .WithAccountType(AccountType.Regular)
            .WithAccountStatus(AccountStatus.Active)
            .WithBalance(1000000)
            .WithAvailableBalance(996000)
            .WithExempt(false)
            .Build();

        private Account GetExemptRegularAccountTest() => new AccountBuilderTest()
            .WithId("1")
            .WithClientId("123456789")
            .WithAccountNumber("4600000000")
            .WithAccountType(AccountType.Regular)
            .WithAccountStatus(AccountStatus.Active)
            .WithBalance(1000000)
            .WithAvailableBalance(996000)
            .WithExempt(true)
            .Build();

        #endregion Private Methods
    }
}