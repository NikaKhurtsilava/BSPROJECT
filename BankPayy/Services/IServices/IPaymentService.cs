namespace BankPayy.Services.IServices
{
    public interface IPaymentService
    {
        bool ProcessPayment(decimal amount);
    }
}
