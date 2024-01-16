using FirstProjectTest.IRepository;
using FirstProjectTest.Models;



namespace FirstProjectTest.Repo.IRepository
{
    public interface ITransactionRepository : ICrudRepository<Transaction>
    {
        IEnumerable<Transaction> GetTransactionsByUserId(string userId);
    }
}
