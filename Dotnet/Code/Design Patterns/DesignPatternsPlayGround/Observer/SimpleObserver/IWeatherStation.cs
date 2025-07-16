namespace DesignPatternsPlayGround.Observer.SimpleObserver;

public interface IWeatherStation
{
    void RegisterObserver(IWeatherObserver observer);
    void UnregisterObserver(IWeatherObserver observer);
    void NotifyObservers();
}