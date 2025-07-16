namespace DesignPatternsPlayGround.Facade;

public class Projector
{
    public void On() => Console.WriteLine("Projector turned ON");
    public void SetInput(string input) => Console.WriteLine($"Projector input set to {input}");
}