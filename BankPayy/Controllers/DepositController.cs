using BankPayy.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankPayy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepositController : ControllerBase
    {
        [HttpPost]
        public IActionResult ProcessDeposit([FromBody] DepositRequest request)
        {
            var paymentUrl = $"{Request.Scheme}://{Request.Host}/BankPayment/BankPayment?transactionId={request.TransactionId}&userId={request.UserId}&amount={request.Amount}";

            return Ok(paymentUrl);
        }
    }
}
