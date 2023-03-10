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
                .ReturnsAsync(ObtenerUnaTransacciónTest);

            var transacción = await _transactionUseCase.FindTransactionById(It.IsAny<string>());

            Assert.NotNull(transacción);
            _mockTransacciónRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
        }

        [Fact]
        public async Task ObtenerTransaccionesPorIdCuenta_Exitoso()
        {
            _mockTransacciónRepository
                .Setup(repository => repository.FindByAccountIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerListaTransacciónTest);

            var transacciones = await _transactionUseCase.FindTransactionsByAccountId(It.IsAny<string>());

            Assert.NotNull(transacciones);
            Assert.Equal(ObtenerListaTransacciónTest().Count, transacciones.Count);
            _mockTransacciónRepository.Verify(mock => mock.FindByAccountIdAsync((It.IsAny<string>())), Times.Once());
        }

        [Fact]
        public async Task RealizarConsignación_Exitoso()
        {
            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValor(100000)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaExentaAhorrosTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(ObtenerCuentaExentaAhorrosTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(ObtenerUnaTransacciónTest);

            var transacción = await _transactionUseCase.MakeDeposit(transacciónTest);

            Assert.NotNull(transacción);

            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()), Times.Once());
        }

        [Fact]
        public async Task RealizarConsignación_EstadoCuentaCancelada_Retorna_Excepcion()
        {
            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaCanceladaTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(ObtenerCuentaCanceladaTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(ObtenerUnaTransacciónTest);

            BusinessException businessException = await Assert.ThrowsAsync<BusinessException>(async () =>
                await _transactionUseCase.MakeDeposit(ObtenerUnaTransacciónTest()));

            Assert.Equal((int)TipoExcepcionNegocio.EstadoCuentaCancelada, businessException.code);
        }

        [Fact]
        public async Task RealizarRetiroCuentaExenta_Exitoso()
        {
            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValor(100000)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaExentaAhorrosTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(ObtenerCuentaExentaAhorrosTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(ObtenerUnaTransacciónTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var transacción = await _transactionUseCase.MakeWithdrawal(transacciónTest);

            Assert.NotNull(transacción);

            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()), Times.Once());
        }

        [Fact]
        public async Task RealizarRetiroCuentaAhorros_Exenta_MontoARetirarMayorSaldo_LanzaExcepción()
        {
            var valorRetiroMayorASaldo = ObtenerCuentaExentaAhorrosTest().Balance + 1;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValor(valorRetiroMayorASaldo)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaExentaAhorrosTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)TipoExcepcionNegocio.ValorRetiroNoPermitido, exception.code);

            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Never());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Never());
        }

        [Fact]
        public async Task RealizarRetiroCuentaCorriente_Exenta_MontoARetiraMayorSaldoMasSobregiro_LanzaExcepción()
        {
            var valorRetiroMayorASaldoMasSobregiro =
                ObtenerCuentaExentaCorrienteTest().Balance + _appSettings.ValorSobregiro + 1;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValor(valorRetiroMayorASaldoMasSobregiro)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaExentaCorrienteTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)TipoExcepcionNegocio.ValorRetiroNoPermitido, exception.code);

            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Never());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Never());
        }

        [Fact]
        public async Task RealizarRetiroCuentaAhorros_NoExenta_MontoARetirarIgualSaldo_LanzaExcepción()
        {
            var valorRetiroIgualSaldo = ObtenerCuentaNoExentaAhorrosTest().Balance;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValor(valorRetiroIgualSaldo)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaNoExentaAhorrosTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)TipoExcepcionNegocio.ValorRetiroNoPermitido, exception.code);

            _mockCuentaRepository.Verify(mock => mock.FindByIdAsync((It.IsAny<string>())), Times.Once());
            _mockTransacciónRepository.Verify(mock => mock.CreateAsync((It.IsAny<Transaction>())), Times.Never());
            _mockCuentaRepository.Verify(mock => mock.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()),
                Times.Never());
        }

        [Fact]
        public async Task RealizarRetiroCuentaCorriente_NoExenta_MontoARetiraIgualSaldoMasSobregiro_LanzaExcepción()
        {
            var valorRetiroIgualASaldoMasSobregiro =
                ObtenerCuentaNoExentaCorrienteTest().Balance + _appSettings.ValorSobregiro + 1;

            var transacciónTest = new TransactionBuilderTest()
                .WithId("1")
                .WithValor(valorRetiroIgualASaldoMasSobregiro)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaNoExentaCorrienteTest);

            _mockOptions
                .Setup(config => config.Value)
                .Returns(_appSettings);

            var exception =
                await Assert.ThrowsAsync<BusinessException>(async () =>
                    await _transactionUseCase.MakeWithdrawal(transacciónTest));

            Assert.Equal((int)TipoExcepcionNegocio.ValorRetiroNoPermitido, exception.code);

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
                .WithValor(100000)
                .Build();

            _mockCuentaRepository
                .Setup(repository => repository.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(ObtenerCuentaExentaAhorrosTest);

            _mockCuentaRepository
                .Setup(repository => repository.UpdateAsync(It.IsAny<string>(), It.IsAny<Account>()))
                .ReturnsAsync(ObtenerCuentaExentaAhorrosTest);

            _mockTransacciónRepository
                .Setup(repository => repository.CreateAsync(It.IsAny<Transaction>()))
                .ReturnsAsync(ObtenerUnaTransacciónTest);

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

        private Transaction ObtenerUnaTransacciónTest() =>
            new TransactionBuilderTest()
                .WithId("1")
                .WithIdCuenta("4300000000")
                .WithFechaMovimiento(DateTime.Now)
                .WithTipoTransacción(TransactionType.Deposit)
                .WithValor(100000)
                .WithSaldoInicial(1000000)
                .WithSaldoFinal(1100000)
                .Build();

        private List<Transaction> ObtenerListaTransacciónTest() => new()
        {
            new TransactionBuilderTest()
                .WithId("1")
                .WithIdCuenta("4600000000")
                .WithFechaMovimiento(DateTime.Now)
                .WithTipoTransacción(TransactionType.Deposit)
                .WithValor(100000)
                .WithSaldoInicial(1000000)
                .WithSaldoFinal(1100000)
                .Build(),
            new TransactionBuilderTest()
                .WithId("2")
                .WithIdCuenta("2300000000")
                .WithFechaMovimiento(DateTime.Now)
                .WithTipoTransacción(TransactionType.Withdrawal)
                .WithValor(100000)
                .WithSaldoInicial(1000000)
                .WithSaldoFinal(900000)
                .WithDescripción("Se Realizo Withdrawal por $100000 desde la cuenta con ID 2")
                .Build(),
        };

        private Account ObtenerCuentaNoExentaAhorrosTest() => new AccountBuilderTest()
            .WithId("1")
            .WithIdCliente("123456789")
            .WithNumeroDeCuenta("4600000000")
            .WithTipoCuenta(AccountType.Savings)
            .WithEstadoCuenta(AccountStatus.Active)
            .WithSaldo(1000000)
            .WithSaldoDisponible(996000)
            .WithExenta(false)
            .Build();

        private Account ObtenerCuentaCanceladaTest()
        {
            Account account = new AccountBuilderTest()
                .WithId("1")
                .WithIdCliente("123456789")
                .WithNumeroDeCuenta("4600000000")
                .WithTipoCuenta(AccountType.Savings)
                .WithSaldo(0)
                .WithSaldoDisponible(0)
                .WithExenta(false)
                .Build();

            account.CancelAccount();

            return account;
        }

        private Account ObtenerCuentaExentaAhorrosTest() => new AccountBuilderTest()
            .WithId("1")
            .WithIdCliente("123456789")
            .WithNumeroDeCuenta("4600000000")
            .WithTipoCuenta(AccountType.Savings)
            .WithEstadoCuenta(AccountStatus.Active)
            .WithSaldo(1000000)
            .WithSaldoDisponible(996000)
            .WithExenta(true)
            .Build();

        private Account ObtenerCuentaNoExentaCorrienteTest() => new AccountBuilderTest()
            .WithId("1")
            .WithIdCliente("123456789")
            .WithNumeroDeCuenta("4600000000")
            .WithTipoCuenta(AccountType.Regular)
            .WithEstadoCuenta(AccountStatus.Active)
            .WithSaldo(1000000)
            .WithSaldoDisponible(996000)
            .WithExenta(false)
            .Build();

        private Account ObtenerCuentaExentaCorrienteTest() => new AccountBuilderTest()
            .WithId("1")
            .WithIdCliente("123456789")
            .WithNumeroDeCuenta("4600000000")
            .WithTipoCuenta(AccountType.Regular)
            .WithEstadoCuenta(AccountStatus.Active)
            .WithSaldo(1000000)
            .WithSaldoDisponible(996000)
            .WithExenta(true)
            .Build();

        #endregion Private Methods
    }
}