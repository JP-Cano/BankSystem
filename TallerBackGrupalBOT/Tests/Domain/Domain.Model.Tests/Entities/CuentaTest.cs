using System.Diagnostics.CodeAnalysis;
using Domain.Model.Entities.Accounts;
using Domain.Model.Entities.Users;
using Xunit;

namespace Domain.Model.Tests.Entities
{
    [ExcludeFromCodeCoverage]
    public class CuentaTest
    {
        [Fact]
        public void CrearCuenta_Exitoso()
        {
            string id = "1";
            string idCliente = "001";
            string numeroCuenta = "23121";
            AccountType accountType = AccountType.Savings;
            decimal saldo = 2000;
            decimal saldoDisponible = 2000;
            bool exenta = true;

            var cuentaId = new Account("1");
            var cuentaCrear = new Account(idCliente, accountType, saldo, exenta);
            var cuentaActualizar = new Account(id, idCliente, numeroCuenta, accountType, saldo, saldoDisponible, exenta);

            Assert.NotNull(cuentaId);
            Assert.NotNull(cuentaCrear);
            Assert.NotNull(cuentaActualizar);
        }

        [Fact]
        public void AsignarSaldoInicial_Exitoso()
        {
            var valor = 100000;

            var cuentaTest = new AccountBuilderTest()
                .WithId("01")
                .WithClientId("001")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AssignInitialBalance(100000);

            Assert.Equal(valor, cuentaTest.Balance);
        }

        [Fact]
        public void AsignarSaldoInicial_Failure()
        {
            var valor = 100000;

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("2")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AssignInitialBalance(2000);

            Assert.NotEqual(valor, cuentaTest.Balance);
        }

        [Fact]
        public void CalcularSaldoDisponible_Exitoso()
        {
            decimal valorGMF = 0.004M;
            decimal saldoDisponible = 996000M;

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("3")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .WithBalance(1000000)
                .WithAvailableBalance(0)
                .Build();

            cuentaTest.CalculateAvailableBalance(valorGMF);

            Assert.Equal(saldoDisponible, cuentaTest.AvailableBalance);
        }

        [Fact]
        public void CalcularSaldoDisponible_Failure()
        {
            decimal valorGMF = 0.004M;
            decimal saldoDisponible = 990000M;

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("3")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .WithBalance(1000000)
                .Build();

            cuentaTest.CalculateAvailableBalance(valorGMF);

            Assert.NotEqual(saldoDisponible, cuentaTest.AvailableBalance);
        }

        [Fact]
        public void AsignarNumeroCuentaAhorros_Exitoso()
        {
            var numeroCuenta = "46-";

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("4")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AssignAccountNumber();

            Assert.StartsWith(numeroCuenta, cuentaTest.AccountNumber);
        }

        [Fact]
        public void AsignarNumeroCuentaAhorros_Failure()
        {
            var numeroCuenta = "23-";

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("4")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AssignAccountNumber();

            Assert.DoesNotContain(numeroCuenta, cuentaTest.AccountNumber);
        }

        [Fact]
        public void AsignarNumeroCuentaCorriente_Exitoso()
        {
            var numeroCuenta = "23-";

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("4")
                .WithAccountType(AccountType.Regular)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AssignAccountNumber();

            Assert.StartsWith(numeroCuenta, cuentaTest.AccountNumber);
        }

        [Fact]
        public void AsignarNumeroCuentaCorriente_Failure()
        {
            var numeroCuenta = "46-";

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("4")
                .WithAccountType(AccountType.Regular)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AssignAccountNumber();

            Assert.DoesNotContain(numeroCuenta, cuentaTest.AccountNumber);
        }

        [Fact]
        public void MarcarCuentaExenta_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("5")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .WithExempt(false)
                .Build();

            cuentaTest.SetAccountAsExempt();

            Assert.True(cuentaTest.Exempt);
        }

        [Fact]
        public void AgregarModificacion_Exitoso()
        {
            var usuarioTest = new UserBuilderTest()
                .WithId("1")
                .WithRol(Roles.Admin)
                .Build();

            var modificacionTest = new Modification(ModificationType.Enable, usuarioTest);

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AddModification(modificacionTest);

            Assert.Contains<Modification>(modificacionTest, cuentaTest.ModificationHistory);
        }

        [Fact]
        public void AgregarModificacion_Failure()
        {
            var usuarioTest = new UserBuilderTest()
                .WithId("1")
                .WithRol(Roles.Admin)
                .Build();

            var modificacionTest = new Modification(ModificationType.Enable, usuarioTest);
            var modificacionTestExpect = new Modification(ModificationType.Enable, usuarioTest);

            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.AddModification(modificacionTest);

            Assert.DoesNotContain<Modification>(modificacionTestExpect, cuentaTest.ModificationHistory);
        }

        [Fact]
        public void ActualizarSaldo_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .WithBalance(2000)
                .Build();

            var nuevoSaldo = cuentaTest.Balance + 3000;

            cuentaTest.UpdateBalance(nuevoSaldo);

            Assert.Equal(cuentaTest.Balance, nuevoSaldo);
        }

        [Fact]
        public void ActualizarSaldo_Failure()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountType(AccountType.Savings)
                .WithAccountStatus(AccountStatus.Active)
                .WithBalance(2000)
                .Build();

            var nuevoSaldo = cuentaTest.Balance + 3000;

            cuentaTest.UpdateBalance(nuevoSaldo);

            Assert.NotEqual(3000, nuevoSaldo);
        }

        [Fact]
        public void EstaActiva_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            var result = cuentaTest.IsActive();

            Assert.True(result);
        }

        [Fact]
        public void EstaInactiva_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            var result = cuentaTest.IsInactive();

            Assert.False(result);
        }

        [Fact]
        public void EstaCancelada_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            var result = cuentaTest.IsCancelled();

            Assert.False(result);
        }

        [Fact]
        public void HabilitarCuenta_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountStatus(AccountStatus.Inactive)
                .Build();

            cuentaTest.EnableAccount();

            Assert.Equal(AccountStatus.Active, cuentaTest.AccountStatus);
        }

        [Fact]
        public void DehabilitarCuenta_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.DisableAccount();

            Assert.Equal(AccountStatus.Inactive, cuentaTest.AccountStatus);
        }

        [Fact]
        public void CancelarCuenta_Exitoso()
        {
            var cuentaTest = new AccountBuilderTest()
                .WithClientId("6")
                .WithAccountNumber("1")
                .WithAccountStatus(AccountStatus.Active)
                .Build();

            cuentaTest.CancelAccount();

            Assert.Equal(AccountStatus.Cancelled, cuentaTest.AccountStatus);
        }
    }
}