﻿using System.Collections;
using UnityEngine;

namespace Game
{
    //Author: Jérémie Bertrand
    public class DrinkState : BaseState
    {
        private readonly Animal animal;
        private const string STATE_TAG = "Drinking...";

        public override void Enter()
        {
#if UNITY_EDITOR
            animal.SetDebugStateTag(STATE_TAG);
#endif
        }

        public DrinkState(Animal animal)
        {
            this.animal = animal;
        }
        
        public override IState Update()
        {
            var drinkable = animal.GetNearestDrinkable();
            if (drinkable == null || animal.Drink(drinkable)) return new SearchState(animal);
            animal.MoveTo(drinkable.Position);
            return this;

        }
    }
}