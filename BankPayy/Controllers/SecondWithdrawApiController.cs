using BankPayy.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankPayy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecondWithdrawApiController : ControllerBase
    {
        [HttpPost("ProcessWithdrawal")]
        public IActionResult ProcessWithdrawal([FromBody] WithdrawRequest withdrawRequest)
        {
            var isEven = withdrawRequest.Amount % 2 == 0;

            if (isEven)
            {
                var response = new WithdrawResponse { IsSuccess = true, TransactionId = withdrawRequest.TransactionId, Amount = withdrawRequest.Amount, UserId = withdrawRequest.UserId};
                return Ok(response);
            }
            else
            {
                var response = new WithdrawResponse { IsSuccess = false, TransactionId = withdrawRequest.TransactionId, Amount = withdrawRequest.Amount };
                return Ok(response);
            }
        }
    }
}
