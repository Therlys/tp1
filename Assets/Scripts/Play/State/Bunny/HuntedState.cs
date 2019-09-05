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
            if(!bunny.IsBeingHunted()) return new SearchState(bunny);
            bunny.GoAwayFromNearestPredator();
            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}