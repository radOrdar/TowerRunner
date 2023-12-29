using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "SoundData", menuName = "StaticData/SoundsData")]
    public class SoundsData : ScriptableObject
    {
        public AudioClip music;
        public AudioClip bump;
        public AudioClip ding;
    }
}