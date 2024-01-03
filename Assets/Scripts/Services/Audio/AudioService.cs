using StaticData;
using UnityEngine;

namespace Services.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioService : MonoBehaviour, IAudioService
    {
        private SoundsData _soundsData;
        private AudioSource _audioSource;
        
        public void Init(SoundsData soundsData)
        {
            _soundsData = soundsData;
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;
        }

        public void PlayMusic()
        {
            _audioSource.clip = _soundsData.music;
            _audioSource.Play();
        }

        public void PlayBump()
        {
            _audioSource.PlayOneShot(_soundsData.bump);
        }

        public void PlayDing()
        {
            _audioSource.PlayOneShot(_soundsData.ding);
        }

        public void PlayFinish()
        {
            _audioSource.Stop();
            _audioSource.clip = _soundsData.finishMusic;
            _audioSource.Play();
        }
    }
}