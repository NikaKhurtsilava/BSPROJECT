using FirstProjectTest.Models;
using FirstProjectTest.Repo.IServices;
using FirstProjectTest.Repo.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProjectTest.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public IActionResult Index(DateTime? startDate, DateTime? endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var transactions = _transactionService.GetTransactionsForUser(userId, startDate, endDate);
            return View(transactions);
        }

    }
}
