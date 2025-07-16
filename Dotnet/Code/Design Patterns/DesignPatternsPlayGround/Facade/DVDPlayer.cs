namespace DesignPatternsPlayGround.Facade;

public class DVDPlayer
{
    public void On() => Console.WriteLine("DVD Player turned ON");
    public void Play(string movie) => Console.WriteLine($"Playing movie: {movie}");
}