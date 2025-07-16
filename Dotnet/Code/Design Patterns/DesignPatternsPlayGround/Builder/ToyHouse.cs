namespace DesignPatternsPlayGround.Builder;

public class ToyHouse
{
    public bool HasGarden { get; set; }
    public int NumberOfDoors { get; set; }
    public int NumberOfWindows { get; set; }
    public bool HasGarage { get; set; }
    public string RoofColor { get; set; }
    public int HouseSize { get; set; }


    public void DescribeTheHouse()
    {
        var description = $"A {HouseSize} square meters toy house with a {RoofColor} Roof," +
                          $" {NumberOfDoors} Doors," +
                          $" {NumberOfWindows} Windows";

        if (HasGarage)
        {
            description += ". Containing a Garage";
        }

        if (HasGarden)
        {
            description += ". With a Garden";
        }


        Console.WriteLine(description);
    }
}