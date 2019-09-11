﻿using System;
 using UnityEngine;

namespace Game
{
    //Author: Mike Bédard
    public class BunnyDeathEventChannel : MonoBehaviour
    {
        public event BunnyHungerDeathEventHandler OnBunnyHungerDeath;
        public event BunnyThirstDeathEventHandler OnBunnyThirstDeath;
        public event BunnyEatenDeathEventHandler OnBunnyEatenDeath;

        public void NotifyBunnyHungerDeath()
        {
            if (OnBunnyHungerDeath != null) OnBunnyHungerDeath();
        }
        
        public void NotifyBunnyThirstDeath()
        {
            if (OnBunnyThirstDeath != null) OnBunnyThirstDeath();
        }
        
        public void NotifyBunnyEatenDeath()
        {
            if (OnBunnyEatenDeath != null) OnBunnyEatenDeath();
        }

        public delegate void BunnyHungerDeathEventHandler();
        public delegate void BunnyThirstDeathEventHandler();
        public delegate void BunnyEatenDeathEventHandler();

    }
}