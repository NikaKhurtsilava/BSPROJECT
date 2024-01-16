using FirstProjectTest.Enums;
using FirstProjectTest.IRepository;
using FirstProjectTest.Models;
using FirstProjectTest.Repo.IServices;
using FirstProjectTest.Repo.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace FirstProjectTest.Controllers
{
    public class WithdrawController : Controller
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletService _walletservice;
        private readonly IHttpClientFactory _httpClientFactory;

        public WithdrawController(IWalletRepository walletRepository, IHttpClientFactory httpClientFactory, IWalletService walletservice)
        {
            _walletRepository = walletRepository;
            _httpClientFactory = httpClientFactory;
            _walletservice = walletservice;
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
                var apiUrl = "https://localhost:7292/api/secondwithdrawapi/processwithdrawal";

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
