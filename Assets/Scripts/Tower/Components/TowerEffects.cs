using Core;
using Cysharp.Threading.Tasks;
using Infrastructure;
using UnityEngine;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem towerSpeedFx;
        [SerializeField] private ParticleSystem fireWorkPf;

        private EventsProvider eventsProvider;
        
        private void Start()
        {
            eventsProvider = ProjectContext.I.EventsProvider;
            eventsProvider.HasteSwitch += SetEnabledSpeedFx;
            eventsProvider.FinishPassed += async () =>
            {
                SetEnabledSpeedFx(false);
                await UniTask.Delay(200);
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
