namespace DesignPatternsPlayGround.Observer.DotNetIObservable;

public class Alarm : IObservable<int>
{
    private readonly List<IObserver<int>> _observers = [];

    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!_observers.Contains(observer)) _observers.Add(observer);
        return new Unsubscriber(_observers, observer);
    }

    public void AlarmCriticalValue(int? value)
    {
        foreach (var observer in _observers)
        {
            if (!value.HasValue)
                observer.OnError(new Exception("Some Generic Exception Here"));
            else
                observer.OnNext(value.Value);
        }
    }

    public void EndTransmission()
    {
        foreach (var observer in _observers.ToArray())
            if (_observers.Contains(observer))
                observer.OnCompleted();

        _observers.Clear();
    }


    private class Unsubscriber(List<IObserver<int>> observers, IObserver<int> observer) : IDisposable
    {
        public void Dispose()
        {
            observers.Remove(observer);
        }
    }
}