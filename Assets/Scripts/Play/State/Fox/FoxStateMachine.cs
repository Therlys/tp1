namespace Game
{
    public class FoxStateMachine
    {
        private IState currentState;

        public FoxStateMachine(IState startingState, Fox fox)
        {
            currentState = startingState;
            currentState?.Enter();
        }

        public bool Update()
        {
            var nextState = currentState?.Update();
            if (nextState != currentState)
            {
                currentState?.Leave();
                currentState = nextState;
                currentState?.Enter();
            }

            return currentState != null;
        }
    }
}