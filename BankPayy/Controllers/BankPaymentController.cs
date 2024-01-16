using BankPayy.Enums;
using BankPayy.Models;
using BankPayy.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankPayy.Controllers
{
    [Route("[controller]/[action]")]
    public class BankPaymentController : Controller
    {
        PaymentService paymentService = new PaymentService();
        private readonly IHttpClientFactory _httpClientFactory;

        public BankPaymentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult BankPayment(int amount, string userId)
        {
            if (Request.Query.TryGetValue("transactionId", out var transactionId))
            {
                ViewData["TransactionId"] = transactionId;
            }

            ViewData["amount"] = amount;
            ViewData["userId"] = userId;

            return View();
        }
        
        public IActionResult ProcessPayment(decimal amount, int transactionId, string userId)
        {
            Console.WriteLine("Processing payment with amount: " + amount + " and trId: " + transactionId);

            var isSuccess = paymentService.ProcessPayment(amount);
            Console.WriteLine("For that amount, the service returned: " + isSuccess);

            SendApiRequest(transactionId, amount, isSuccess, userId);

            return Ok(isSuccess
                ? new { success = true, message = "Payment successful.", transactionId }
                : new { success = false, message = "Payment failed.", transactionId });
        }

        private void SendApiRequest(int transactionId, decimal amount, bool isSuccess, string userId)
        {
            

            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = "https://localhost:44398/api/firstprojectapi/processpayment";

                var requestData = new DepositRequest
                {
                    TransactionId = transactionId,
                    UserId = userId,
                    Amount = amount,
                    IsSuccess = isSuccess
                };

                var response = httpClient.PostAsJsonAsync(apiUrl, requestData).Result;

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("API request successful.");
                }
                else
                {
                    Console.WriteLine("API request failed. Status code: " + response.StatusCode);
                }
            }
        }

    }
}


    
