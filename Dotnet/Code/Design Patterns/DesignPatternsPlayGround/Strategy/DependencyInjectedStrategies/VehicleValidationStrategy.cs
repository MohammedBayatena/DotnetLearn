namespace DesignPatternsPlayGround.Strategy.DependencyInjectedStrategies;

public class VehicleValidationStrategy : IStrategy
{
    public List<string> Validate(Vehicle vehicle)
    {
        var result = new List<string>();

        if (!IsApplicable(vehicle)) return result;
        if (vehicle.LicensePlateNumber == null)
        {
            result.Add("A Vehicle LicensePlateNumber Cannot Be Null");
        }

        if (vehicle.NumberOfWheels < 2)
        {
            result.Add("A Vehicle must have at least two wheels");
        }

        return result;
    }

    public bool IsApplicable(object objectToValidate)
    {
        return objectToValidate is Vehicle;
    }
}