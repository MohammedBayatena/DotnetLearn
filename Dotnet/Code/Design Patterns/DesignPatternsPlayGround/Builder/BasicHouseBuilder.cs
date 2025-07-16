namespace DesignPatternsPlayGround.Builder;

public class BasicHouseBuilder : IHouseBuilder
{
    private readonly ToyHouse _house = new ToyHouse();

    public void SetHouseSize()
    {
        _house.HouseSize = 50;
    }

    public void BuildDoors()
    {
        _house.NumberOfDoors = 2;
    }

    public void BuildWindows()
    {
        _house.NumberOfWindows = 4;
    }

    public void BuildGarage()
    {
        _house.HasGarage = false;
    }

    public void BuildGarden()
    {
        _house.HasGarage = false;
    }

    public void BuildRoof()
    {
        _house.RoofColor = "Red";
    }

    public ToyHouse GetResult()
    {
        return _house;
    }
}