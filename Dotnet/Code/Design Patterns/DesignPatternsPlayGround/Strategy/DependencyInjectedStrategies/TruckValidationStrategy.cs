namespace DesignPatternsPlayGround.Strategy.DependencyInjectedStrategies;

public class TruckValidationStrategy : IStrategy
{
    public List<string> Validate(Vehicle vehicle)
    {
        var result = new List<string>();

        if (!IsApplicable(vehicle)) return result;
        if (vehicle.NumberOfSeats > 2)
        {
            result.Add("A Truck Should Not Have More than Two Seats");
        }

        if (vehicle.NumberOfWheels < 8)
        {
            result.Add("A Truck Should Not have less than 8 wheels");
        }

        return result;
    }

    public bool IsApplicable(object objectToValidate)
    {
        return objectToValidate is Truck;
    }
}