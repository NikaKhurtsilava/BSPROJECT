using BankPayy.Models;
using BankPayy.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPayy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepositController : ControllerBase
    {
        private readonly IOptions<BankSettings> _bankSettings;

        public DepositController(IOptions<BankSettings> bankSettings)
        {
            _bankSettings = bankSettings;
        }

        [HttpPost]
        public IActionResult ProcessDeposit([FromBody] DepositRequest request)
        {
            var paymentUrl = $"{_bankSettings.Value.PaymentUrl}?transactionId={request.TransactionId}&userId={request.UserId}&amount={request.Amount}";

            return Ok(paymentUrl);  
        }
    }
}
