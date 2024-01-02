using UnityEngine;

namespace Tower.Components
{
    [RequireComponent(typeof(TowerMove))]
    public class TowerEffects : MonoBehaviour
    {
        [SerializeField] private ParticleSystem towerSpeedFx;
        [SerializeField] private ParticleSystem fireWorkPf;

        private void Start()
        {
            GetComponent<TowerMove>().OnHasteSwitch += SetEnabledSpeedFx;
            GetComponentInChildren<TowerCollision>().OnFinishPassed += () => SetEnabledSpeedFx(false);
            GetComponentInChildren<TowerCollision>().OnFinishPassed += async () =>
            {
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
