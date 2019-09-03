namespace Game
{
    public abstract class BaseState : IState
    {
        public virtual void Enter()
        {
            // Empty on purpose. Nothing to do by default.
        }

        public abstract IState Update();

        public virtual void Leave()
        {
            // Empty on purpose. Nothing to do by default.
        }
    }
}