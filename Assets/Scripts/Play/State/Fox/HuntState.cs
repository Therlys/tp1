namespace Game
{
    public class HuntState : BaseState
    {
        private readonly Fox fox;
        public HuntState(Fox fox)
        {
            this.fox = fox;
        }

        public override void Enter()
        {
            base.Enter();
        }
        
        public override IState Update()
        {
            if (!fox.IsHungry)
            {
                return new SearchState(fox);
            }
            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}