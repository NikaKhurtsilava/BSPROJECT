using Microsoft.AspNetCore.Mvc;
using FirstProjectTest.Enums;
using FirstProjectTest.Repo.IServices;
using FirstProjectTest.Models;
using FirstProjectTest.IRepository;
using System.Net.Http;
using FirstProjectTest.Repo.Services;


namespace FirstProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FirstProjectApiController : ControllerBase
    {
        
        private readonly IWalletRepository _walletRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public FirstProjectApiController(IWalletRepository walletRepository, IHttpClientFactory httpClientFactory)
        {
            _walletRepository = walletRepository;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("ProcessPayment")]
        public IActionResult ProcessPayment([FromBody] DepositRequest paymentRequest)
        {
            var transactionId = paymentRequest.TransactionId;
            var userId = paymentRequest.UserId;
            var amount = paymentRequest.Amount;
            var isSuccess = paymentRequest.IsSuccess;

            if (isSuccess)
            {
                _walletRepository.UpdateTransactionStatusAndWalletCurrentBalance(userId, transactionId, (int)TransactionStatus.Success, amount);
            }
            else
            {
                _walletRepository.UpdateTransactionStatus(userId, transactionId, (int)TransactionStatus.Rejected);
            }

            return Ok();
        }


    }
}
