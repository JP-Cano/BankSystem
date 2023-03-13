using Domain.Model.Entities.Transactions;

namespace Domain.Model.Tests
{
    public class TransactionBuilderTest
    {
        private string _id = string.Empty;
        private string _accountId = string.Empty;
        private DateTime _movementDate;
        private TransactionType _transactionType;
        private decimal _value = 0M;
        private decimal _initialBalance = 0M;
        private decimal _finalBalance = 0M;
        private string _description = string.Empty;

        public TransactionBuilderTest()
        {
        }

        public Transaction Build() =>
            new(_id, _accountId, _movementDate,
                _transactionType, _value,
                _initialBalance, _finalBalance, _description);

        public TransactionBuilderTest WithId(string id)
        {
            _id = id;
            return this;
        }

        public TransactionBuilderTest WithAccountId(string accountId)
        {
            _accountId = accountId;
            return this;
        }

        public TransactionBuilderTest WithMovementDate(DateTime date)
        {
            _movementDate = date;
            return this;
        }

        public TransactionBuilderTest WithTransactionType(TransactionType transactionType)
        {
            _transactionType = transactionType;
            return this;
        }

        public TransactionBuilderTest WithValue(decimal value)
        {
            _value = value;
            return this;
        }

        public TransactionBuilderTest WithInitialBalance(decimal initialBalance)
        {
            _initialBalance = initialBalance;
            return this;
        }

        public TransactionBuilderTest WithFinalBalance(decimal finalBalance)
        {
            _finalBalance = finalBalance;
            return this;
        }

        public TransactionBuilderTest WithDescription(string description)
        {
            _description = description;
            return this;
        }
    }
}