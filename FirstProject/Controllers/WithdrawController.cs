using FirstProjectTest.Enums;
using FirstProjectTest.IRepository;
using FirstProjectTest.Models;
using FirstProjectTest.Repo.IServices;
using FirstProjectTest.Repo.Services;
using FirstProjectTest.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FirstProjectTest.Controllers
{
    public class WithdrawController : Controller
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletService _walletservice;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IOptions<FirstProjectSettings> _firstProjectSettings;

        public WithdrawController(
            IWalletRepository walletRepository,
            IHttpClientFactory httpClientFactory,
            IWalletService walletservice,
            IOptions<FirstProjectSettings> firstProjectSettings)
        {
            _walletRepository = walletRepository;
            _httpClientFactory = httpClientFactory;
            _walletservice = walletservice;
            _firstProjectSettings = firstProjectSettings;
        }

        public IActionResult Withdraw()
        {
            return View();
        }

        [HttpPost]
        public IActionResult MakeWithdrawal(decimal amount)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Json(new { success = false, message = "User ID not available." });
            }
            var result = _walletservice.MakeWithdrawal(userId, amount);
            
            var withdrawRequest = new WithdrawRequest {
                TransactionId = result.TransactionId,
                UserId = userId,
                Amount = amount
            };

            
            var withdrawalApiResponse = SendWithdrawalApiRequest(withdrawRequest);

            if (withdrawalApiResponse != null)
            {
                _walletRepository.UpdateTransactionStatus(
                    userId,
                    withdrawalApiResponse.TransactionId,
                    (int)TransactionStatus.Success
                    );
                return Ok(new { success = withdrawalApiResponse.IsSuccess, message = "withdrawal completed" });
            }
            else
            {
                _walletRepository.RefundAndUpdateTransactionStatus(
                    userId,
                    withdrawalApiResponse.TransactionId,
                    (int)TransactionStatus.Rejected
                );
                return Json(new { success = false, message = "Withdrawal failed. Amount refunded." });
            }
            
        }

        private WithdrawResponse SendWithdrawalApiRequest(WithdrawRequest withdrawRequest)
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = _firstProjectSettings.Value.WithdrawUrl;

                var response = httpClient.PostAsJsonAsync(apiUrl, withdrawRequest).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response.Content.ReadFromJsonAsync<WithdrawResponse>().Result;
                }
                else
                {
                    Console.WriteLine("API request failed. Status code: " + response.StatusCode);
                    return null;
                }
            }
        }
    }

}
