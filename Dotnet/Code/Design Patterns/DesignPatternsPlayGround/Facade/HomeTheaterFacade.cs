namespace DesignPatternsPlayGround.Facade;

public class HomeTheaterFacade(Projector p, SoundSystem s, Lights l, DVDPlayer d)
{
    public void StartMovie(string movie)
    {
        Console.WriteLine("Starting Movie Night...");

        l.Dim();
        p.On();
        p.SetInput("DVD");
        s.On();
        s.SetVolume(10);
        d.On();
        d.Play(movie);
    }
}