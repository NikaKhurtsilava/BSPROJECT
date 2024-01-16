using Dapper;
using FirstProjectTest.IRepository;
using FirstProjectTest.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using static Dapper.SqlMapper;

namespace FirstProjectTest.Repository
{
    public class WalletRepository : IWalletRepository
    {

        private readonly IDbConnection _dbConnection;

        public WalletRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public int GetLatestTransactionId(string userId)
        {
            const string sql = "SELECT TOP 1 TransactionId FROM Transactions WHERE UserId = @UserId ORDER BY TransactionDate DESC";

            return _dbConnection.QueryFirstOrDefault<int>(sql, new { UserId = userId });
        }
        public int Create(Wallet wallet)
        {
            const string sql = "INSERT INTO Wallet (UserId, CurrentBalance) VALUES (@UserId, @CurrentBalance); SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return _dbConnection.ExecuteScalar<int>(sql, wallet);
        }

        public bool Delete(int walletId)
        {
            const string sql = "DELETE FROM Wallet WHERE WalletId = @WalletId";

            var affectedRows = _dbConnection.Execute(sql, new { WalletId = walletId });

            return affectedRows > 0;
        }

        public Wallet GetById(int walletId)
        {
            const string sql = "SELECT * FROM Wallet WHERE WalletId = @WalletId";

            return _dbConnection.QueryFirstOrDefault<Wallet>(sql, new { WalletId = walletId });

        }

        public Wallet GetWalletByUserId(string userId)
        {
            const string sql = "SELECT * FROM Wallet WHERE UserId = @UserId";

            return _dbConnection.QueryFirstOrDefault<Wallet>(sql, new { UserId = userId });

        }

        public void MakeDeposit(string userId, decimal amount, out int transactionId)
        {
            const string depositProcedure = "MakeDeposit";
            var parameters = new { UserId = userId, Amount = amount };
            _dbConnection.Execute(depositProcedure, parameters, commandType: CommandType.StoredProcedure);

            transactionId = GetLatestTransactionId(userId);
        }

        public bool Update(Wallet wallet)
        {
            const string sql = "UPDATE Wallet SET CurrentBalance = @CurrentBalance WHERE WalletId = @WalletId";

            var affectedRows = _dbConnection.Execute(sql, wallet);

            return affectedRows > 0;
        }
        public void UpdateWalletBalance(string userId, decimal amount)
        {
            _dbConnection.Execute("UpdateWalletBalance", new { UserId = userId, Amount = amount }, commandType: CommandType.StoredProcedure);
        }

        public void Withdraw(string userId, decimal amount, out int transactionId)
        {
            var parameters = new { UserId = userId, Amount = amount };
            _dbConnection.Execute("WithdrawProcedure", parameters, commandType: CommandType.StoredProcedure);
            transactionId = GetLatestTransactionId(userId);
        }

        public void UpdateTransactionStatusAndWalletCurrentBalance(string userId, int transactionId, int transactionStatus, decimal amount)
        {
            const string updateProcedure = "UpdateTransactionStatusAndWalletCurrentBalance";

            var parameters = new
            {
                UserId = userId,
                TransactionId = transactionId,
                TransactionStatus = transactionStatus,
                Amount = amount
            };

            _dbConnection.Execute(updateProcedure, parameters, commandType: CommandType.StoredProcedure);
        }

        public void UpdateTransactionStatus(string userId, int transactionId, int transactionStatus)
        {
            const string updateProcedure = "UpdateTransactionStatus";

            var parameters = new
            {
                UserId = userId,
                TransactionId = transactionId,
                TransactionStatus = transactionStatus
            };

            _dbConnection.Execute(updateProcedure, parameters, commandType: CommandType.StoredProcedure);
        }
        public void RefundAndUpdateTransactionStatus(string userId, int transactionId, decimal amount)
        {
            var parameters = new { UserId = userId, TransactionId = transactionId, Amount = amount };

            _dbConnection.Execute("RefundAndUpdateTransactionStatusProcedure", parameters, commandType: CommandType.StoredProcedure);
        }
    }


}
