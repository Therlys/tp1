using UnityEngine;

namespace Game
{
    public sealed class Bunny : Animal, IPrey
    {
        [Header("Other")] [SerializeField] [Range(0f, 1f)] private float nutritiveValue = 1f;

        private BunnyDeathEventChannel bunnyDeathEventChannel;
        private BunnySpawnEventChannel bunnySpawnEventChannel;
        private const float MAXIMUM_HUNT_DETECT_DISTANCE = 20f;
        public bool IsEatable => !Vitals.IsDead;

        private new void Awake()
        {
            base.Awake();
            var bunnySensor = Sensor;
            var predatorSensor = bunnySensor.For<IPredator>();
            bunnyDeathEventChannel = Finder.BunnyDeathEventChannel;
            bunnySpawnEventChannel = Finder.BunnySpawnEventChannel;
        }

        private void Start()
        {
            bunnySpawnEventChannel.NotifyBunnySpawn();
        }

        public override IEatable GetNearestEatable()
        {
            IEatable eatable = null;
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                var vegetable = sensedObject.GetComponent<IVegetable>();
                if (vegetable != null && vegetable.IsEatable)
                {
                    if (eatable == null || MathExtensions.SquareDistanceBetween(Position, vegetable.Position) < MathExtensions.SquareDistanceBetween(Position, eatable.Position))
                    {
                        eatable = vegetable;
                    }
                }
            }
            return eatable;
        }

        public override IPredator GetNearestPredator()
        {
            IPredator predator = null;
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                Fox fox = sensedObject.GetComponent<Fox>();

                if (fox != null)
                {
                    if (predator == null || MathExtensions.SquareDistanceBetween(Position, fox.Position) < MathExtensions.SquareDistanceBetween(Position, predator.Position))
                    {
                        predator = fox;
                    }
                }
            }
            return predator;
        }
        
        public override Animal GetNearestFriend()
        {
            Animal friend = null;
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                var bunny = sensedObject.GetComponent<Bunny>();
                if (bunny != null && MathExtensions.SquareDistanceBetween(Position, bunny.Position) <=
                    OffspringCreator.ReproductionMaxRange && bunny.IsAvailable)
                {
                    if (friend == null || MathExtensions.SquareDistanceBetween(Position, bunny.Position) <
                        MathExtensions.SquareDistanceBetween(Position, friend.Position))
                    {
                        friend = bunny;
                        friend.AskToRecur(this);
                    }
                }
            }
            return friend;
        }

        public override bool IsBeingHunted()
        {
            var predator = GetNearestPredator();
            if (predator == null) return false;
            return MathExtensions.SquareDistanceBetween(Position, predator.Position) < MAXIMUM_HUNT_DETECT_DISTANCE;
        }

        public IEffect Eat()
        {
            bunnyDeathEventChannel.NotifyBunnyEatenDeath();
            Vitals.Die();

            return new LoseHungerEffect(nutritiveValue);
        }
    }
}