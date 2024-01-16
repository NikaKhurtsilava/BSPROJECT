using FirstProjectTest.Models;
using FirstProjectTest.Repo.IServices;
using FirstProjectTest.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace FirstProjectTest.Controllers
{
    public class DepositController : Controller
    {
        private readonly IOptions<FirstProjectSettings> _firstProjectSettings;
        private readonly IWalletService _walletService;

        public DepositController(IWalletService walletService, IOptions<FirstProjectSettings> firstProjectSettings)
        {
            _walletService = walletService;
            _firstProjectSettings = firstProjectSettings;
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
                client.BaseAddress = new Uri(_firstProjectSettings.Value.ApiUrl); 
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
