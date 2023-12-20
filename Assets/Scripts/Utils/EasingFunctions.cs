using UnityEngine;

namespace Utils
{
    public static class EasingFunctions
    {
        public static float EaseOutQuint(float t) =>
            1 - Mathf.Pow(1 - t, 5);

        public static float EaseOutCubic(float t) => 
            1 - Mathf.Pow(1 - t, 3);

        public static float EaseOutCirc(float t) => 
            Mathf.Sqrt(1 - Mathf.Pow(t - 1, 2));
    }
}
