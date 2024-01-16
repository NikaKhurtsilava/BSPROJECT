using FirstProjectTest.Models;

namespace FirstProjectTest.Repo.IServices
{
    public interface IWalletService
    {
        void CreateWalletByUserId(string userId);
        TransactionResult MakeDeposit(string userId, decimal amount);
        TransactionResult MakeWithdrawal(string userId, decimal amount);
        decimal GetCurrentBalanceByUserId(string userId);
    }
}
