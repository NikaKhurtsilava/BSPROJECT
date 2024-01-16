namespace FirstProjectTest.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public int TransactionStatus { get; set; }
        public decimal CurrentBalance { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
