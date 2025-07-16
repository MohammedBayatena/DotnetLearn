namespace DesignPatternsPlayGround.Strategy.SimpleStrategy;

public class ShoppingCart()
{
    private IPaymentStrategy? _paymentStrategy;

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy;
    }

    public void Checkout(decimal amount)
    {
        if (_paymentStrategy == null)
        {
            Console.WriteLine("Payment strategy not selected.");
        }
        else
        {
            _paymentStrategy.Pay(amount);
        }
    }
}