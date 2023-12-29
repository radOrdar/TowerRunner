using StaticData;

namespace Services.Audio
{
    public interface IAudioService : IService
    {
        void Init(SoundsData soundsData);
        void PlayMusic();
        void PlayBump();
        void PlayDing();
    }
}