namespace BankPayy.Models
{
    public class DepositRequest
    {
        public int TransactionId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public bool IsSuccess { get; set; }
    }
}
