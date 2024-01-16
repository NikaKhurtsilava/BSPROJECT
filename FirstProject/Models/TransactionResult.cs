
using FirstProjectTest.Enums;

namespace FirstProjectTest.Models
{
    public class TransactionResult
    {
        public bool Success { get; set; }
        public TransactionStatus Status { get; set; }
        public string Message { get; set; }
        public int TransactionId { get; set; }

    }
}
