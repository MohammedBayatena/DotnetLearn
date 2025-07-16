namespace DesignPatternsPlayGround.Observer.SimpleObserver;

public class PhoneDisplay : IWeatherObserver
{
    public void Update(float temperature)
    {
        Console.WriteLine("Phone Display Observer Consumed a Message");
        Console.WriteLine($"Phone Display: Current temperature is {temperature}°C");
    }
}