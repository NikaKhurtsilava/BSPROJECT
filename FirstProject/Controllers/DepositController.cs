using FirstProjectTest.Models;
using FirstProjectTest.Repo.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProjectTest.Controllers
{
    public class DepositController : Controller
    {
        private readonly IWalletService _walletService;

        public DepositController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public IActionResult Deposit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeDeposit(decimal amount)
        {
            Console.WriteLine("START CONTROLLER DEPOSIT METHOD");
            
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
               
                return Json(new { success = false, message = "User ID not available." });
            }

            var result = _walletService.MakeDeposit(userId, amount);

            var depositRequest = new DepositRequest
            {
                TransactionId = result.TransactionId, 
                UserId = userId,
                Amount = amount
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7292"); 
                var response = client.PostAsJsonAsync("/api/deposit", depositRequest).Result;

                if (response.IsSuccessStatusCode)
                {
                    var paymentUrl = response.Content.ReadAsStringAsync().Result;

                    return Json(new { success = true, message = "Deposit successful.", transactionId = result.TransactionId, paymentUrl });
                }
                else
                {
                    return Json(new { success = false, message = "Error processing deposit." });
                }
            }
        }

        
    }
}
