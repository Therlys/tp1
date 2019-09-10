﻿using UnityEngine;

namespace Game
{
    public class FoxDeathEventChannel : MonoBehaviour
    {
        public event FoxHungerDeathEventHandler OnFoxHungerDeath;
        public event FoxThirstDeathEventHandler OnFoxThirstDeath;

        public void NotifyFoxHungerDeath()
        {
            if (OnFoxHungerDeath != null) OnFoxHungerDeath();
        }
        
        public void NotifyFoxThirstDeath()
        {
            if (OnFoxThirstDeath != null) OnFoxThirstDeath();
        }
        
        public delegate void FoxHungerDeathEventHandler();
        public delegate void FoxThirstDeathEventHandler();
    }
}