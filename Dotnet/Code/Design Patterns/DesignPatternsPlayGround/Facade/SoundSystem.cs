namespace DesignPatternsPlayGround.Facade;

public class SoundSystem
{
    public void On() => Console.WriteLine("Sound System turned ON");
    public void SetVolume(int level) => Console.WriteLine($"Volume set to {level}");
}