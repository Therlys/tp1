namespace Game
{
    //Author: Jérémie Bertrand
    public class StateMachine
    {
        private IState currentState;

        public StateMachine(Animal animal)
        {
            currentState = new SearchState(animal);
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