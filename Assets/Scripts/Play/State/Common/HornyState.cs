namespace Game
{
    public class HornyState : BaseState
    {
        private readonly Animal animal = null;
#if UNITY_EDITOR
        private const string STATE_TAG = "Fucking...";
#endif

        public HornyState(Animal animal)
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
            return this;
        }
    }
}