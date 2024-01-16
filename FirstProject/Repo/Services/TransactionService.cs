using FirstProjectTest.Models;
using FirstProjectTest.Repo.IRepository;
using FirstProjectTest.Repo.IServices;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public List<Transaction> GetTransactionsForUser(string userId, DateTime? startDate, DateTime? endDate)
    {
        var transactions = _transactionRepository.GetTransactionsByUserId(userId);

        if (startDate.HasValue)
        {
            transactions = transactions.Where(t => t.TransactionDate >= startDate.Value).ToList();
        }

        if (endDate.HasValue)
        {
            transactions = transactions.Where(t => t.TransactionDate <= endDate.Value.AddDays(1).AddTicks(-1)).ToList();
        }

        return (List<Transaction>)transactions;
    }

  
}
