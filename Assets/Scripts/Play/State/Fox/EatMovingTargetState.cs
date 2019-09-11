namespace Game
{
    //Author: Jérémie Bertrand
    public class EatMovingTargetState : BaseState
    {
        private readonly Animal animal;
#if UNITY_EDITOR
        private const string STATE_TAG = "Eating...";
#endif

        public override void Enter()
        {
#if UNITY_EDITOR
            animal.SetDebugStateTag(STATE_TAG);
#endif
        }
        

        public EatMovingTargetState(Animal animal)
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