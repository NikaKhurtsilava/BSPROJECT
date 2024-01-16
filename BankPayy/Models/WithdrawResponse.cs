namespace BankPayy.Models
{
    public class WithdrawResponse
    {
        public bool IsSuccess { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
        
    }
}
