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
        }
        
        public override IState Update()
        {
            if(!animal.IsFollowingPath) animal.MoveTo(null);
            if (animal.IsHungry)
            {
                return new EatState(animal);
            }
            else if(animal.IsHorny)
            {
                return new DrinkState(animal);
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