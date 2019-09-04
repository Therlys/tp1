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
            animal.MoveTo(null);
        }
        
        public override IState Update()
        {
            if (animal.IsHungry)
            {
                return new EatState(animal);
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