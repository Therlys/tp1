using System.Collections;
using UnityEngine;

namespace Game
{
    public class EatState : BaseState
    {
        private readonly Animal animal;
        private IEatable eatable = null;

        public override void Enter()
        {
            eatable = animal.GetNearestEatable();
            animal.MoveTo(eatable.Position);
        }
        
        public EatState(Animal animal)
        {
            this.animal = animal;
        }
        
        public override IState Update()
        {
            eatable = animal.GetNearestEatable();
            if (eatable == null || !eatable.IsEatable || animal.Eat(eatable)) return new SearchState(animal);
            animal.MoveTo(eatable.Position);
            return this;

        }


        public override void Leave()
        {
            animal.MoveTo(null);
        }
    }
}