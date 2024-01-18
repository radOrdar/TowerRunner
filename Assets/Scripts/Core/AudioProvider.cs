﻿using StaticData;
using UnityEngine;

namespace Core.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioProvider : MonoBehaviour
    {
        [SerializeField] private SoundsData _soundsData;
        private AudioSource _audioSource;
        
        public bool Muted
        {
            get => _audioSource.mute;
            set => _audioSource.mute = value;
        }
        
        private void Awake()
        {
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