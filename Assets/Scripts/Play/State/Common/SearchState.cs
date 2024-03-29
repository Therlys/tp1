﻿using UnityEngine;

namespace Game
{
    //Author: Jérémie Bertrand & Mike Bédard
    public class SearchState : BaseState
    {
        private readonly Animal animal = null;
#if UNITY_EDITOR
        private const string STATE_TAG = "Searching...";
#endif
        public SearchState(Animal animal)
        {
            this.animal = animal;
        }

        public override void Enter()
        {
#if UNITY_EDITOR
            animal.SetDebugStateTag(STATE_TAG);
#endif
        }
        
        public override IState Update()
        {
            if(!animal.IsFollowingPath) animal.MoveTo(null);

            if (animal.IsBeingHunted())
            {
                return new FleeState(animal);
            }
            
            if(animal.IsThirsty)
            {
                return new DrinkState(animal);
            }

            if (animal.IsHungry)
            {
                if (animal is IPredator)
                {
                    return new EatMovingTargetState(animal);
                }
                return new EatIdleTargetState(animal);
            }
            if (animal.IsHorny)
            {
                return new HornyState(animal);
            }

            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}