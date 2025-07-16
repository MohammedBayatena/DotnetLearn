namespace DesignPatternsPlayGround.Factory;



//Toy Types Enum
public enum ToyType
{
    Car,
    Miku,
    Robot
}

//IToy Interface (The Toy BluePrint)
public interface IToy
{
    void Play();
}

// The Toys (Production Lines)
public class CarToy : IToy
{
    public void Play()
    {
        Console.WriteLine("Vroom! I'm a Car Toy.");
    }
}

public class MikuToy : IToy
{
    public void Play()
    {
        Console.WriteLine("Miku Miku Beaaaam!");
    }
}

public class RobotToy : IToy
{
    public void Play()
    {
        Console.WriteLine("Beep boop! I'm a Robot Toy.");
    }
}

//The Factory
public class ToysFactory
{
    public IToy CreateToy(ToyType type)
    {
        IToy toy = type switch
        {
            ToyType.Car => new CarToy(),
            ToyType.Miku => new MikuToy(),
            ToyType.Robot => new RobotToy(),
            _ => throw new NotImplementedException()
        };
        return toy;
    }
}