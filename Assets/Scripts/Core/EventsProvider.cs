using System;
using Services;

namespace Core
{
    public class EventsProvider
    {
        public event Action GatePassed;
        public event Action GateCollided;
        public event Action FinishPassed;
        public event Action<bool> HasteSwitch; 

        public void OnGatePassed()
        {
            GatePassed?.Invoke();
        }

        public void OnGateCollided()
        {
            GateCollided?.Invoke();
        }

        public void OnFinishPassed()
        {
            FinishPassed?.Invoke();
        }

        public void OnHasteSwitch(bool enable)
        {
            HasteSwitch?.Invoke(enable);
        }
    }
}
