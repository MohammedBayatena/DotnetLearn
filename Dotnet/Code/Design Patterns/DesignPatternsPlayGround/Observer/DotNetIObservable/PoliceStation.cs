namespace DesignPatternsPlayGround.Observer.DotNetIObservable;

public class PoliceStation : IObserver<int>
{
    private readonly string _name = "PoliceStation";

    private IDisposable? _unsubscriber;


    public PoliceStation(string name)
    {
        _name = name;
    }

    public virtual void Subscribe(IObservable<int>? provider)
    {
        if (provider != null)
            _unsubscriber = provider.Subscribe(this);
    }


    public virtual void OnCompleted()
    {
        Console.WriteLine("The Critical Alarm has completed transmitting data to {0}.", _name);
        Unsubscribe();
    }

    public virtual void OnError(Exception e)
    {
        Console.WriteLine("{0}: The Alarm Value cannot be determined.", _name);
    }

    public virtual void OnNext(int value)
    {
        Console.WriteLine("{1}: The current value is {0}", value, _name);
    }

    public virtual void Unsubscribe()
    {
        _unsubscriber?.Dispose();
    }
}