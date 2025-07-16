namespace DesignPatternsPlayGround.Builder;

public class FancyHouseBuilder
{
    private readonly ToyHouse _house = new ToyHouse()
        .WithDoorsNumber(3)
        .WithWindowsNumber(12)
        .WithGarage(true)
        .WithGarden(true)
        .WithHouseSize(250)
        .WithRoofColor("green");

    public ToyHouse GetResult()
    {
        return _house;
    }
}