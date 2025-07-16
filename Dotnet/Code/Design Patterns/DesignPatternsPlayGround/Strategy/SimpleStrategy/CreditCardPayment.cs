namespace DesignPatternsPlayGround.Strategy.SimpleStrategy;

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using Credit Card.");
    }
}