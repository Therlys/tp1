namespace Game
{
    public class BaseStateMachine
    {
        private IState currentState;

        public BaseStateMachine(IState startingState)
        {
            currentState = startingState;
            currentState?.Enter();
        }

        public bool Update()
        {
            var nextState = currentState?.Update();
            if(nextState != currentState)
            {
                currentState?.Leave();
                currentState = nextState;
                currentState?.Enter();
            }
            return currentState != null;
        }
    }
}