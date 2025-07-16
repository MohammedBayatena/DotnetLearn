namespace DesignPatternsPlayGround.Observer.SimpleObserver;

public class WindowDisplay : IWeatherObserver
{
    public void Update(float temperature)
    {
        Console.WriteLine("Window Display Observer Consumed a Message");
        Console.WriteLine($"Window Display: It's {temperature}°C outside.");
    }
}