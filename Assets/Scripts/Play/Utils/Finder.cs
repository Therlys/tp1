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

        private static StatisticGenerator statisticGenerator = null;

        private static StatisticRepository statisticRepository = null;
        
        
        private static BunnyDeathEventChannel bunnyDeathEventChannel;
        private static FoxDeathEventChannel foxDeathEventChannel;
        private static BunnySpawnEventChannel bunnySpawnEventChannel;
        private static FoxSpawnEventChannel foxSpawnEventChannel;

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

        public static StatisticRepository GetStatisticRepository()
        {
            return statisticRepository ??
                   (statisticRepository = new StatisticRepository(SqLiteConnectionFactory.GetConnection()));
        }

        public static StatisticGenerator GetStatisticGenerator()
        {
            return statisticGenerator ?? (statisticGenerator = GameObject.FindWithTag(Tags.MAIN_CONTROLLER).GetComponent<StatisticGenerator>());
        }
        
        public static BunnyDeathEventChannel BunnyDeathEventChannel
        {
            get
            {
                if (bunnyDeathEventChannel == null)
                    bunnyDeathEventChannel = GameObject.FindWithTag(Tags.MAIN_CONTROLLER).GetComponent<BunnyDeathEventChannel>();
                return bunnyDeathEventChannel;
            }
        }
        
        public static FoxDeathEventChannel FoxDeathEventChannel
        {
            get
            {
                if (foxDeathEventChannel == null)
                    foxDeathEventChannel = GameObject.FindWithTag(Tags.MAIN_CONTROLLER).GetComponent<FoxDeathEventChannel>();
                return foxDeathEventChannel;
            }
        }
        
        public static BunnySpawnEventChannel BunnySpawnEventChannel
        {
            get
            {
                if (bunnySpawnEventChannel == null)
                    bunnySpawnEventChannel = GameObject.FindWithTag(Tags.MAIN_CONTROLLER).GetComponent<BunnySpawnEventChannel>();
                return bunnySpawnEventChannel;
            }
        }
        
        public static FoxSpawnEventChannel FoxSpawnEventChannel
        {
            get
            {
                if (foxSpawnEventChannel == null)
                    foxSpawnEventChannel = GameObject.FindWithTag(Tags.MAIN_CONTROLLER).GetComponent<FoxSpawnEventChannel>();
                return foxSpawnEventChannel;
            }
        }
    }
}