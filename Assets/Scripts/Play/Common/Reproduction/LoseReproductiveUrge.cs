using UnityEngine;

namespace Game
{
    public struct LoseReproductiveUrge : IEffect
    {
        public void ApplyOn(GameObject gameObject)
        {
            gameObject.GetComponentInChildren<VitalStats>().HaveSex();
        }
    }
}