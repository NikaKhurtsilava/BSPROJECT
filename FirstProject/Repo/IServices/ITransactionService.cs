using FirstProjectTest.Enums;
using FirstProjectTest.Models;

namespace FirstProjectTest.Repo.IServices
{
    public interface ITransactionService
    {
        List<Transaction> GetTransactionsForUser(string userId, DateTime? startDate, DateTime? endDate);
    }
}
