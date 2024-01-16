using FirstProjectTest.Enums;
using FirstProjectTest.Models;

namespace FirstProjectTest.IRepository
{
    public interface IWalletRepository: ICrudRepository<Wallet>
    {
        Wallet GetWalletByUserId(string userId);

        void MakeDeposit(string userId, decimal amount, out int transactionId);
        void Withdraw(string userId, decimal amount, out int transactionId);
        int GetLatestTransactionId(string userId);
        void UpdateTransactionStatusAndWalletCurrentBalance(string userId, int transactionId, int transactionStatus, decimal amount);
        void UpdateTransactionStatus(string userId, int transactionId, int transactionStatus);
        void RefundAndUpdateTransactionStatus(string userId, int transactionId, decimal amount);

    }
}
