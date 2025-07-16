namespace DesignPatternsPlayGround.Strategy.DependencyInjectedStrategies;

public interface IStrategy
{
    List<string> Validate(Vehicle vehicle);
    public bool IsApplicable(object objectToValidate);
}