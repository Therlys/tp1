using UnityEngine;

namespace Game
{
    //Author: Mike Bédard
    public class BunnySpawnEventChannel : MonoBehaviour
    {
        public event BunnySpawnEventHandler OnBunnySpawn;

        public void NotifyBunnySpawn()
        {
            if (OnBunnySpawn != null) OnBunnySpawn();
        }

        public delegate void BunnySpawnEventHandler();
        
    }
}