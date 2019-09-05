using UnityEngine;

namespace Game
{
    public class EatState : BaseState
    {
        private readonly Animal animal;
        private IEatable eatable = null;
        private Vector3 previousEatablePosition;

        public override void Enter()
        {
            eatable = animal.GetNearestEatable();
            previousEatablePosition = eatable.Position;
            animal.MoveTo(previousEatablePosition);
            
        }
        
        public EatState(Animal animal)
        {
            this.animal = animal;
        }
        
        public override IState Update()
        {
            eatable = animal.GetNearestEatable();
            if (eatable.IsEatable && !animal.Eat(eatable))
            {
                if (previousEatablePosition != eatable.Position)
                {
                    previousEatablePosition = eatable.Position;
                    animal.MoveTo(eatable.Position);   
                }
                return this;
            }
            return new SearchState(animal);

        }

        public override void Leave()
        {
            animal.MoveTo(null);
        }
    }
}