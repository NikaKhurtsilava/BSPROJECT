namespace BankPayy.Models
{
    public class WithdrawRequest
    {
        public bool IsSuccess { get; set; }
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
    }
}
