using Domain.Model.Entities.Accounts;

namespace Domain.Model.Tests;

public class AccountBuilderTest
{
    private string _id = string.Empty;
    private string _idCliente = string.Empty;
    private string _accountNumber = string.Empty;
    private AccountType _accountType;
    private AccountStatus _accountStatus;
    private decimal _balance = 0;
    private decimal _availableBalance = 0;
    private bool _exempt;

    public AccountBuilderTest()
    {
    }

    public AccountBuilderTest WithId(string id)
    {
        _id = id;
        return this;
    }

    public AccountBuilderTest WithIdCliente(string id)
    {
        _idCliente = id;
        return this;
    }

    public AccountBuilderTest WithNumeroDeCuenta(string numeroDeCuenta)
    {
        _accountNumber = numeroDeCuenta;
        return this;
    }

    public AccountBuilderTest WithTipoCuenta(AccountType accountType)
    {
        _accountType = accountType;
        return this;
    }

    public AccountBuilderTest WithEstadoCuenta(AccountStatus accountStatus)
    {
        _accountStatus = accountStatus;
        return this;
    }

    public AccountBuilderTest WithSaldo(decimal saldo)
    {
        _balance = saldo;
        return this;
    }

    public AccountBuilderTest WithSaldoDisponible(decimal saldoDisponible)
    {
        _availableBalance = saldoDisponible;
        return this;
    }

    public AccountBuilderTest WithExenta(bool exenta)
    {
        _exempt = exenta;
        return this;
    }

    public Account Build() => new(_id, _idCliente, _accountNumber, _accountType, _balance, _availableBalance, _exempt);
}