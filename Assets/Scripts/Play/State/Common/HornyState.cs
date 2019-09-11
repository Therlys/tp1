namespace Game
{
    //Author: Jérémie Bertrand
    public class HornyState : BaseState
    {
        private readonly Animal animal = null;
#if UNITY_EDITOR
        private const string STATE_TAG = "Recurring...";
#endif

        public HornyState(Animal animal)
        {
            this.animal = animal;
        }
        
        public override void Enter()
        {
            animal.SetIsRecurring(true);
#if UNITY_EDITOR
            animal.SetDebugStateTag(STATE_TAG);
#endif
        }

        public override IState Update()
        {
            var friend = animal.GetNearestFriend();
            if (friend == null || !friend.IsAvailable || animal.CreateOffspringWith(friend)) return new SearchState(animal);
            animal.MoveTo(friend.Position);
            return this;
        }

        public override void Leave()
        {
            animal.SetIsRecurring(false);
        }
    }
}