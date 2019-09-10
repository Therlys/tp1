using UnityEngine;

namespace Game
{
    public sealed class Fox : Animal, IPredator
    {
        public override IEatable GetNearestEatable()
        {
            IEatable eatable = null;
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                var bunny = sensedObject.GetComponent<Bunny>();
                if (bunny != null && bunny.IsEatable)
                {
                    if (eatable == null || MathExtensions.SquareDistanceBetween(Position, bunny.Position) < MathExtensions.SquareDistanceBetween(Position, eatable.Position))
                    {
                        eatable = bunny;
                    }
                }
            }
            return eatable;
        }
        
        public override Animal GetNearestFriend()
        {
            Animal friend = null;
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                var fox = sensedObject.GetComponent<Fox>();
                if (fox != null && MathExtensions.SquareDistanceBetween(Position, fox.Position) <=
                    OffspringCreator.ReproductionMaxRange && fox.IsAvailable)
                {
                    if (friend == null || MathExtensions.SquareDistanceBetween(Position, fox.Position) <
                        MathExtensions.SquareDistanceBetween(Position, friend.Position))
                    {
                        if(friend != null) friend.StopRecurring(this);
                        friend = fox;
                        friend.AskToRecur(this);
                    }
                }
            }
            return friend;
        }

        public override IPredator GetNearestPredator()
        {
            return null;
        }

        public override bool IsBeingHunted()
        {
            return false;
        }
    }
}