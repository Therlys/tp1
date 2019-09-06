using UnityEngine;

namespace Game
{
    public class FleeState : BaseState
    {
        private readonly Animal animal;
        private const string STATE_TAG = "Fleeing...";

        public override void Enter()
        {
            animal.StateName = STATE_TAG;
        }
        public FleeState(Animal animal)
        {
            this.animal = animal;
        }

        
        public override IState Update()
        {
            if(!animal.IsBeingHunted()) return new SearchState(animal);
            var predator = animal.GetNearestPredator();
            if (predator != null)
            {
                animal.GoAwayFrom(predator.Position);
            }
            else
            {
                return new SearchState(animal);
            }
            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}