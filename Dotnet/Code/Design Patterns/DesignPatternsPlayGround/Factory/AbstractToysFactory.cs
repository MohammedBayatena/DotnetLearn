namespace DesignPatternsPlayGround.Factory;

//Interfaces
public interface ICarToy
{
    public void Drive();
}

public interface IDollToy
{
    public void MakeSound();
}

public interface IToyFactory
{
    ICarToy CreateCarToy();
    IDollToy CreateDollToy();
}

//Products Classes

public class LegoCar : ICarToy
{
    public void Drive()
    {
        Console.WriteLine("Lego car goes click-clack!");
    }
}

public class SegaCar : ICarToy
{
    public void Drive()
    {
        Console.WriteLine("SEGA car rolls smoothly!");
    }
}

public class LegoDoll : IDollToy
{
    public void MakeSound()
    {
        Console.WriteLine("Lego doll says blocky things!");
    }
}

public class SegaDoll : IDollToy
{
    public void MakeSound()
    {
        Console.WriteLine("SEGA doll sings a lullaby!");
    }
}

//Factories Classes
public class LegoFactory : IToyFactory
{
    public ICarToy CreateCarToy()
    {
        return new LegoCar();
    }

    public IDollToy CreateDollToy()
    {
        return new LegoDoll();
    }
}

public class SegaFactory : IToyFactory
{
    public ICarToy CreateCarToy()
    {
        return new SegaCar();
    }

    public IDollToy CreateDollToy()
    {
        return new SegaDoll();
    }
}

public class AbstractToysFactory(IToyFactory toyFactory) : IToyFactory
{
    public ICarToy CreateCarToy()
    {
        return toyFactory.CreateCarToy();
    }

    public IDollToy CreateDollToy()
    {
        return toyFactory.CreateDollToy();
    }
}