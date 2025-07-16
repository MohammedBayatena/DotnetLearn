namespace DesignPatternsPlayGround.Builder;

public static class HouseBuilderHelper
{
    public static ToyHouse WithHouseSize(this ToyHouse house, int size)
    {
        house.HouseSize = size;
        return house;
    }

    public static ToyHouse WithDoorsNumber(this ToyHouse house, int doorsNumber)
    {
        house.NumberOfDoors = doorsNumber;
        return house;
    }

    public static ToyHouse WithWindowsNumber(this ToyHouse house, int windowsNumber)
    {
        house.NumberOfWindows = windowsNumber;
        return house;
    }

    public static ToyHouse WithGarage(this ToyHouse house, bool hasGarage)
    {
        house.HasGarage = hasGarage;
        return house;
    }

    public static ToyHouse WithGarden(this ToyHouse house, bool hasGarden)
    {
        house.HasGarden = hasGarden;
        return house;
    }

    public static ToyHouse WithRoofColor(this ToyHouse house, string roofColor)
    {
        house.RoofColor = roofColor;
        return house;
    }
}