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

        public TransactionBuilderTest WithIdCuenta(string idCuenta)
        {
            _accountId = idCuenta;
            return this;
        }

        public TransactionBuilderTest WithFechaMovimiento(DateTime fecha)
        {
            _movementDate = fecha;
            return this;
        }

        public TransactionBuilderTest WithTipoTransacción(TransactionType transactionType)
        {
            _transactionType = transactionType;
            return this;
        }

        public TransactionBuilderTest WithValor(decimal valor)
        {
            _value = valor;
            return this;
        }

        public TransactionBuilderTest WithSaldoInicial(decimal saldoInicial)
        {
            _initialBalance = saldoInicial;
            return this;
        }

        public TransactionBuilderTest WithSaldoFinal(decimal saldoFinal)
        {
            _finalBalance = saldoFinal;
            return this;
        }

        public TransactionBuilderTest WithDescripción(string descripción)
        {
            _description = descripción;
            return this;
        }
    }
}