using UnityEngine;

namespace Infrastructure
{
   public class EntryPoint : MonoBehaviour
   {
      [SerializeField] private AppStartup appStartup;

      private void Awake()
      {
         if (FindAnyObjectByType<AppStartup>() == null)
         {
            Instantiate(appStartup);
         }
      }
   }
}
