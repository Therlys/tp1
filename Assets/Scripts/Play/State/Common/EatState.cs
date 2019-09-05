namespace Game
{
    public class EatState : BaseState
    {
        private readonly Animal animal;
        private readonly IEatable eatable;

        public EatState(Animal animal, IEatable eatable)
        {
            this.animal = animal;
            this.eatable = eatable;
        }
        
        public override void Enter()
        {
            animal.MoveTo(eatable.Position);
        }
        
        public override IState Update()
        {
            if (eatable.IsEatable && !animal.Eat(eatable)) return this;

            return new SearchState(animal);
        }

        public override void Leave()
        {
        }
    }
}