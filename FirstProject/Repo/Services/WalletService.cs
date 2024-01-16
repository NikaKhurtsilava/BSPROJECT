using Dapper;
using FirstProjectTest.Enums;
using FirstProjectTest.IRepository;
using FirstProjectTest.Models;
using FirstProjectTest.Repo.IRepository;
using FirstProjectTest.Repo.IServices;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FirstProjectTest.Repo.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly ITransactionRepository _transactionRepository;
        public WalletService(IWalletRepository walletRepository, ITransactionRepository transactionRepository)
        {
            _walletRepository = walletRepository;
            _transactionRepository = transactionRepository;
        }

        public decimal GetCurrentBalanceByUserId(string userId)
        {
            var wallet = _walletRepository.GetWalletByUserId(userId);

            if (wallet != null)
            {
                return wallet.CurrentBalance;
            }

            return 0m;
        }


        public void CreateWalletByUserId(string userId)
        {
            var wallet = new Wallet { UserId = userId, CurrentBalance = 0m };
            _walletRepository.Create(wallet); 
        }

        public TransactionResult MakeDeposit(string userId, decimal amount)
        {
            var wallet = _walletRepository.GetWalletByUserId(userId);

            int transactionId;
            _walletRepository.MakeDeposit(userId, amount, out transactionId);

            return new TransactionResult { Success = true, TransactionId = transactionId };
        }


        public TransactionResult MakeWithdrawal(string userId, decimal amount)
        {
            var wallet = _walletRepository.GetWalletByUserId(userId);

            if (wallet == null)
            {
                CreateWalletByUserId(userId);
                wallet = _walletRepository.GetWalletByUserId(userId);
            }

            if (wallet.CurrentBalance < amount)
            {
                return new TransactionResult { Success = false, Message = "Insufficient balance." };
            }
            int transactionId;
            _walletRepository.Withdraw(userId, amount, out transactionId);
            
            return new TransactionResult { Success = true, TransactionId = transactionId, Message = "Withdrawal successful." };

        }

    }
}
