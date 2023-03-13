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

    public AccountBuilderTest WithClientId(string id)
    {
        _idCliente = id;
        return this;
    }

    public AccountBuilderTest WithAccountNumber(string accountNumber)
    {
        _accountNumber = accountNumber;
        return this;
    }

    public AccountBuilderTest WithAccountType(AccountType accountType)
    {
        _accountType = accountType;
        return this;
    }

    public AccountBuilderTest WithAccountStatus(AccountStatus accountStatus)
    {
        _accountStatus = accountStatus;
        return this;
    }

    public AccountBuilderTest WithBalance(decimal balance)
    {
        _balance = balance;
        return this;
    }

    public AccountBuilderTest WithAvailableBalance(decimal availableBalance)
    {
        _availableBalance = availableBalance;
        return this;
    }

    public AccountBuilderTest WithExempt(bool exempt)
    {
        _exempt = exempt;
        return this;
    }

    public Account Build() => new(_id, _idCliente, _accountNumber, _accountType, _balance, _availableBalance, _exempt);
}