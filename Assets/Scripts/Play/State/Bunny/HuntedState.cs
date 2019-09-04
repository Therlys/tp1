namespace Game
{
    public class HuntedState : BaseState
    {
        private readonly Bunny bunny;
        public HuntedState(Bunny bunny)
        {
            this.bunny = bunny;
        }

        public override void Enter()
        {
            base.Enter();
        }
        
        public override IState Update()
        {
            
            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}