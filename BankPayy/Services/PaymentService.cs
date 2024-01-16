using BankPayy.Services.IServices;

namespace BankPayy.Services
{
    public class PaymentService : IPaymentService
    {
        public bool ProcessPayment(decimal amount)
        {
            return amount % 2 == 0;
        }


    }
}
