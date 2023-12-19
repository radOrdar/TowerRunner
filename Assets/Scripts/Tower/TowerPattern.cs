using System.Collections.Generic;
using Obstacle;
using UnityEngine;

namespace Tower
{
    public class TowerPattern
    {
        public int[][,] matrix;
        public Dictionary<Vector3, int[,]> towerProjections;
    }
}
