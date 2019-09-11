using UnityEngine;

namespace Game
{
    //Author: Mike Bédard
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