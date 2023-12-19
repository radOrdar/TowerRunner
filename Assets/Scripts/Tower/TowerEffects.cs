using UnityEngine;

public class TowerEffects : MonoBehaviour
{
    [SerializeField] private ParticleSystem towerSpeedFx;

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
