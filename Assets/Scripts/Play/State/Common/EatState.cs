namespace Game
{
    public class EatState : BaseState
    {
        private readonly Animal animal;
        private IEatable eatable = null;

        public override void Enter()
        {
            animal.SetCurrentTargetPosition(eatable.Position);
        }
        
        public EatState(Animal animal, IEatable eatable)
        {
            this.animal = animal;
            this.eatable = eatable;
        }
        
        public override IState Update()
        {
            if (eatable.IsEatable && !animal.Eat(eatable))
            {
                return this;
            }
            
            return new SearchState(animal);
        }

        public override void Leave()
        {
            animal.SetCurrentTargetPosition(null);
        }
    }
}