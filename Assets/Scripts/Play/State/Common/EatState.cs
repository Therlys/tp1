using System.Collections;
using UnityEngine;

namespace Game
{
    public class EatState : BaseState
    {
        private readonly Animal animal;

        public EatState(Animal animal)
        {
            this.animal = animal;
        }
        
        public override IState Update()
        {
            var eatable = animal.GetNearestEatable();
            if (eatable == null || !eatable.IsEatable || animal.Eat(eatable)) return new SearchState(animal);
            animal.MoveTo(eatable.Position);
            return this;

        }
    }
}