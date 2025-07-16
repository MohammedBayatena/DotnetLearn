namespace DesignPatternsPlayGround.Strategy.SimpleStrategy;

public interface IPaymentStrategy
{
    void Pay(decimal amount);
}