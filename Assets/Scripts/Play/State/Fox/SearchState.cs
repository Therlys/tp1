namespace Game
{
    public class SearchState : BaseState
    {
        private readonly Fox fox;
        public SearchState(Fox fox)
        {
            this.fox = fox;
        }

        public override void Enter()
        {
            base.Enter();
        }
        
        public override IState Update()
        {
            if (fox.IsThirsty)
            {
                // Chercher nourriture
                
            }
            else if (fox.IsHungry)
            {
                // Chercher lapin
            }
            else if(fox.IsHorny)
            {
                // Chercher partenaire
            }
            else
            {
                // Déplacement random
            }
            
            return this;
        }

        public override void Leave()
        {
            base.Leave();
        }
    }
}