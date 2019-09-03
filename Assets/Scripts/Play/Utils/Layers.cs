using UnityEngine;

namespace Game
{
    public static class Layers
    {
        //TODO : Si jamais vous créez des "Layers", ajoutez vos constantes ici.
        public static readonly LayerMask DEFAULT = LayerMask.NameToLayer("Default");
        public static readonly LayerMask SENSOR = LayerMask.NameToLayer("Sensor");
    }
}