namespace DesignPatternsPlayGround.Builder;

public interface IHouseBuilder
{
    void SetHouseSize();
    void BuildDoors();
    void BuildWindows();
    void BuildGarage();
    void BuildGarden();
    void BuildRoof();
}