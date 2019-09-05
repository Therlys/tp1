using System.Collections;
using UnityEngine;

namespace Game
{
    public class DrinkState : BaseState
    {
        private readonly Animal animal;
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