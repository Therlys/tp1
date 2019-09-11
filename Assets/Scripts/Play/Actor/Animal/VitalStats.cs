using System;
using UnityEngine;

namespace Game
{
    public sealed class VitalStats : MonoBehaviour
    {
        [Header("Initial stats")] [SerializeField] [Range(0f, 1f)] private float initialHunger = 0f;
        [SerializeField] [Range(0f, 1f)] private float initialThirst = 0f;
        [SerializeField] [Range(0f, 1f)] private float initialReproductiveUrge = 0f;
        [Header("Stats consumption")] [SerializeField] private float hungerPerSecond = 0.005f;
        [SerializeField] private float thirstPerSecond = 0.01f;
        [SerializeField] private float reproductiveUrgePerSecond = 0.1f;
        [Header("Death")] [SerializeField] private float hungerDeathThreshold = 1f;
        [SerializeField] private float thirstDeathThreshold = 1f;
        private BunnyDeathEventChannel bunnyDeathEventChannel;
        private FoxDeathEventChannel foxDeathEventChannel;
        private Type ownerType = null;


        private float hunger;
        private float thirst;
        private float reproductiveUrge;
        private bool isDead;

        public event VitalStatsEventHandler OnDeath;

        public void SetOwnerType (Type type)
        {
            ownerType = type;
        }

        public float Hunger
        {
            get => hunger;
            private set => hunger = Mathf.Clamp01(value);
        }

        public float Thirst
        {
            get => thirst;
            private set => thirst = Mathf.Clamp01(value);
        }

        public float ReproductiveUrge
        {
            get => reproductiveUrge;
            private set => reproductiveUrge = Mathf.Clamp01(value);
        }
        

        public bool IsDead
        {
            get => isDead;
            private set
            {
                if (value && !isDead)
                {
                    isDead = true;
                    NotifyDeath();
                }
            }
        }

        private void Awake()
        {
            bunnyDeathEventChannel = Finder.BunnyDeathEventChannel;
            foxDeathEventChannel = Finder.FoxDeathEventChannel;
        }

        private void Start()
        {
            hunger = initialHunger;
            thirst = initialThirst;
            reproductiveUrge = initialReproductiveUrge;
            isDead = false;
        }

        private void Update()
        {
            if (!IsDead)
            {
                Hunger += hungerPerSecond * Time.deltaTime;
                Thirst += thirstPerSecond * Time.deltaTime;
                ReproductiveUrge += reproductiveUrgePerSecond * Time.deltaTime;

                // Author: Mike Bédard
                if (Hunger >= hungerDeathThreshold)
                {
                    if(ownerType == typeof(Fox))
                        foxDeathEventChannel.NotifyFoxHungerDeath();
                    else if(ownerType == typeof(Bunny))
                        bunnyDeathEventChannel.NotifyBunnyHungerDeath();
                    Die();
                }

                if (Thirst >= thirstDeathThreshold)
                {
                    if(ownerType == typeof(Fox))
                        foxDeathEventChannel.NotifyFoxThirstDeath();
                    else if (ownerType == typeof(Bunny))
                        bunnyDeathEventChannel.NotifyBunnyThirstDeath();
                    Die();
                }
            }
        }

        public void Eat(float nutritiveValue)
        {
            Hunger -= nutritiveValue;
        }

        public void Drink(float nutritiveValue)
        {
            Thirst -= nutritiveValue;
        }

        public void HaveSex()
        {
            ReproductiveUrge = 0;
        }

        public void Die()
        {
            IsDead = true;
        }

        private void NotifyDeath()
        {
            if (OnDeath != null) OnDeath();
        }
    }

    public delegate void VitalStatsEventHandler();
}