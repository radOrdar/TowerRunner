using System;
using Obstacle;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services.Generator
{
    public class GateGeneratorService : IGateGeneratorService
    {
        public GatePattern GeneratePattern(int[][,] yxz)
        {
            float rnd = Random.value;
            int patternWidth = yxz[0].GetLength(0);
            int patternHeight = yxz.Length;

            GatePattern pattern = new GatePattern();

            pattern.Direction = rnd switch
            {
                < 0.25f => Vector3.forward,
                < 0.5f => Vector3.back,
                < 0.75f => Vector3.right,
                _ => Vector3.left
            };
            
            int[,] matrix = new int[patternWidth, patternHeight];
            
            for (int level = 0; level < patternHeight; level++)
            {
                int[,] xz = yxz[level];

                for (int i = 0; i < xz.GetLength(0); i++)
                {
                    if (rnd < 0.25f)
                    {
                        if (xz[i, 2] != 1) 
                            matrix[i, level] = 1;
                    } else if (rnd < 0.5f)
                    {
                        if(xz[4 - i, 2] != 1)
                            matrix[i, level] = 1;
                    } else if (rnd < 0.75f)
                    {
                        if (xz[2, i] != 1) 
                            matrix[i, level] = 1;
                    } else
                    {
                        if (xz[2, 4 - i] != 1) 
                            matrix[i, level] = 1;
                    }
                }
            }
            pattern.Matrix = matrix;

            return pattern;
        }
    }
}