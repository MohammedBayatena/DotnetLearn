namespace DesignPatternsPlayGround.Observer.SimpleObserver;

public class WeatherStation : IWeatherStation
{
    private readonly List<IWeatherObserver> _observers = [];
    private float _temprature = 0;


    public void RegisterObserver(IWeatherObserver observer)
    {
        _observers.Add(observer);
    }

    public void UnregisterObserver(IWeatherObserver observer)
    {
        _observers.Remove(observer);
    }


    public void SetTemperature(float temp)
    {
        Console.WriteLine($"WeatherStation: Temperature updated to {temp}°C");
        _temprature = temp;
        NotifyObservers();
    }


    public void NotifyObservers()
    {
        foreach (var observer in _observers)
        {
            observer.Update(_temprature);
        }
    }
}