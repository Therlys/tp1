﻿using UnityEngine;

namespace Game
{
    public class SearchState : BaseState
    {
        private readonly Animal animal;
        private const string STATE_TAG = "Searching...";
        public SearchState(Animal animal)
        {
            this.animal = animal;
        }

        public override void Enter()
        {
            animal.StateName = STATE_TAG;
        }
        
        public override IState Update()
        {
            if(!animal.IsFollowingPath) animal.MoveTo(null);

            if (animal.IsBeingHunted())
            {
                return new FleeState(animal);
            }

            else if (animal.IsHungry)
            {
                return new EatState(animal);
            }
            
            else if(animal.IsThirsty)
            {
                return new DrinkState(animal);
            }

            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}