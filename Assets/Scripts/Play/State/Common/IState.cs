namespace Game
{
    //Author: Jérémie Bertrand
    public interface IState
    {
        void Enter();
        IState Update();
        void Leave();
    }
}