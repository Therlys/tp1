using UnityEngine;

namespace Game
{
    public sealed class Bunny : Animal, IPrey
    {
        [Header("Other")] [SerializeField] [Range(0f, 1f)] private float nutritiveValue = 1f;

        public bool IsEatable => !Vitals.IsDead;

        protected new void Awake()
        {
            base.Awake();
            
            //TODO : Compléter le comportement du lapin.

            //TODO : Lire ce commentaire.
            //IMPORTANT!!! Chaque animal est composé d'un senseur et d'un stimuli.
            //             Le stimuli sert à indiquer la présence de l'entité.
            //             Le senseur sert à détecter les stimulis.
            //
            //             De base. les senseurs détectent tous les stimulis.
            //             Autrement dit, ils ne font pas la distinction entre un "Bunny" et un "Fox".
            //
            //             Il est cependant possible de créer un sous-senseur spécialisé", faisant office
            //             de filtre. Ce dernier est donc capable de faire la distinction entre une "Bunny"
            //             et un "Fox.
            //
            //             Pour créer un sous-senseur, utilisez la méthode générique "For", en lui indiquant
            //             ce que vous désirez détecter. Notez que les relations d'héritage sont respectés. Cela
            //             veut donc dire que si vous créez un sous-senseur de "Animal", ce dernier détectera
            //             les "Bunny" et les "Fox", mais pas les "Vegetable" et les "Water". Utilisez donc ce fait
            //             à votre avantage.
            //
            //             Dernier détail à noter : la classe "Sensor" ne fait pas parti de "Unity". C'est une classe
            //             "custom" qui vous a été donné avec ce projet. Vous êtes d'ailleurs invité à la consulter.
            var bunnySensor = Sensor;
            var predatorSensor = bunnySensor.For<IPredator>();
        }

        public IEffect Eat()
        {
            Vitals.Die();

            return new LoseHungerEffect(nutritiveValue);
        }
    }
}