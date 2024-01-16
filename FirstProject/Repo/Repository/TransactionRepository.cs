using Dapper;
using FirstProjectTest.Repo.IRepository;
using System.Data;
using FirstProjectTest.Models;

namespace FirstProjectTest.Repo.Repository
{
    
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDbConnection _dbConnection;

        public TransactionRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
        }

        public int Create(Transaction transaction)
        {
            const string sql = @"INSERT INTO Transactions (UserId, Amount, TransactionType, TransactionStatus, CurrentBalance, TransactionDate)
                                 VALUES (@UserId, @Amount, @TransactionType, @TransactionStatus, @CurrentBalance, @TransactionDate);
                                 SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return _dbConnection.ExecuteScalar<int>(sql, transaction);
        }

        public bool Delete(int transactionId)
        {
            const string sql = "DELETE FROM Transactions WHERE TransactionId = @TransactionId";

            var affectedRows = _dbConnection.Execute(sql, new { TransactionId = transactionId });

            return affectedRows > 0;
        }

        public Transaction GetById(int transactionId)
        {
            const string sql = "SELECT * FROM Transactions WHERE TransactionId = @TransactionId";

            return _dbConnection.QueryFirstOrDefault<Transaction>(sql, new { TransactionId = transactionId });
        }

        public IEnumerable<Transaction> GetTransactionsByUserId(string userId)
        {
            const string sql = "SELECT * FROM Transactions WHERE UserId = @UserId";

            return _dbConnection.Query<Transaction>(sql, new { UserId = userId }).ToList();
        }

        public bool Update(Transaction transaction)
        {
            const string sql = @"UPDATE Transactions 
                                 SET Amount = @Amount, 
                                     TransactionType = @TransactionType, 
                                     TransactionStatus = @TransactionStatus, 
                                     CurrentBalance = @CurrentBalance, 
                                     TransactionDate = @TransactionDate 
                                 WHERE TransactionId = @TransactionId";

            var affectedRows = _dbConnection.Execute(sql, transaction);

            return affectedRows > 0;
        }



    }
}
