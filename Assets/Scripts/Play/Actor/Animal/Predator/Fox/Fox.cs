using UnityEngine;

namespace Game
{
    public sealed class Fox : Animal, IPredator
    {
        //TODO : Compléter le comportement du renard.
        private void Update()
        {
            foreach (var sensedObject in Sensor.SensedObjects)
            {
                var bunny = sensedObject.GetComponent<Bunny>();
                if (bunny != null)
                {
                    // Je suis un @$@!@ de lapin
                    Mover.MoveTo(sensedObject.transform.position);
                    bunny.Eat();
                    break;
                }
            }
        }
    }
}