using UnityEngine;

public class GameRunner : MonoBehaviour
{
   [SerializeField] private Bootstrap bootstrap;

   private void Awake()
   {
      if (FindAnyObjectByType<Bootstrap>() == null)
      {
         Instantiate(bootstrap);
      }
   }
}
