namespace BankingSystem.Test
{
    [TestFixture]
    public class BankAccountTests
    {
        private BankAccount _account;

        [SetUp]
        public void Setup()
        {
            _account = new BankAccount("12345", "Rozer", 1000);
        }

        [Test]
        public void Deposit_PositiveAmount_IncreasesBalance()
        {
            _account.Deposit(500);
            Assert.AreEqual(1500, _account.Balance);
        }

        [Test]
        public void Deposit_NegativeAmount_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => _account.Deposit(-100));
        }

        [Test]
        public void Withdraw_ValidAmount_DecreasesBalance()
        {
            _account.Withdraw(200);
            Assert.AreEqual(800, _account.Balance);
        }

        [Test]
        public void Withdraw_InsufficientFunds_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() => _account.Withdraw(2000));
        }

        [Test]
        public void TransferTo_ValidAmount_UpdatesBothAccounts()
        {
            var target = new BankAccount("67890", "Alex", 500);
            _account.TransferTo(target, 300);

            Assert.AreEqual(700, _account.Balance);
            Assert.AreEqual(800, target.Balance);
        }

        [Test]
        public void TransferTo_NullTarget_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _account.TransferTo(null, 100));
        }
    }

}