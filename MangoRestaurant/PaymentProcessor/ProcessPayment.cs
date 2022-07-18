namespace PaymentProcessor
{
    public class ProcessPayment : IProcessPayment
    {
        public async Task<bool> PaymentProcessor()
        {
            return true;
        }
    }
}