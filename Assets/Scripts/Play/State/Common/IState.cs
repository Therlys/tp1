namespace Game
{
    public interface IState
    {
        void Enter();
        IState Update();
        void Leave();
    }
}