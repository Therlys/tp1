using UnityEngine;

namespace Game
{
    public sealed class Feeder : MonoBehaviour
    {
        [SerializeField] private float maxEatDistance = 1f;
        [SerializeField] private float maxDrinkDistance = 1f;

        private IEntity entity;

        public float MaxEatDistance => maxEatDistance;
        public float MaxDrinkDistance => maxDrinkDistance;

        private void Awake()
        {
            entity = transform.parent.GetComponent<IEntity>();
        }

        public bool Eat(IEatable eatable)
        {
            if (IsInReach(eatable))
            {
                eatable.Eat().ApplyOn(transform.parent.gameObject);
                return true;
            }

            return false;
        }

        public bool Drink(IDrinkable drinkable)
        {
            if (IsInReach(drinkable))
            {
                drinkable.Drink().ApplyOn(transform.parent.gameObject);
                return true;
            }

            return false;
        }

        private bool IsInReach(IEatable eatable)
        {
            return IsInReach(eatable, maxEatDistance);
        }

        private bool IsInReach(IDrinkable eatable)
        {
            return IsInReach(eatable, maxDrinkDistance);
        }

        private bool IsInReach(IEntity eatable, float maxDistance)
        {
            //Use a square to compute max distance for performance reasons.
            if (Mathf.Abs(entity.Position.x - eatable.Position.x) <= maxDistance) return true;
            if (Mathf.Abs(entity.Position.y - eatable.Position.y) <= maxDistance) return true;
            return false;
        }
    }
}