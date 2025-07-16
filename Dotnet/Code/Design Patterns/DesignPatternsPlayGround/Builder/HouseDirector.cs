namespace DesignPatternsPlayGround.Builder;

public class HouseDirector
{
    //The Director tells the builder what steps to take and in what order.
    public void Construct(IHouseBuilder builder)
    {
        builder.SetHouseSize();
        builder.BuildRoof();
        builder.BuildDoors();
        builder.BuildWindows();
        builder.BuildGarage();
        builder.BuildGarden();
    }
}