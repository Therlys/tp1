using UnityEngine;

namespace Game
{
    public class SearchState : BaseState
    {
        private readonly Animal animal;
        public SearchState(Animal animal)
        {
            this.animal = animal;
        }

        public override void Enter()
        {
            base.Enter();
        }
        
        public override IState Update()
        {
            if (animal.IsHungry)
            {
                var eatable = animal.GetNearestEatable();
                if (eatable != null) return new EatState(animal, eatable);
            }
            else if(animal.IsHorny)
            {
                // Chercher partenaire
            }
            else
            {
            }
            
            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}