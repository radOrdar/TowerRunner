using Tower;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/LevelData")]
    public class LevelData : ScriptableObject
    {
        public int towerLevels = 8;
        public int numLedge = 3;
        public int numOfGates = 3;
        public int distanceBtwGates = 10;
    }
}