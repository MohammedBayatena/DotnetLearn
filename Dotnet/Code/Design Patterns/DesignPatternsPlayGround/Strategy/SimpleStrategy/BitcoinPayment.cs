namespace DesignPatternsPlayGround.Strategy.SimpleStrategy;

public class BitcoinPayment : IPaymentStrategy
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount:C} using Bitcoin.");
    }
}