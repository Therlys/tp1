using UnityEngine;

namespace Game
{
    public struct LoseHungerEffect : IEffect
    {
        private readonly float nutritiveValue;

        public LoseHungerEffect(float nutritiveValue)
        {
            this.nutritiveValue = nutritiveValue;
        }

        public void ApplyOn(GameObject gameObject)
        {
            gameObject.GetComponentInChildren<VitalStats>().Eat(nutritiveValue);
        }
    }
}