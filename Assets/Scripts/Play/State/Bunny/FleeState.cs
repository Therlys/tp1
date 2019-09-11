using UnityEngine;

namespace Game
{
    //Author: Mike Bédard
    public class FleeState : BaseState
    {
        private readonly Animal animal;
#if UNITY_EDITOR
        private const string STATE_TAG = "Fleeing...";
#endif

        public override void Enter()
        {
#if UNITY_EDITOR
            animal.SetDebugStateTag(STATE_TAG);
#endif
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