using System.Collections;
using UnityEngine;

namespace Game
{
    public class EatState : BaseState
    {
        private readonly Animal animal;
        private const string STATE_TAG = "Eating...";

        public override void Enter()
        {
            animal.StateName = STATE_TAG;
        }
        

        public EatState(Animal animal)
        {
            this.animal = animal;
        }
        
        public override IState Update()
        {
            var eatable = animal.GetNearestEatable();
            if (eatable == null || !eatable.IsEatable || animal.Eat(eatable)) return new SearchState(animal);
            animal.MoveTo(eatable.Position);
            return this;
        }
    }
}