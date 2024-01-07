using Services;
using Services.Event;
using UnityEngine;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem towerSpeedFx;
        [SerializeField] private ParticleSystem fireWorkPf;

        private EventService _eventService;
        
        private void Start()
        {
            _eventService = AllServices.Instance.Get<EventService>();
            _eventService.HasteSwitch += SetEnabledSpeedFx;
            _eventService.FinishPassed += async () =>
            {
                SetEnabledSpeedFx(false);
                await Awaitable.WaitForSecondsAsync(0.2f);
                var firework = Instantiate(fireWorkPf, transform);
                firework.transform.localPosition = Vector3.up * 10;
            };
        }

        public void SetEnabledSpeedFx(bool enable)
        {
            if (enable)
            {
                towerSpeedFx.Play();
            } else
            {
                towerSpeedFx.Stop();
            }
        }
    }
}
