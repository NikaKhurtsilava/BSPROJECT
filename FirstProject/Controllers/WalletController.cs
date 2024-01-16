using Microsoft.AspNetCore.Mvc;
using FirstProjectTest.Repo.IServices;
using System.Security.Claims;

[Controller]
public class WalletController : Controller
{
    private readonly IWalletService _walletService;

    public WalletController(IWalletService walletService)
    {
        _walletService = walletService;
    }

    [HttpGet]
    public IActionResult GetCurrentBalance()
    {
        Console.WriteLine("GetCurrentBalance action method called.");
        
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Json(new { success = false, message = "User ID not available." });
        }

        decimal currentBalance = _walletService.GetCurrentBalanceByUserId(userId);

        return Json(new { currentBalance });
    }

    
}
