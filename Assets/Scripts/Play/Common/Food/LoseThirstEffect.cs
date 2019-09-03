using UnityEngine;

namespace Game
{
    public struct LoseThirstEffect : IEffect
    {
        private readonly float nutritiveValue;

        public LoseThirstEffect(float nutritiveValue)
        {
            this.nutritiveValue = nutritiveValue;
        }

        public void ApplyOn(GameObject gameObject)
        {
            gameObject.GetComponentInChildren<VitalStats>().Drink(nutritiveValue);
        }
    }
}