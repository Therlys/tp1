using Harmony;
using UnityEngine;

namespace Game
{
    public static class Finder
    {
        //TODO : Lire ce commentaire.
        //       Voici le Finder. Le Finder permet de trouver les objets dit "Globaux" dans le monde.
        //       Par exemple, vous pouvez obtenir le "SqLiteConnectionFactory" via le "Finder".
        //
        //       Remarquez que le "Finder" n'est nullement performant, car il n'utilise pas de cache. Vous
        //       n'avez pas à corriger ce défaut.
        public static SqLiteConnectionFactory SqLiteConnectionFactory => FindWithTag<SqLiteConnectionFactory>(Tags.MAIN_CONTROLLER);
        public static RandomSeed RandomSeed => FindWithTag<RandomSeed>(Tags.MAIN_CONTROLLER);
        public static PrefabFactory PrefabFactory => FindWithTag<PrefabFactory>(Tags.MAIN_CONTROLLER);
        public static TerrainGrid Terrain => FindWithTag<TerrainGrid>(Tags.TERRAIN);
        public static FloraGrid Flora => FindWithTag<FloraGrid>(Tags.FLORA);
        public static NavigationMesh NavigationMesh => FindWithTag<NavigationMesh>(Tags.NAVIGATION_MESH);
        public static PathFinder PathFinder => FindWithTag<PathFinder>(Tags.NAVIGATION_MESH);

        private static T FindWithTag<T>(string tag)
        {
            return GameObject.FindWithTag(tag).GetComponent<T>();
        }
    }
}