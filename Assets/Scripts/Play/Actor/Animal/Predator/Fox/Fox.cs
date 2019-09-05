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
    }
}