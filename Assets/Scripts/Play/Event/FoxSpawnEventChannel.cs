using UnityEngine;

namespace Game
{
    public class FoxSpawnEventChannel : MonoBehaviour
    {
        public event FoxSpawnEventHandler OnFoxSpawn;

        public void NotifyFoxSpawn()
        {
            if (OnFoxSpawn != null) OnFoxSpawn();
        }

        public delegate void FoxSpawnEventHandler();
        
    }
}