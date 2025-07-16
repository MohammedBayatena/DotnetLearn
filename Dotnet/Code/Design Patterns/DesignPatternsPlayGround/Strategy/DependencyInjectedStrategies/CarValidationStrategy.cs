namespace DesignPatternsPlayGround.Strategy.DependencyInjectedStrategies;

public class CarValidationStrategy : IStrategy
{
    public List<string> Validate(Vehicle vehicle)
    {
        var result = new List<string>();

        if (!IsApplicable(vehicle)) return result;
        if (vehicle.NumberOfSeats > 4)
        {
            result.Add("A Personal Car Should Not Have More than Four Seats");
        }

        if (vehicle.Weight > 3)
        {
            result.Add("A Personal Car Should Not Weight more than three Tons");
        }

        return result;
    }

    public bool IsApplicable(object objectToValidate)
    {
        return objectToValidate is Car;
    }
}