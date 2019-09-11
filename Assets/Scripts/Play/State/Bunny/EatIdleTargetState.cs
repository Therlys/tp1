namespace Game
{
    //Author: Jérémie Bertrand
    public class EatIdleTargetState : BaseState
    {
        private readonly Animal animal;
        private IEatable eatableTarget;
        
        private IEatable EatableTarget
        {
            get => eatableTarget;
            set
            {
                if (eatableTarget != value)
                {
                    eatableTarget = value;
                    if (eatableTarget != null) animal.MoveTo(value.Position);
                }
            }
        }
        
#if UNITY_EDITOR
        private const string STATE_TAG = "Eating...";
#endif

        public override void Enter()
        {
#if UNITY_EDITOR
            animal.SetDebugStateTag(STATE_TAG);
#endif
        }
        

        public EatIdleTargetState(Animal animal)
        {
            this.animal = animal;
            EatableTarget = animal.GetNearestEatable();
        }
        
        public override IState Update()
        {
            EatableTarget = animal.GetNearestEatable();
            if (EatableTarget == null || !EatableTarget.IsEatable || animal.Eat(EatableTarget)) return new SearchState(animal);
            return this;
        }
    }
}