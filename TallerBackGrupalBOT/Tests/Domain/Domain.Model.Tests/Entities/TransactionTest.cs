using System.Diagnostics.CodeAnalysis;
using Domain.Model.Entities.Transactions;
using Xunit;

namespace Domain.Model.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class TransactionTest
    {
        [Fact]
        public void GenerarDescripción_Exitosa_TipoTransacciónConsignación()
        {
            var descripciónEsperada = "Deposit made of $100000 to account with ID 1";
            var transacciónTest = new TransactionBuilderTest()
                .WithValue(100000)
                .WithTransactionType(TransactionType.Deposit)
                .WithAccountId("1")
                .Build();

            transacciónTest.GenerateDescription();

            Assert.Equal(descripciónEsperada, transacciónTest.Description);
        }

        [Fact]
        public void GenerarDescripción_Exitosa_TipoTransacciónRetiro()
        {
            var descripciónEsperada = "Withdrawal made of $200000 from account with ID 1";
            var transacciónTest = new TransactionBuilderTest()
                .WithValue(200000)
                .WithTransactionType(TransactionType.Withdrawal)
                .WithAccountId("1")
                .Build();

            transacciónTest.GenerateDescription();

            Assert.Equal(descripciónEsperada, transacciónTest.Description);
        }

        [Fact]
        public void GenerarDescripción_Exitosa_TipoTransacciónTransferencia()
        {
            var idCuentaDestino = "2";
            var descripciónEsperada = "Transfer made of $100000 from account with ID 1 to account with ID 2";
            var transacciónTest = new TransactionBuilderTest()
                .WithValue(100000)
                .WithTransactionType(TransactionType.Transfer)
                .WithAccountId("1")
                .Build();

            transacciónTest.GenerateDescription(idCuentaDestino);

            Assert.Equal(descripciónEsperada, transacciónTest.Description);
        }

        [Fact]
        public void AsignarTipoTransacción_Consignación()
        {
            var tipoAsignar = TransactionType.Deposit;

            var transacciónTest = new TransactionBuilderTest()
                .WithValue(100000)
                .WithAccountId("1")
                .Build();

            transacciónTest.AssignTransactionType(tipoAsignar);

            Assert.Equal(tipoAsignar, transacciónTest.TransactionType);
        }

        [Fact]
        public void AsignarAsignarFechaMovimiento_Exitoso()
        {
            var transacciónTest = new TransactionBuilderTest()
                .WithValue(100000)
                .WithAccountId("1")
                .Build();

            transacciónTest.AddMovementDate();

            Assert.IsType<DateTime>(transacciónTest.MovementDate);
        }

        [Fact]
        public void AsignarSaldoInicial_Exitoso()
        {
            var valor = 100000;

            var transacciónTest = new TransactionBuilderTest()
                .WithValue(100000)
                .WithAccountId("1")
                .Build();

            transacciónTest.AssignInitialBalance(valor);

            Assert.Equal(valor, transacciónTest.InitialBalance);
        }

        [Fact]
        public void AsignarAsignarSaldoFinalDebito_Exitoso_RestaElValor()
        {
            var valor = 100000;

            var transacciónTest = new TransactionBuilderTest()
                .WithValue(100000)
                .WithInitialBalance(500000)
                .WithAccountId("1")
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
                .WithValue(100000)
                .WithInitialBalance(500000)
                .WithAccountId("1")
                .Build();

            var saldoFinalEsperado = transacciónTest.InitialBalance + valor;

            transacciónTest.AssignFinalCreditBalance(valor);

            Assert.Equal(saldoFinalEsperado, transacciónTest.FinalBalance);
        }
    }
}