using Tower;

namespace Services.Generator
{
    public interface ITowerGeneratorService : IService
    {
        TowerPattern GeneratePattern(int towerLevels, int numLedge);
    }
}