using Obstacle;

namespace Services.Generator
{
    public interface IGateGeneratorService : IService
    {

        public GatePattern GeneratePattern(int[][,] yxz);
    }
}