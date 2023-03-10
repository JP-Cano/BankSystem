using System.Diagnostics.CodeAnalysis;
using Domain.Model.Entities.Transactions;
using Xunit;

namespace Domain.Model.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class TransacciónTest
    {
        [Fact]
        public void GenerarDescripción_Exitosa_TipoTransacciónConsignación()
        {
            var descripciónEsperada = "Se Realizo Deposit por $100000 a la cuenta con ID 1";
            var transacciónTest = new TransactionBuilderTest()
                .WithValor(100000)
                .WithTipoTransacción(TransactionType.Deposit)
                .WithIdCuenta("1")
                .Build();

            transacciónTest.GenerateDescription();

            Assert.Equal(descripciónEsperada, transacciónTest.Description);
        }

        [Fact]
        public void GenerarDescripción_Exitosa_TipoTransacciónRetiro()
        {
            var descripciónEsperada = "Se Realizo Withdrawal por $200000 desde la cuenta con ID 1";
            var transacciónTest = new TransactionBuilderTest()
                .WithValor(200000)
                .WithTipoTransacción(TransactionType.Withdrawal)
                .WithIdCuenta("1")
                .Build();

            transacciónTest.GenerateDescription();

            Assert.Equal(descripciónEsperada, transacciónTest.Description);
        }

        [Fact]
        public void GenerarDescripción_Exitosa_TipoTransacciónTransferencia()
        {
            var idCuentaDestino = "2";
            var descripciónEsperada = "Se Realizo Transfer por $100000 desde la cuenta con ID 1 a la cuenta con ID 2";
            var transacciónTest = new TransactionBuilderTest()
                .WithValor(100000)
                .WithTipoTransacción(TransactionType.Transfer)
                .WithIdCuenta("1")
                .Build();

            transacciónTest.GenerateDescription(idCuentaDestino);

            Assert.Equal(descripciónEsperada, transacciónTest.Description);
        }

        [Fact]
        public void AsignarTipoTransacción_Consignación()
        {
            var tipoAsignar = TransactionType.Deposit;

            var transacciónTest = new TransactionBuilderTest()
                .WithValor(100000)
                .WithIdCuenta("1")
                .Build();

            transacciónTest.AssignTransactionType(tipoAsignar);

            Assert.Equal(tipoAsignar, transacciónTest.TransactionType);
        }

        [Fact]
        public void AsignarAsignarFechaMovimiento_Exitoso()
        {
            var transacciónTest = new TransactionBuilderTest()
                .WithValor(100000)
                .WithIdCuenta("1")
                .Build();

            transacciónTest.AddMovementDate();

            Assert.IsType<DateTime>(transacciónTest.MovementDate);
        }

        [Fact]
        public void AsignarSaldoInicial_Exitoso()
        {
            var valor = 100000;

            var transacciónTest = new TransactionBuilderTest()
                .WithValor(100000)
                .WithIdCuenta("1")
                .Build();

            transacciónTest.AssignInitialBalance(valor);

            Assert.Equal(valor, transacciónTest.InitialBalance);
        }

        [Fact]
        public void AsignarAsignarSaldoFinalDebito_Exitoso_RestaElValor()
        {
            var valor = 100000;

            var transacciónTest = new TransactionBuilderTest()
                .WithValor(100000)
                .WithSaldoInicial(500000)
                .WithIdCuenta("1")
                .Build();

            var saldoFinalEsperado = transacciónTest.InitialBalance - valor;

            transacciónTest.AssignDebitFinalBalance(valor);

            Assert.Equal(saldoFinalEsperado, transacciónTest.FinalBalance);
        }

        [Fact]
        public void AsignarAsignarSaldoFinalCredito_Exitoso_SumaElValor()
        {
            var valor = 100000;

            var transacciónTest = new TransactionBuilderTest()
                .WithValor(100000)
                .WithSaldoInicial(500000)
                .WithIdCuenta("1")
                .Build();

            var saldoFinalEsperado = transacciónTest.InitialBalance + valor;

            transacciónTest.AssignFinalCreditBalance(valor);

            Assert.Equal(saldoFinalEsperado, transacciónTest.FinalBalance);
        }
    }
}